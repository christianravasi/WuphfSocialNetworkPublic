using WuphfApi.Models;

namespace WuphfApi.Repository.IRepository
{
    public interface ILikeRepository : IRepository<Like>
    {
        Task<ApplicationUser?> GetApplicationUserFromName(string name);
        Task<IEnumerable<Like>?> GetLikesOfPost(int idPost);
        Task<IEnumerable<Like>?> GetLikesOfComment(int idCommento);
        Task<bool> LikeToPost(int idPost, string userId);
        Task<bool> LikeToComment(int idCommento, string userId);
        Task<bool> RemoveLikesFromPost(int idPost);
        Task<bool> RemoveLikesFromComment(int idComment);
    }
}
