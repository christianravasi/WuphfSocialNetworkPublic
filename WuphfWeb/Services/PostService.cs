using WuphfUtility;
using WuphfWeb.Models;
using WuphfWeb.Models.DTO;
using WuphfWeb.Services;
using WuphfWeb.Services.IServices;

namespace WuphfWeb.Services
{
    public class PostService : BaseService, IPostService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string wuphfUrl;

        public PostService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            wuphfUrl = configuration.GetValue<string>("ServiceUrls:WuphfAPI");
        }

        public Task<T> CreateAsync<T>(PostCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = wuphfUrl + "/api/v1/PostAPI",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = wuphfUrl + "/api/v1/PostAPI/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = wuphfUrl + "/api/v1/PostAPI",
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

        public Task<T> GetPostsOfFollowing<T>(string followerName, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = wuphfUrl + "/api/v1/PostAPI/PostSeguiti?followerName=" + followerName,
                Token = token
            });
        }

        public Task<T> GetCommentsFromPost<T>(int idPost, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = wuphfUrl + "/api/v1/PostAPI/Commenti/" + idPost,
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = wuphfUrl + "/api/v1/PostAPI/" + id,
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(PostUpdateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = wuphfUrl + "/api/v1/PostAPI/" + dto.Id,
                Token = token
            }) ;
        }
    }
}
