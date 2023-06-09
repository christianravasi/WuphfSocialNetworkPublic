using WuphfUtility;
using WuphfWeb.Models;
using WuphfWeb.Models.DTO;
using WuphfWeb.Services.IServices;

namespace WuphfWeb.Services
{
    public class ChatService : BaseService, IChatService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string commentoUrl;

        public ChatService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            commentoUrl = configuration.GetValue<string>("ServiceUrls:WuphfAPI");
        }

        public Task<T> GetChatsOfUserAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = commentoUrl + "/api/v1/ChatAPI",
                Token = token
            });
        }
        public Task<T> GetChatAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = commentoUrl + "/api/v1/ChatAPI/" + id,
                Token = token
            });
        }
        public Task<T> CreateOrDeleteChatAsync<T>(ChatCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = commentoUrl + "/api/v1/ChatAPI",
                Token = token
            });
        }
        public Task<T> CreateMessageAsync<T>(MessaggioCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = commentoUrl + "/api/v1/MessaggioAPI",
                Token = token
            });
        }
        public Task<T> DeleteMessageAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = commentoUrl + "/api/v1/MessaggioAPI/" + id,
                Token = token
            });
        }
    }
}
