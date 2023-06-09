using WuphfApi.Models;
using System.Linq.Expressions;

namespace WuphfApi.Repository.IRepository
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<Post> UpdateAsync(Post entity);
        Task<ApplicationUser?> GetApplicationUserFromName(string name);
        Task<string> GetUsernameFromId(string userId);
        Task<string?> GetFotoProfiloFromId(string userId);
        Task<IEnumerable<Commento>?> GetCommentiFromPost(int idPost);
        Task<IEnumerable<Post>?> GetPostsOfUser(string userName);
        Task<IEnumerable<Post>?> GetPostsOfFollowing(string followerName);
        Task<bool> UpdatePostsVisualizzatiDaUtente(IEnumerable<Post>? posts, string username);
        Task<bool> UpdatePostVisualizzatoDaUtente(int postId, string username);
    }
}
