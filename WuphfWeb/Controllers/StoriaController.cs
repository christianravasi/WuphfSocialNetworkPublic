using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using WuphfUtility;
using WuphfWeb.Models;
using WuphfWeb.Models.DTO;
using WuphfWeb.Models.VM;
using WuphfWeb.Services;
using WuphfWeb.Services.IServices;

namespace WuphfWeb.Controllers
{
    public class StoriaController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStoriaService _storiaService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;

        public StoriaController(ILogger<HomeController> logger, IMapper mapper, IStoriaService storiaService, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _mapper = mapper;
            _storiaService = storiaService;
            _hostEnvironment = hostEnvironment;
        }
        [Authorize]
        public async Task<IActionResult> GetStoria(int id)
        {
            var response = await _storiaService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<StoriaDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }
        [HttpGet]
        public async Task<IActionResult> GetStoriaJson(int id)
        {
            var response = await _storiaService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<StoriaDTO>(Convert.ToString(response.Result));
                return Json(new { success = true, model = model });
            }
            return NotFound();
        }
        [Authorize]
        public IActionResult CreateStoria()
        {
            var postDTO = new StoriaCreateDTO();
            return View(postDTO);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStoria(StoriaCreateDTO model, IFormFile? fileFoto, IFormFile? fileVideo)
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
                var response = await _storiaService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Storia created successfully";
                    return RedirectToAction("Index", "Home");
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
    }
}
