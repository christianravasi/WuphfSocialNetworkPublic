using WuphfUtility;
using WuphfWeb.Models;
using WuphfWeb.Models.DTO;
using WuphfWeb.Services.IServices;

namespace WuphfWeb.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string wuphfUrl;

        public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            wuphfUrl = configuration.GetValue<string>("ServiceUrls:WuphfAPI");
        }

        public Task<T> LoginAsync<T>(LoginRequestDTO obj)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = wuphfUrl + "/api/v1/UsersAuth/login"
            });
        }

        public Task<T> RegisterAsync<T>(RegisterationRequestDTO obj)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = wuphfUrl + "/api/v1/UsersAuth/register"
            });
        }

        public Task<T> ConfirmEmail<T>(string userId, string code)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = wuphfUrl + "/api/v1/UsersAuth/register?userId=" + userId + "&code=" + code
            });
        }

        public Task<T> ChangePassword<T>(ChangePasswordModel obj)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = wuphfUrl + "/api/v1/UsersAuth/change-password"
            });
        }

        public Task<T> ResetPasswordToken<T>(ResetPasswordTokenModel obj)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = wuphfUrl + "/api/v1/UsersAuth/reset-password-token"
            });
        }
        public Task<T> ResetPassword<T>(ResetPasswordModel obj)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = wuphfUrl + "/api/v1/UsersAuth/reset-password"
            });
        }
    }
}
