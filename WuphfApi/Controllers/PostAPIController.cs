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
using Microsoft.Extensions.Hosting;
using System.Xml.Linq;

namespace WuphfApi.Controllers
{
    [Route("api/v{version:apiVersion}/PostAPI")]
    [ApiController]
    [ApiVersion("1.0")]
    public class PostAPIController : Controller
    {
        protected APIResponse _response;
        private readonly IPostRepository _dbPosts;
        private readonly ILikeRepository _dbLike;
        private readonly IMapper _mapper;
        public PostAPIController(IPostRepository dbPosts, IMapper mapper, ILikeRepository dbLike)
        {
            _dbPosts = dbPosts;
            _mapper = mapper;
            _response = new();
            _dbLike = dbLike;
        }

        [HttpGet]
        [Authorize]
        //[ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetPosts()
        {
            try
            {
                IEnumerable<Post> posts = await _dbPosts.GetAllAsync();
                var identity = User.Identity.Name;
                if (identity != null)
                {
                    var user = await _dbPosts.GetApplicationUserFromName(identity);
                    var postsDto = _mapper.Map<List<PostDTO>>(posts);
                    foreach (var item in postsDto)
                    {
                        if (item.FkUser == user.Id)
                        {
                            item.IsUsers = true;
                        }
                        else
                        {
                            item.IsUsers = false;
                        }
                        item.Username = await _dbPosts.GetUsernameFromId(item.FkUser);
                    }
                    _response.Result = postsDto;
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    return Ok(_response);
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

        [HttpGet("{id:int}", Name = "GetPost")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetPost(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var post = await _dbPosts.GetAsync(u => u.Id == id);
                if (post == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                var identity = User.Identity.Name;
                if (identity != null)
                {
                    var user = await _dbPosts.GetApplicationUserFromName(identity);
                    var postDto = _mapper.Map<PostDTO>(post);
                    if (postDto.FkUser == user.Id)
                    {
                        postDto.IsUsers = true;
                    }
                    else
                    {
                        postDto.IsUsers = false;
                    }
                    postDto.Username = await _dbPosts.GetUsernameFromId(postDto.FkUser);
                    postDto.FotoProfilo = await _dbPosts.GetFotoProfiloFromId(postDto.FkUser);
                    _response.Result = postDto;
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

        [HttpGet("Commenti/{idPost:int}", Name = "GetCommentiFromPost")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetCommentiFromPost(int idPost)
        {
            try
            {
                var commenti = await _dbPosts.GetCommentiFromPost(idPost);

                var userIdentity = User.Identity;
                if (userIdentity != null)
                {
                    var claimsIdentity = (ClaimsIdentity)userIdentity;
                    var claim = claimsIdentity?.FindFirst(ClaimTypes.Name);
                    if (claim != null)
                    {
                        var user = await _dbPosts.GetApplicationUserFromName(claim.Value);
                        if (user != null)
                        {
                            var comments = _mapper.Map<List<CommentoDTO>>(commenti);
                            foreach (var commento in comments)
                            {
                                if (commento.FkUser == user.Id)
                                {
                                    commento.IsUsers = true;
                                }
                                else
                                {
                                    commento.IsUsers = false;
                                }
                                commento.Username = await _dbPosts.GetUsernameFromId(commento.FkUser);
                                commento.FotoProfilo = await _dbPosts.GetFotoProfiloFromId(commento.FkUser);
                            }
                            _response.Result = comments;
                            _response.StatusCode = HttpStatusCode.OK;
                            return Ok(_response);
                        }
                        _response.IsSuccess = false;
                    }
                }               
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpGet("Utente", Name = "GetPostsOfUser")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetPostsOfUser(string userName)
        {
            try
            {
                var posts = await _dbPosts.GetPostsOfUser(userName);

                _response.Result = _mapper.Map<IEnumerable<PostDTO>>(posts);
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
        [HttpGet("PostSeguiti", Name = "GetPostsOfFollowing")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetPostsOfFollowing(string followerName)
        {
            try
            {
                var posts = await _dbPosts.GetPostsOfFollowing(followerName);
                var postsDto = _mapper.Map<IEnumerable<PostDTO>>(posts);
                foreach (var item in postsDto)
                {
                    item.Username = await _dbPosts.GetUsernameFromId(item.FkUser);
                    item.FotoProfilo = await _dbPosts.GetFotoProfiloFromId(item.FkUser);
                }
                await _dbPosts.UpdatePostsVisualizzatiDaUtente(posts, followerName);
                _response.Result = postsDto;
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
        [HttpPost("Visualizzato")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> Visualizzato([FromBody] VisualizzatoDTO model)
        {
            try
            {
                if (User.Identity.Name == model.Username)
                {
                    var success = await _dbPosts.UpdatePostVisualizzatoDaUtente(model.FkPost, model.Username);
                    if (success)
                    {
                        _response.IsSuccess = true;
                        _response.Result = success;
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
        //[HttpGet]
        //[Authorize]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult<APIResponse>> GetPostsWithComments()
        //{
        //    try
        //    {
        //        IEnumerable<Post> posts = await _dbPosts.GetAllAsync();
        //        var comments = new List<Commento>();
        //        foreach (var post in posts)
        //        {
        //            comments.AddRange(await _dbPosts.GetCommentiFromPost(post.Id));
        //        }


        //        _response.Result = _mapper.Map<List<CommentoDTO>>(comments);
        //        _response.StatusCode = HttpStatusCode.OK;
        //        return Ok(_response);
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsSuccess = false;
        //        _response.ErrorMessages = new List<string>() { ex.ToString() };
        //    }
        //    return _response;
        //}

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreatePost([FromBody] PostCreateDTO createDTO)
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
                        var user = await _dbPosts.GetApplicationUserFromName(claim.Value);
                        if (user != null)
                        {
                            createDTO.FkUser = user.Id;
                            Post post = _mapper.Map<Post>(createDTO);

                            post.DataCreazione = DateTime.Now;
                            await _dbPosts.CreateAsync(post);
                            _response.Result = _mapper.Map<PostDTO>(post);
                            _response.StatusCode = HttpStatusCode.Created;
                            return CreatedAtRoute("GetPost", new { id = post.Id }, _response);
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
        [HttpDelete("{id:int}", Name = "DeletePost")]
        [Authorize]
        public async Task<ActionResult<APIResponse>> DeletePost(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var post = await _dbPosts.GetAsync(u => u.Id == id);
                if (post == null)
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
                        var user = await _dbPosts.GetApplicationUserFromName(claim.Value);
                        if (user != null)
                        {
                            if (post.FkUser == user.Id)
                            {
                                await _dbLike.RemoveLikesFromPost(post.Id);
                                await _dbPosts.RemoveAsync(post);
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
        [Authorize]
        [HttpPut("{id:int}", Name = "UpdatePost")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdatePost(int id, [FromBody] PostUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }

                var userIdentity = User.Identity;
                if (userIdentity != null)
                {
                    var claimsIdentity = (ClaimsIdentity)userIdentity;
                    var claim = claimsIdentity?.FindFirst(ClaimTypes.Name);
                    if (claim != null)
                    {
                        var user = await _dbPosts.GetApplicationUserFromName(claim.Value);
                        if (user != null)
                        {
                            Post post = _mapper.Map<Post>(updateDTO);

                            post.DataUpdate = DateTime.Now;
                            post.FkUser = user.Id;
                            await _dbPosts.UpdateAsync(post);
                            _response.StatusCode = HttpStatusCode.NoContent;
                            _response.IsSuccess = true;
                            return Ok(_response);
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

        //[HttpPatch("{id:int}", Name = "UpdatePartialPost")]
        //[Authorize]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> UpdatePartialPost(int id, JsonPatchDocument<PostUpdateDTO> patchDTO)
        //{
        //    if (patchDTO == null || id == 0)
        //    {
        //        return BadRequest();
        //    }
        //    var villa = await _dbPosts.GetAsync(u => u.Id == id, tracked: false);

        //    PostUpdateDTO villaDTO = _mapper.Map<PostUpdateDTO>(villa);


        //    if (villa == null)
        //    {
        //        return BadRequest();
        //    }
        //    patchDTO.ApplyTo(villaDTO, ModelState);
        //    Post model = _mapper.Map<Post>(villaDTO);

        //    await _dbPosts.UpdateAsync(model);

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    return NoContent();
        //}
    }
}
