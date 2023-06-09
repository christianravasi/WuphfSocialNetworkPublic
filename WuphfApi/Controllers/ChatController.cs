using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Xml.Linq;
using WuphfApi.Models;
using WuphfApi.Models.DTO;
using WuphfApi.Repository.IRepository;

namespace WuphfApi.Controllers
{
    [Route("api/v{version:apiVersion}/ChatAPI")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ChatController : Controller
    {
        protected APIResponse _response;
        private readonly IChatRepository _dbChat;
        private readonly IMessaggioRepository _dbMessaggi;
        private readonly IMapper _mapper;
        public ChatController(IChatRepository dbChat, IMapper mapper, IMessaggioRepository dbMessaggi)
        {
            _dbChat = dbChat;
            _mapper = mapper;
            _response = new();
            _dbMessaggi = dbMessaggi;
        }

        [HttpGet("{id:int}", Name = "GetChat")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetChat(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var chat = await _dbChat.GetChat(id);
                if (chat == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                if (chat.Username1 != User.Identity.Name && chat.Username2 != User.Identity.Name)
                {
                    _response.StatusCode = HttpStatusCode.Forbidden;
                    return BadRequest(_response);
                }
                var chatDto = _mapper.Map<ChatDTO>(chat);
                var messaggi = await _dbMessaggi.GetMessaggiOfChat(chat.Id);
                chatDto.Messaggi = _mapper.Map<IEnumerable<MessaggioDTO>>(messaggi);
                _response.Result = chatDto;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetChatsOfUser()
        {
            try
            {
                if (User.Identity.Name != null)
                {
                    var chats = await _dbChat.GetChatsOfUser(User.Identity.Name);
                    var chatsDto = _mapper.Map<IEnumerable<ChatDTO>>(chats);
                    //foreach (var chat in chatsDto)
                    //{
                    //    var messaggi = await _dbMessaggi.GetMessaggiOfChat(chat.Id);
                    //    chat.Messaggi = _mapper.Map<IEnumerable<MessaggioDTO>>(messaggi);
                    //}                    
                    _response.Result = chatsDto;
                    _response.StatusCode = HttpStatusCode.OK;
                    return Ok(_response);
                }
                
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpGet("ChatsWithMessages")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetChatsOfUserWithMessages()
        {
            try
            {
                if (User.Identity.Name != null)
                {
                    var chats = await _dbChat.GetChatsOfUser(User.Identity.Name);
                    var chatsDto = _mapper.Map<IEnumerable<ChatDTO>>(chats);
                    foreach (var chat in chatsDto)
                    {
                        var messaggi = await _dbMessaggi.GetMessaggiOfChat(chat.Id);
                        chat.Messaggi = _mapper.Map<IEnumerable<MessaggioDTO>>(messaggi);
                    }
                    _response.Result = chatsDto;
                    _response.StatusCode = HttpStatusCode.OK;
                    return Ok(_response);
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateOrDeleteChat([FromBody] ChatCreateDTO model)
        {
            try
            {
                if (User.Identity.Name != null)
                {
                    var chat = await _dbChat.CreateOrDeleteChat(User.Identity.Name, model.FkUser2);
                    if (chat != null)
                    {
                        var test = new Chat();
                        if (test != chat)
                        {
                            _response.Result = _mapper.Map<ChatDTO>(chat);
                            _response.StatusCode = HttpStatusCode.Created;
                            return CreatedAtRoute("GetChat", new { id = chat.Id }, _response);
                        }
                        _response.StatusCode = HttpStatusCode.NoContent;
                        _response.IsSuccess = true;
                        return Ok(_response);
                    }
                }
                
                _response.IsSuccess = false;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
