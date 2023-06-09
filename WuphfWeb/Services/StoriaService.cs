using WuphfUtility;
using WuphfWeb.Models;
using WuphfWeb.Models.DTO;
using WuphfWeb.Services.IServices;

namespace WuphfWeb.Services
{
    public class StoriaService : BaseService, IStoriaService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string wuphfUrl;

        public StoriaService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            wuphfUrl = configuration.GetValue<string>("ServiceUrls:WuphfAPI");
        }
        public Task<T> GetStorieOfFollowing<T>(string followerName, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = wuphfUrl + "/api/v1/StoriaAPI?followerName=" + followerName,
                Token = token
            });
        }
        public Task<T> CreateAsync<T>(StoriaCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = wuphfUrl + "/api/v1/StoriaAPI",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = wuphfUrl + "/api/v1/StoriaAPI/" + id,
                Token = token
            });
        }
        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = wuphfUrl + "/api/v1/StoriaAPI/" + id,
                Token = token
            });
        }
    }
}
