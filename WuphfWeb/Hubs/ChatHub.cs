using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using WuphfUtility;
using WuphfWeb.Models;
using WuphfWeb.Models.DTO;
using WuphfWeb.Services.IServices;

namespace WuphfWeb.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IConfiguration _config;
        private readonly IChatService _chatService;
        private readonly IMapper _mapper;
        private readonly IUtenteService _utenteService;
        public ChatHub(IChatService chatService, IMapper mapper, IConfiguration config, IUtenteService utenteService)
        {
            _chatService = chatService;
            _mapper = mapper;
            _config = config;
            _utenteService = utenteService;
        }
        public override Task OnConnectedAsync()
        {
            Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            return base.OnConnectedAsync();
        }
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", Context.User.Identity.Name, message);
        }

        public async Task<Task> SendMessageToGroup(string receiver, string message, string idChat, string token)
        {
            var response = await _chatService.CreateMessageAsync<APIResponse>(new Models.DTO.MessaggioCreateDTO()
            {
                IdChat = int.Parse(idChat),
                Sender = Context.User.Identity.Name,
                Receiver = receiver,
                Testo = message
            }, token);
            if (response.IsSuccess)
            {
                var bot_token = _config["Notifications:ApiKey"];
                string updateUrl = $"https://api.telegram.org/bot{bot_token}/getUpdates";
                HttpClient client = new();
                var risposta = await client.GetAsync($"{updateUrl}");
                Rootobject responseT = await risposta.Content.ReadFromJsonAsync<Rootobject>();
                
                response = await _utenteService.GetUser<APIResponse>(receiver, token);
                var user = JsonConvert.DeserializeObject<UserDTO>(Convert.ToString(response.Result));
                string chat_id = $"{user.TelegramChatId}";
                string userToken = user.TelegramToken;

                if (chat_id == $"{0}")
                {
                    foreach (var item in responseT.result)
                    {
                        if (item.message.text.Split(' ').Length == 2 && item.message.text.Split(' ')[1] == userToken)
                        {
                            chat_id = item.message.chat.id.ToString();
                            user.TelegramChatId = int.Parse(chat_id);
                            var r = await _utenteService.TelegramChatId<APIResponse>(user, token);
                            break;
                        }
                    }                   
                }
                if (chat_id != $"{0}")
                {
                    string notification_text = $"Hai ricevuto un messaggio!";
                    string messagioUrl = $"https://api.telegram.org/bot{bot_token}/sendMessage?chat_id={chat_id}&text={notification_text}&parse_mode=markdown";
                    var notifica = await client.GetAsync($"{messagioUrl}");
                }
            }
            await Clients.Group(Context.User.Identity.Name).SendAsync("ReceiveMessage", Context.User.Identity.Name, message);
            return Clients.Group(receiver).SendAsync("ReceiveMessage", Context.User.Identity.Name, message);
        }
    }
}
