using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class LikeAndCount
    {
        public int? Count { get; set; }
        public bool IsUsers { get; set; }
    }
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILikeService _likeService;
        private readonly IPostService _postService;
        private readonly IStoriaService _storiaService;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IPostService postService, IMapper mapper, ILikeService likeService, IStoriaService storiaService)
        {
            _logger = logger;
            _postService = postService;
            _mapper = mapper;
            _likeService = likeService;
            _storiaService = storiaService;
        }

        public async Task<IActionResult> Index()
        {
            ListPostVM listPostVM = new ListPostVM();
            List<PostDTO> list = new();
            var userIdentity = User.Identity.Name;
            if (User.Identity.IsAuthenticated && userIdentity != null)
            {
                var response = await _postService.GetPostsOfFollowing<APIResponse>(userIdentity, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<PostDTO>>(Convert.ToString(response.Result));
                    foreach (var item in list)
                    {
                        var likeAndCount = await GetLikesOfPost(item.Id);
                        item.LikeCount = likeAndCount.Count;
                        item.IsLiked = likeAndCount.IsUsers;
                    }
                    listPostVM.Posts = list;
                }
                var responseStorie = await _storiaService.GetStorieOfFollowing<APIResponse>(userIdentity, HttpContext.Session.GetString(SD.SessionToken));
                if (responseStorie != null && responseStorie.IsSuccess)
                {
                    listPostVM.Storie = JsonConvert.DeserializeObject<List<StoriaDTO>>(Convert.ToString(responseStorie.Result));
                }
            }     
            return View(listPostVM);
        }

        public async Task<IActionResult> GetNextPosts()
        {
            List<PostDTO> list = new();
            var userIdentity = User.Identity.Name;
            if (User.Identity.IsAuthenticated && userIdentity != null)
            {
                var response = await _postService.GetPostsOfFollowing<APIResponse>(userIdentity, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<PostDTO>>(Convert.ToString(response.Result));
                    foreach (var item in list)
                    {
                        var likeAndCount = await GetLikesOfPost(item.Id);
                        item.LikeCount = likeAndCount.Count;
                        item.IsLiked = likeAndCount.IsUsers;
                    }
                }
            }
            return Json(new { data = list });
        }

        [Authorize]
        public async Task<LikeAndCount?> GetLikesOfPost(int idPost)
        {
            var response = await _likeService.GetLikesOfPost<APIResponse>(idPost, HttpContext.Session.GetString(SD.SessionToken));
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
        public async Task<bool> LikeToPost(int id)
        {
            var response = await _likeService.LikeToPost<APIResponse>(id, "ciao", HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                return true;
            }
            return false;
        }
        [Authorize]
        public async Task<bool> LikeToComment(int id)
        {
            var response = await _likeService.LikeToComment<APIResponse>(id, "ciao", HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                return true;
            }
            return false;
        }

        //[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<bool> LikeToPost(ListPostVM model)
        //{
        //    var response = await _likeService.LikeToPost<APIResponse>(model.PostId, "ciao", HttpContext.Session.GetString(SD.SessionToken));
        //    if (response != null && response.IsSuccess)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        //public async Task<IActionResult> Index()
        //{
        //    List<PostDTO> list = new();

        //    var response = await _postService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        //    if (response != null && response.IsSuccess)
        //    {
        //        list = JsonConvert.DeserializeObject<List<PostDTO>>(Convert.ToString(response.Result));
        //    }
        //    return View(list);
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}