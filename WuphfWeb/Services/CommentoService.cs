using WuphfUtility;
using WuphfWeb.Models;
using WuphfWeb.Models.DTO;
using WuphfWeb.Services.IServices;

namespace WuphfWeb.Services
{
    public class CommentoService : BaseService, ICommentoService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string commentoUrl;

        public CommentoService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            commentoUrl = configuration.GetValue<string>("ServiceUrls:WuphfAPI");
        }

        public Task<T> CreateAsync<T>(CommentoCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = commentoUrl + "/api/v1/CommentoAPI",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = commentoUrl + "/api/v1/CommentoAPI/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = commentoUrl + "/api/v1/CommentoAPI",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = commentoUrl + "/api/v1/CommentoAPI/" + id,
                Token = token
            });
        }
    }
}
