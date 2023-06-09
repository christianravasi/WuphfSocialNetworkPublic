using WuphfApi.Models;
using WuphfApi.Models.DTO;

namespace WuphfApi.Repository.IRepository
{
    public interface IUtenteRepository
    {
        Task<bool> UpdateProfilePicture(ProfilePictureUpdateDTO userDto);
        Task<ApplicationUser> FindUser(string userId);
        Task<ApplicationUser> UserFollowers(string userName, string followerName);
        Task<bool> IsSeguito(string userName, string followerName);
        Task<bool> IsChat(string user1, string user2);
        Task<IEnumerable<ApplicationUser>?> FindSimilarUsers(string username);
        Task<int> NumeroFollower(string username);
        Task<int> NumeroFollowing(string username);
        Task<bool> CreateTelegramChatId(string username, int chaatId);
    }
}
