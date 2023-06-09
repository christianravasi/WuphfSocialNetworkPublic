using WuphfUtility;
using WuphfWeb.Models;
using WuphfWeb.Models.DTO;
using WuphfWeb.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;

namespace WuphfWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IEmailSender _emailSender;
        public AuthController(IAuthService authService, IEmailSender emailSender)
        {
            _authService = authService;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO obj = new();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO obj)
        {
            APIResponse response = await _authService.LoginAsync<APIResponse>(obj);
            if (response != null && response.IsSuccess)
            { 
                LoginResponseDTO model = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(model.Token);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == "unique_name").Value));
                identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


                HttpContext.Session.SetString(SD.SessionToken, model.Token);
                return RedirectToAction("Index","Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", response.ErrorMessages.FirstOrDefault());
                return View(obj);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegisterationRequestDTO obj = new();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmEmail(RegisterationRequestDTO obj)
        {
            APIResponse response =  await _authService.RegisterAsync<APIResponse>(obj);
            if (response != null && response.IsSuccess)
            {
                ResponseObj confirmEmailObj = JsonConvert.DeserializeObject<ResponseObj>(Convert.ToString(response.Result));
                if (confirmEmailObj != null)
                {
                    var callbackUrl = Url.Page(
                        "/Auth/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = confirmEmailObj.userId, code = confirmEmailObj.code, returnUrl = confirmEmailObj.returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(confirmEmailObj.email, "Conferma la tua email",
                        $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Clicca qui</a> per confermare il tuo account.");
                }
                return RedirectToAction(nameof(EmailInviata));
            }
            TempData["error"] = "Errore";
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> EmailInviata()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code, string returnUrl, string page)
        {
            var response = await _authService.ConfirmEmail<APIResponse>(userId, code);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(Login));
            }
            return RedirectToAction(nameof(Register));
        }

        [Authorize]
        public async Task<IActionResult> ChangePassword()
        {
            var model = new ChangePasswordModel();
            model.Username = User.Identity.Name;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel obj)
        {
            obj.Username = User.Identity.Name;
            var response = await _authService.ChangePassword<APIResponse>(obj);
            if (response != null && response.IsSuccess)
            { 
                TempData["success"] = "Password cambiata correttamente";
                return RedirectToAction("Index", "Home");
            }
            TempData["error"] = "Errore";
            return RedirectToAction(nameof(ChangePassword));
        }
        public async Task<IActionResult> ForgotPassword()
        {
            return View(new ResetPasswordTokenModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordTokenModel model)
        {
            var response = await _authService.ResetPasswordToken<APIResponse>(model);
            if (response != null && response.IsSuccess)
            {
                ResponseObj obj = JsonConvert.DeserializeObject<ResponseObj>(Convert.ToString(response.Result));
                if (obj != null)
                {
                    var callbackUrl = Url.Page(
                        "/Auth/ResetPassword",
                        pageHandler: null,
                        values: new { userId = obj.userId, code = obj.code, returnUrl = obj.returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(obj.email, "Reset Password",
                    $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Clicca qui</a> per resettare la tua password. " +
                    $"Se non hai chiesto tu di cambiare la password del tuo account, ignora questa email.");
                }
                TempData["success"] = "Controlla la tua mail!";
                return RedirectToAction("Index", "Home");
            }
            TempData["error"] = "Errore";
            return RedirectToAction(nameof(ChangePassword));
        }

        public async Task<IActionResult> ResetPassword(string userId, string code, string returnUrl, string page)
        {
            var model = new ResetPasswordModel();
            model.Username = userId;
            model.Token = code;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPasswordFinal(ResetPasswordModel obj)
        {
            var response = await _authService.ResetPassword<APIResponse>(obj);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Password cambiata correttamente";
                return RedirectToAction(nameof(Login));
            }
            TempData["error"] = "Errore";
            return RedirectToAction(nameof(Login));
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(SD.SessionToken, "");
            return RedirectToAction("Index","Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
    public class ResponseObj
    {
        public string userId { get; set; }
        public string code { get; set; }
        public string returnUrl { get; set; }
        public string email { get; set; }
    }
}
