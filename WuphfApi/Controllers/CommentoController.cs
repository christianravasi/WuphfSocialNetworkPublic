using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using WuphfApi.Models;
using WuphfApi.Repository.IRepository;
using WuphfApi.Models.DTO;
using System.Security.Claims;
using System.Xml.Linq;

namespace WuphfApi.Controllers
{
    [Route("api/v{version:apiVersion}/CommentoAPI")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CommentoController : Controller
    {
        protected APIResponse _response;
        private readonly ICommentoRepository _dbCommenti;
        private readonly ILikeRepository _dbLike;
        private readonly IMapper _mapper;
        public CommentoController(ICommentoRepository dbCommenti, IMapper mapper, ILikeRepository dbLike)
        {
            _dbCommenti = dbCommenti;
            _mapper = mapper;
            _response = new();
            _dbLike = dbLike;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetCommenti()
        {
            try
            {
                IEnumerable<Commento> commenti = await _dbCommenti.GetAllAsync();

                _response.Result = _mapper.Map<List<CommentoDTO>>(commenti);
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

        [HttpGet("{id:int}", Name = "GetCommento")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetCommento(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var commento = await _dbCommenti.GetAsync(u => u.Id == id);
                if (commento == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<CommentoDTO>(commento);
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

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateCommento([FromBody] CommentoCreateDTO createDTO)
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
                        var user = await _dbCommenti.GetApplicationUserFromName(claim.Value);
                        if (user != null)
                        {
                            createDTO.FkUser = user.Id;
                            Commento commento = _mapper.Map<Commento>(createDTO);

                            commento.DataCreazione = DateTime.Now;
                            await _dbCommenti.CreateAsync(commento);
                            _response.Result = _mapper.Map<CommentoDTO>(commento);
                            _response.StatusCode = HttpStatusCode.Created;
                            return CreatedAtRoute("GetCommento", new { id = commento.Id }, _response);
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
        [HttpDelete("{id:int}", Name = "DeleteCommento")]
        [Authorize]
        public async Task<ActionResult<APIResponse>> DeleteCommento(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var commento = await _dbCommenti.GetAsync(u => u.Id == id);
                if (commento == null)
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
                        var user = await _dbCommenti.GetApplicationUserFromName(claim.Value);
                        if (user != null)
                        {
                            if (commento.FkUser == user.Id)
                            {
                                await _dbLike.RemoveLikesFromComment(commento.Id);
                                await _dbCommenti.RemoveAsync(commento);
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
