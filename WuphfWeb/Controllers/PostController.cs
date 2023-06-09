using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using WuphfUtility;
using WuphfWeb.Models;
using WuphfWeb.Models.DTO;
using WuphfWeb.Models.VM;
using WuphfWeb.Services.IServices;

namespace WuphfWeb.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly ILikeService _likeService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;
        public PostController(IPostService postService, IMapper mapper, IWebHostEnvironment hostEnvironment, ILikeService likeService)
        {
            _postService = postService;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
            _likeService = likeService;
        }
        [Authorize]
        public async Task<IActionResult> GetPost(int id)
        {
            var response = await _postService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                PostVM model = new PostVM();
                model.Post = JsonConvert.DeserializeObject<PostDTO>(Convert.ToString(response.Result));
                var responseCommenti = await _postService.GetCommentsFromPost<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    model.Commenti = JsonConvert.DeserializeObject<List<CommentoDTO>>(Convert.ToString(responseCommenti.Result));
                    foreach (var item in model.Commenti)
                    {
                        var likeAndCount = await GetLikesOfComment(item.Id);
                        item.LikeCount = likeAndCount.Count;
                        item.IsLiked = likeAndCount.IsUsers;
                    }
                }
                model.Post.LikeCount = await GetLikesOfPost(model.Post.Id);
                return View(model);
            }
            return NotFound();
        }
        [Authorize]
        public IActionResult CreatePost()
        {
            var postDTO = new PostCreateDTO();
            return View(postDTO);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(PostCreateDTO model, IFormFile? fileFoto, IFormFile? fileVideo)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (fileFoto != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploadDir = Path.Combine(wwwRootPath, "images", "foto");
                    var fileExtension = Path.GetExtension(fileFoto.FileName);
                    var filePath = Path.Combine(uploadDir, fileName + fileExtension);
                    var fileUrlString = filePath[wwwRootPath.Length..].Replace(@"\\", @"\");
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        fileFoto.CopyTo(fileStream);
                    }
                    model.Immagine = fileUrlString;
                }
                if (fileVideo != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploadDir = Path.Combine(wwwRootPath, "images", "video");
                    var fileExtension = Path.GetExtension(fileVideo.FileName);
                    var filePath = Path.Combine(uploadDir, fileName + fileExtension);
                    var fileUrlString = filePath[wwwRootPath.Length..].Replace(@"\\", @"\");
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        fileVideo.CopyTo(fileStream);
                    }
                    model.Video = fileUrlString;
                }
                var response = await _postService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Post created successfully";
                    return RedirectToAction("Index", "Home");
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
        [Authorize]
        public async Task<int?> GetLikesOfPost(int idPost)
        {
            var response = await _likeService.GetLikesOfPost<APIResponse>(idPost, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                IEnumerable<LikeDTO>? likes = JsonConvert.DeserializeObject<IEnumerable<LikeDTO>?>(Convert.ToString(response.Result));
                if (likes != null)
                {
                    var count = likes.Count();
                    return count;
                }
                return 0;
            }
            return null;
        }

        [Authorize]
        public async Task<LikeAndCount?> GetLikesOfComment(int idCommento)
        {
            var response = await _likeService.GetLikesOfComment<APIResponse>(idCommento, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                IEnumerable<LikeDTO>? likes = JsonConvert.DeserializeObject<IEnumerable<LikeDTO>?>(Convert.ToString(response.Result));
                if (likes != null)
                {
                    LikeAndCount likeAndCount = new LikeAndCount();
                    foreach (var item in likes)
                    {
                        if (item.IsUsers == true)
                        {
                            likeAndCount.IsUsers = true;
                            break;
                        }
                    }
                    likeAndCount.Count = likes.Count();
                    return likeAndCount;
                }
                return new LikeAndCount();
            }
            return null;
        }

        [Authorize]
        public async Task<IActionResult> UpdatePost(int id)
        {
            var response = await _postService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                PostDTO model = JsonConvert.DeserializeObject<PostDTO>(Convert.ToString(response.Result));
                if (model.IsUsers)
                {
                    return View(_mapper.Map<PostUpdateDTO>(model));
                }
            }
            return NotFound();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePost(PostUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _postService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Post updated successfully";
                    return RedirectToAction("Index", "Home");
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id != null)
            {
                var response = await _postService.DeleteAsync<APIResponse>((int)id, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Post deleted successfully";
                    return Json(new { success = true, message = "Eliminazione completata" });
                }
            }
            TempData["error"] = "Error encountered.";
            return Json(new { success = false, message = "C'è stato un errore..." });
        }
    }
}
