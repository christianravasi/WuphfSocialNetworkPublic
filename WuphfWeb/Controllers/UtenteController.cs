using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using WuphfUtility;
using WuphfWeb.Models;
using WuphfWeb.Models.DTO;
using WuphfWeb.Models.VM;
using WuphfWeb.Services;
using WuphfWeb.Services.IServices;

namespace WuphfWeb.Controllers
{
    public class UtenteController : Controller
    {
        private readonly IUtenteService _utenteService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILikeService _likeService;

        public UtenteController(IUtenteService utenteService, IWebHostEnvironment hostEnvironment, ILikeService likeService)
        {
            _utenteService = utenteService;
            _hostEnvironment = hostEnvironment;
            _likeService = likeService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> FindSimilarUsernames(string username)
        {
            var response = await _utenteService.FindSimilarUsernames<APIResponse>(username, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                var users = JsonConvert.DeserializeObject<List<UserDTO>>(Convert.ToString(response.Result));
                return Json(new { success = true, data = users });
                //return PartialView("_SearchResult", users);
            }
            return Json(new { success = false, message = "Qualcosa è andato storto..." });
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
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var response = await _utenteService.GetUser<APIResponse>(User.Identity.Name, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                var user = JsonConvert.DeserializeObject<UserDTO>(Convert.ToString(response.Result));
                ProfileVM profileVM = new ProfileVM();
                profileVM.User = user;
                profileVM.ProfilePictureUpdateDTO = new ProfilePictureUpdateDTO();
                return View(profileVM);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> VisualizzaUtente(string username)
        {
            var response = await _utenteService.GetUser<APIResponse>(username, HttpContext.Session.GetString(SD.SessionToken));
            var responsePosts = await _utenteService.GetPostsOfUser<APIResponse>(username, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess && responsePosts != null && responsePosts.IsSuccess)
            {
                UserVM userVM = new UserVM()
                {
                    UserDTO = JsonConvert.DeserializeObject<UserDTO>(Convert.ToString(response.Result)),
                    SegueDTO = new SegueDTO(),
                    Posts = JsonConvert.DeserializeObject<IEnumerable<PostDTO>>(Convert.ToString(responsePosts.Result))
                };
                userVM.UserDTO.NumeroPost = userVM.Posts.Count();
                foreach (var item in userVM.Posts)
                {
                    var likeAndCount = await GetLikesOfPost(item.Id);
                    item.LikeCount = likeAndCount.Count;
                    item.IsLiked = likeAndCount.IsUsers;
                }
                return View(userVM);
            }
            TempData["error"] = "An error occurred.";
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Follow(UserVM data)
        {
            string userName = data.SegueDTO.userName;
            var user = User.Identity;
            var claims = user?.Name;
            string followerName = string.Empty;
            if (claims != null)
            {
                followerName = claims;
            }
            var response = await _utenteService.Follow<APIResponse>(userName, followerName, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Azione avvenuta con successo!";
                return RedirectToAction("Index", "Home");
            }
            TempData["error"] = "Qualcosa è andato storto...";
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfilePicture(ProfilePictureUpdateDTO obj, IFormFile? fileFoto)
        {
            var user = User.Identity;
            var claims = user?.Name;
            if (claims != null)
            {
                obj.UserId = claims;
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (fileFoto != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploadDir = Path.Combine(wwwRootPath, "images", "fotoProfilo");
                    var fileExtension = Path.GetExtension(fileFoto.FileName);
                    var filePath = Path.Combine(uploadDir, fileName + fileExtension);
                    var fileUrlString = filePath[wwwRootPath.Length..].Replace(@"\\", @"\");
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        fileFoto.CopyTo(fileStream);
                    }
                    obj.FotoProfilo = fileUrlString;
                }
                var response = await _utenteService.UpdateProfilePicture<APIResponse>(obj, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Modifica effettuata correttamente";
                    return RedirectToAction(nameof(Profile));
                }
            }
            TempData["error"] = "Errore";
            return RedirectToAction(nameof(Profile));
        }
    }
}
