using WuphfApi.Models;

namespace WuphfApi.Repository.IRepository
{
    public interface IStoriaRepository : IRepository<Storia>
    {
        Task<ApplicationUser?> GetApplicationUserFromName(string name);
        Task<IEnumerable<Storia>?> GetStorieOfFollowing(string followerName);
        Task<string> GetUsernameFromId(string userId);
        Task<string?> GetFotoProfiloFromId(string userId);
    }
}
