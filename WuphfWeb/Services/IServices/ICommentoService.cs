using WuphfWeb.Models.DTO;

namespace WuphfWeb.Services.IServices
{
    public interface ICommentoService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(CommentoCreateDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
