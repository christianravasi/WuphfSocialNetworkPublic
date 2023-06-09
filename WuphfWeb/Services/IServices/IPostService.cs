using WuphfWeb.Models.DTO;

namespace WuphfWeb.Services.IServices
{
    public interface IPostService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(PostCreateDTO dto, string token);
        Task<T> UpdateAsync<T>(PostUpdateDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
        Task<T> GetCommentsFromPost<T>(int idPost, string token);
        Task<T> GetPostsOfUser<T>(string userName, string token);
        Task<T> GetPostsOfFollowing<T>(string followerName, string token);
    }
}
