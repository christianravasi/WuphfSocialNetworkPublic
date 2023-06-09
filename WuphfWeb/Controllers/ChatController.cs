using System;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WuphfUtility;
using WuphfWeb.Models;
using WuphfWeb.Models.DTO;
using WuphfWeb.Models.VM;
using WuphfWeb.Services;
using WuphfWeb.Services.IServices;

namespace WuphfWeb.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ILogger<ChatController> _logger;
        private readonly IChatService _chatService;
        private readonly IMapper _mapper;

        public ChatController(ILogger<ChatController> logger, IChatService chatService, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _chatService = chatService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<ChatDTO> chat = new List<ChatDTO>();
            var response = await _chatService.GetChatsOfUserAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                chat.AddRange(JsonConvert.DeserializeObject<List<ChatDTO>>(Convert.ToString(response.Result)));
                foreach (var item in chat)
                {
                    if (User.Identity.Name == item.Username1)
                    {
                        item.IsUser1 = true;
                    }
                    else
                    {
                        item.IsUser1 = false;
                    }
                }
            }
            return View(chat);
        }

        public async Task<IActionResult> Utente(int id)
        {
            var response = await _chatService.GetChatAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                var chat = JsonConvert.DeserializeObject<ChatDTO>(Convert.ToString(response.Result));
                if (User.Identity.Name == chat.Username1)
                {
                    chat.IsUser1 = true;
                }
                else
                {
                    chat.IsUser1 = false;
                }
                return View(new ChatVM() { Chat = chat, Token = HttpContext.Session.GetString(SD.SessionToken) });
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrDeleteChat(UserVM userVM)
        {
            var model = userVM.ChatCreateDTO;
            model.FkUser1 = User.Identity.Name;
            if (ModelState.IsValid && model.FkUser2 != string.Empty)
            {
                var response = await _chatService.CreateOrDeleteChatAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            if (id != 0)
            {
                var response= await _chatService.DeleteMessageAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Messaggio deleted successfully";
                    return Json(new { success = true, message = "Eliminazione completata" });
                }
            }
            TempData["error"] = "Error encountered.";
            return Json(new { success = false, message = "C'è stato un errore..." });
        }
    }
}
