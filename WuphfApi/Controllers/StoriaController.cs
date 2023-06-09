using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using WuphfApi.Models;
using WuphfApi.Models.DTO;
using WuphfApi.Repository.IRepository;

namespace WuphfApi.Controllers
{
    [Route("api/v{version:apiVersion}/StoriaAPI")]
    [ApiController]
    [ApiVersion("1.0")]
    public class StoriaController : Controller
    {
        protected APIResponse _response;
        private readonly IStoriaRepository _dbStorie;
        private readonly IMapper _mapper;
        public StoriaController(IStoriaRepository dbStorie, IMapper mapper)
        {
            _dbStorie = dbStorie;
            _mapper = mapper;
            _response = new();
        }
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetStorieOfFollowing(string followerName)
        {
            try
            {
                var storie = await _dbStorie.GetStorieOfFollowing(followerName);
                var storieDto = _mapper.Map<IEnumerable<StoriaDTO>>(storie);
                foreach (var item in storieDto)
                {
                    item.Username = await _dbStorie.GetUsernameFromId(item.FkUser);
                    item.FotoProfilo = await _dbStorie.GetFotoProfiloFromId(item.FkUser);
                }
                //await _dbStorie.UpdatePostsVisualizzatiDaUtente(posts, followerName);
                _response.IsSuccess = true;
                _response.Result = storieDto;
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
        [HttpGet("{id:int}", Name = "GetStoria")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetStoria(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var storia = await _dbStorie.GetAsync(u => u.Id == id);
                if (storia == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                var identity = User.Identity.Name;
                if (identity != null)
                {
                    var user = await _dbStorie.GetApplicationUserFromName(identity);
                    var storiaDto = _mapper.Map<StoriaDTO>(storia);
                    if (storiaDto.FkUser == user.Id)
                    {
                        storiaDto.IsUsers = true;
                    }
                    else
                    {
                        storiaDto.IsUsers = false;
                    }
                    storiaDto.Username = await _dbStorie.GetUsernameFromId(storiaDto.FkUser);
                    storiaDto.FotoProfilo = await _dbStorie.GetFotoProfiloFromId(storiaDto.FkUser);
                    _response.Result = storiaDto;
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    return Ok(_response);
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

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateStoria([FromBody] StoriaCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                var userIdentity = User.Identity;
                if (userIdentity != null)
                {
                    var claimsIdentity = (ClaimsIdentity)userIdentity;
                    var claim = claimsIdentity?.FindFirst(ClaimTypes.Name);
                    if (claim != null)
                    {
                        var user = await _dbStorie.GetApplicationUserFromName(claim.Value);
                        if (user != null)
                        {
                            createDTO.FkUser = user.Id;
                            Storia storia = _mapper.Map<Storia>(createDTO);

                            storia.DataCreazione = DateTime.Now;
                            await _dbStorie.CreateAsync(storia);
                            _response.Result = _mapper.Map<StoriaDTO>(storia);
                            _response.StatusCode = HttpStatusCode.Created;
                            return CreatedAtRoute("GetStoria", new { id = storia.Id }, _response);
                        }
                        _response.IsSuccess = false;
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteStoria")]
        [Authorize]
        public async Task<ActionResult<APIResponse>> DeleteStoria(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var storia = await _dbStorie.GetAsync(u => u.Id == id);
                if (storia == null)
                {
                    return NotFound();
                }
                var userIdentity = User.Identity;
                if (userIdentity != null)
                {
                    var claimsIdentity = (ClaimsIdentity)userIdentity;
                    var claim = claimsIdentity?.FindFirst(ClaimTypes.Name);
                    if (claim != null)
                    {
                        var user = await _dbStorie.GetApplicationUserFromName(claim.Value);
                        if (user != null)
                        {
                            if (storia.FkUser == user.Id)
                            {
                                await _dbStorie.RemoveAsync(storia);
                                _response.StatusCode = HttpStatusCode.NoContent;
                                _response.IsSuccess = true;
                                return Ok(_response);
                            }
                        }
                        _response.IsSuccess = false;
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
