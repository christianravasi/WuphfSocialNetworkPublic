using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using System.Security.Claims;
using WuphfUtility;
using WuphfWeb.Models;
using WuphfWeb.Models.DTO;
using WuphfWeb.Models.VM;
using WuphfWeb.Services;
using WuphfWeb.Services.IServices;

namespace WuphfWeb.Controllers
{
    public class CommentoController : Controller
    {
        private readonly ICommentoService _commentoService;
        private readonly IMapper _mapper;
        public CommentoController(ICommentoService commentoService, IMapper mapper)
        {
            _commentoService = commentoService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCommento(PostVM model)
        {
            model.CommentoCreateDTO.FkUser = "ciao";
            model.CommentoCreateDTO.DataCreazione = DateTime.Now;
            var response = await _commentoService.CreateAsync<APIResponse>(model.CommentoCreateDTO, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Commento created successfully";
                return RedirectToAction("GetPost", "Post", new { id = model.CommentoCreateDTO.FkPost });
            }
            TempData["error"] = "Error encountered.";
            return RedirectToAction("GetPost", "Post", new { id = model.CommentoCreateDTO.FkPost });
        }
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteCommento(int? id)
        {
            if (id != null)
            {
                var response = await _commentoService.DeleteAsync<APIResponse>((int)id, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Commento deleted successfully";
                    return Json(new { success = true, message = "Eliminazione completata" });
                }
            }
            TempData["error"] = "Error encountered.";
            return Json(new { success = false, message = "C'è stato un errore..." });
        }
    }
}
