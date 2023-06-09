using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System.Xml.Linq;
using WuphfApi.Models;
using WuphfApi.Models.DTO;
using WuphfApi.Repository.IRepository;

namespace WuphfApi.Controllers
{
    [Route("api/v{version:apiVersion}/LikeAPI")]
    [ApiController]
    [ApiVersion("1.0")]
    public class LikeController : Controller
    {
        protected APIResponse _response;
        private readonly ILikeRepository _dbLikes;
        private readonly IMapper _mapper;
        public LikeController(ILikeRepository dbLikes, IMapper mapper)
        {
            _dbLikes = dbLikes;
            _mapper = mapper;
            _response = new();
        }
        [HttpGet("Post/{idPost:int}", Name = "GetLikesOfPost")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetLikesOfPost(int idPost)
        {
            try
            {
                var likes = await _dbLikes.GetLikesOfPost(idPost);
                var user = await _dbLikes.GetApplicationUserFromName(User.Identity.Name);
                var likesDto = _mapper.Map<IEnumerable<LikeDTO>>(likes);
                if (user != null)
                {
                    foreach (var item in likesDto)
                    {
                        if (item.FkUser == user.Id)
                        {
                            item.IsUsers = true;
                        }
                        else
                        {
                            item.IsUsers = false;
                        }
                    }
                }
                _response.Result = likesDto;
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpGet("Comment/{idCommento:int}", Name = "GetLikesOfComment")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetLikesOfComment(int idCommento)
        {
            try
            {
                var likes = await _dbLikes.GetLikesOfComment(idCommento);
                var user = await _dbLikes.GetApplicationUserFromName(User.Identity.Name);
                var likesDto = _mapper.Map<IEnumerable<LikeDTO>>(likes);
                if (user != null)
                {
                    foreach (var item in likesDto)
                    {
                        if (item.FkUser == user.Id)
                        {
                            item.IsUsers = true;
                        }
                        else
                        {
                            item.IsUsers = false;
                        }
                    }
                }
                _response.Result = likesDto;
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpPost("Post")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> LikeToPost([FromBody] LikeCreateDTO model)
        {
            try
            {
                var user = await _dbLikes.GetApplicationUserFromName(User.Identity.Name);
                if (user != null && model.FkPost != null)
                {
                    bool success = await _dbLikes.LikeToPost((int)model.FkPost, user.Id);
                    if (success)
                    {
                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
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
        [HttpPost("Comment")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> LikeToComment([FromBody] LikeCreateDTO model)
        {
            try
            {
                var user = await _dbLikes.GetApplicationUserFromName(User.Identity.Name);
                if (user != null && model.FkCommento != null)
                {
                    bool success = await _dbLikes.LikeToComment((int)model.FkCommento, user.Id);
                    if (success)
                    {
                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
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
