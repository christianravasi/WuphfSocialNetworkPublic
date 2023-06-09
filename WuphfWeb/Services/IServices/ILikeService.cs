namespace WuphfWeb.Services.IServices
{
    public interface ILikeService
    {
        Task<T> GetLikesOfPost<T>(int idPost, string token);
        Task<T> GetLikesOfComment<T>(int idCommento, string token);
        Task<T> LikeToPost<T>(int idPost, string userId, string token);
        Task<T> LikeToComment<T>(int idCommento, string userId, string token);
    }
}
