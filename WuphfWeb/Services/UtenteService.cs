using NuGet.Common;
using System.Security.Policy;
using WuphfUtility;
using WuphfWeb.Models;
using WuphfWeb.Models.DTO;
using WuphfWeb.Services.IServices;
using static WuphfUtility.SD;

namespace WuphfWeb.Services
{
    public class UtenteService : BaseService, IUtenteService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string wuphfUrl;
        public UtenteService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            wuphfUrl = configuration.GetValue<string>("ServiceUrls:WuphfAPI");
        }

        public Task<T> UpdateProfilePicture<T>(ProfilePictureUpdateDTO obj, string token)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.PUT,
                Data = obj,
                Url = wuphfUrl + "/api/v1/Profile/UpdateProfilePicture",
                Token = token
            });
        }
        public Task<T> GetUser<T>(string username, string token)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.GET,
                Url = wuphfUrl + "/api/v1/Profile?username=" + username,
                Token = token
            });
        }
        public Task<T> FindSimilarUsernames<T>(string username, string token)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.GET,
                Url = wuphfUrl + "/api/v1/Profile/SimilarUsernames?username=" + username,
                Token = token
            });
        }
        public Task<T> TelegramChatId<T>(UserDTO user, string token)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.POST,
                Data = user,
                Url = wuphfUrl + "/api/v1/Profile/SaveChatId",
                Token = token
            });
        }
        public Task<T> Follow<T>(string userName, string followerName, string token)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.PUT,
                Data = new SegueDTO() { userName = userName, followerName = followerName },
                Url = wuphfUrl + "/api/v1/Profile/UserFollowers",
                Token = token
            });
        }
        public Task<T> GetPostsOfUser<T>(string userName, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = wuphfUrl + "/api/v1/PostAPI/Utente?userName=" + userName,
                Token = token
            });
        }
    }
}
