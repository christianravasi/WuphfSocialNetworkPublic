using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using WuphfApi.Models;
using WuphfApi.Models.DTO;
using WuphfApi.Repository.IRepository;


namespace WuphfApi.Controllers
{
    [Route("api/v{version:apiVersion}/Profile")]
    [ApiController]
    [ApiVersionNeutral]
    public class UtenteController : Controller
    {
        private readonly IUtenteRepository _utenteRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public UtenteController(IUtenteRepository userRepo, IMapper mapper)
        {
            _utenteRepo = userRepo;
            _mapper = mapper;
            _response = new();
        }
        [Authorize]
        [HttpPost("SaveChatId", Name = "SaveChatId")]
        public async Task<ActionResult<APIResponse>> SaveChatId([FromBody] UserDTO userDto)
        {
            try
            {
                string username = userDto.UserName;
                int chatId = userDto.TelegramChatId;
                if (username == string.Empty)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var success = await _utenteRepo.CreateTelegramChatId(username, chatId);
                if (!success)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.IsSuccess = success;
                _response.Result = success;
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

        [HttpGet("SimilarUsernames", Name = "FindSimilarUsernames")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> FindSimilarUsernames(string username)
        {
            try
            {
                if (username == string.Empty)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var users = _utenteRepo.FindSimilarUsers(username);
                if (users == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                var userDto = _mapper.Map<List<UserDTO>>(users.Result);
                _response.Result = userDto;
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

        [HttpGet(Name = "GetUser")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetUser(string username)
        {
            try
            {
                if (username == string.Empty)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var user = await _utenteRepo.FindUser(username);
                if (user == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                var userDto = _mapper.Map<UserDTO>(user);
                var userIdentity = User.Identity;
                if (userIdentity != null)
                {
                    var claimsIdentity = (ClaimsIdentity)userIdentity;
                    var claim = claimsIdentity?.FindFirst(ClaimTypes.Name);
                    if (claim != null)
                    {
                        userDto.IsSeguito = await _utenteRepo.IsSeguito(username, claim.Value);
                        userDto.IsChat = await _utenteRepo.IsChat(username, claim.Value);
                        userDto.NumeroFollower = await _utenteRepo.NumeroFollower(username);
                        userDto.NumeroFollowing = await _utenteRepo.NumeroFollowing(username);
                        _response.Result = userDto;
                        _response.StatusCode = HttpStatusCode.OK;
                        return Ok(_response);
                    }
                }
                _response.IsSuccess = false;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [Authorize]
        [HttpPut("UpdateProfilePicture")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<APIResponse> UpdateProfilePicture([FromBody] ProfilePictureUpdateDTO model)
        {
            var userIdentity = User.Identity;
            if (userIdentity != null)
            {
                var claimsIdentity = (ClaimsIdentity)userIdentity;
                var claim = claimsIdentity?.FindFirst(ClaimTypes.Name);
                if (claim != null)
                {
                    var userDb = await _utenteRepo.FindUser(model.UserId);
                    if (claim.Value == userDb.UserName)
                    {
                        var response = await _utenteRepo.UpdateProfilePicture(model);
                        if (response)
                        {
                            _response.StatusCode = HttpStatusCode.OK;
                            _response.IsSuccess = true;
                            return _response;
                        }
                    }
                }
            }
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessages.Add("Qualcosa è andato storto...");
            return _response;
        }
        [Authorize]
        [HttpPut("UserFollowers")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<APIResponse> UserFollowers([FromBody] SegueDTO segueDTO)
        {
            try
            {
                string userName = segueDTO.userName;
                string followerName = segueDTO.followerName;
                var userIdentity = User.Identity;
                if (userIdentity != null)
                {
                    var claimsIdentity = (ClaimsIdentity)userIdentity;
                    var claim = claimsIdentity?.FindFirst(ClaimTypes.Name);
                    if (claim != null)
                    {
                        var follower = await _utenteRepo.FindUser(claim.Value);
                        if (follower != null && follower.UserName == followerName)
                        {
                            var result = await _utenteRepo.UserFollowers(userName, followerName);
                            if (result.UserName == followerName)
                            {
                                _response.StatusCode = HttpStatusCode.OK;
                                _response.IsSuccess = true;
                                return _response;
                            }                    
                        }
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.ErrorMessages.Add("Qualcosa è andato storto...");
                    }
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
    }
}
