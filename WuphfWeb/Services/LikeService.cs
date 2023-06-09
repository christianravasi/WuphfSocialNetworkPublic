using NuGet.Common;
using WuphfUtility;
using WuphfWeb.Models;
using WuphfWeb.Models.DTO;
using WuphfWeb.Services.IServices;

namespace WuphfWeb.Services
{
    public class LikeService : BaseService, ILikeService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string wuphfUrl;

        public LikeService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            wuphfUrl = configuration.GetValue<string>("ServiceUrls:WuphfAPI");
        }
        public Task<T> GetLikesOfPost<T>(int idPost, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = wuphfUrl + "/api/v1/LikeAPI/Post/" + idPost,
                Token = token
            });
        }
        public Task<T> GetLikesOfComment<T>(int idCommento, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = wuphfUrl + "/api/v1/LikeAPI/Comment/" + idCommento,
                Token = token
            });
        }
        public Task<T> LikeToPost<T>(int idPost, string userId, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = new LikeCreateDTO() { FkPost = idPost, FkUser = userId },
                Url = wuphfUrl + "/api/v1/LikeAPI/Post",
                Token = token
            });
        }
        public Task<T> LikeToComment<T>(int idCommento, string userId, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = new LikeCreateDTO() { FkCommento = idCommento, FkUser = userId },
                Url = wuphfUrl + "/api/v1/LikeAPI/Comment",
                Token = token
            });
        }
    }
}
