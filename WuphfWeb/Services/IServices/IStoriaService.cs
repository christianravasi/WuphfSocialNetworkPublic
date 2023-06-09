using WuphfWeb.Models.DTO;

namespace WuphfWeb.Services.IServices
{
    public interface IStoriaService
    {
        Task<T> GetStorieOfFollowing<T>(string followerName, string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(StoriaCreateDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
