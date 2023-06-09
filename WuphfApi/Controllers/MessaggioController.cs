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
    [Route("api/v{version:apiVersion}/MessaggioAPI")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MessaggioController : Controller
    {
        protected APIResponse _response;
        private readonly IMessaggioRepository _dbMessaggio;
        private readonly IMapper _mapper;
        public MessaggioController(IMessaggioRepository dbMessaggio, IMapper mapper)
        {
            _dbMessaggio = dbMessaggio;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet("{id:int}", Name = "GetMessaggio")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetMessaggio(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var messaggio = await _dbMessaggio.GetAsync(m => m.Id == id);
                if (messaggio == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<MessaggioDTO>(messaggio);
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
        public async Task<ActionResult<APIResponse>> CreateMessaggio([FromBody] MessaggioCreateDTO model)
        {
            try
            {
                if (User.Identity.Name != null)
                {
                    var messaggio = await _dbMessaggio.CreateMessaggio(User.Identity.Name, model.Receiver, model.IdChat, model.Testo);
                    if (messaggio != null)
                    {
                        _response.Result = _mapper.Map<MessaggioDTO>(messaggio);
                        _response.StatusCode = HttpStatusCode.Created;
                        return CreatedAtRoute("GetMessaggio", new { id = messaggio.Id }, _response);
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

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteMessaggio")]
        [Authorize]
        public async Task<ActionResult<APIResponse>> DeleteMessaggio(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var messaggio = await _dbMessaggio.GetAsync(u => u.Id == id);
                if (messaggio == null)
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
                        var user = await _dbMessaggio.GetApplicationUserFromName(claim.Value);
                        if (user != null)
                        {
                            if (messaggio.Sender == user.Id)
                            {
                                await _dbMessaggio.RemoveAsync(messaggio);
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
