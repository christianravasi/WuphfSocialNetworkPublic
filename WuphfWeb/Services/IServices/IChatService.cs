using WuphfWeb.Models.DTO;

namespace WuphfWeb.Services.IServices
{
    public interface IChatService
    {
        Task<T> GetChatsOfUserAsync<T>(string token);
        Task<T> GetChatAsync<T>(int id, string token);
        Task<T> CreateOrDeleteChatAsync<T>(ChatCreateDTO dto, string token);
        Task<T> CreateMessageAsync<T>(MessaggioCreateDTO dto, string token);
        Task<T> DeleteMessageAsync<T>(int id, string token);
    }
}
