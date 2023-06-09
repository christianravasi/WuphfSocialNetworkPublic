using WuphfUtility;
using WuphfWeb.Models;
using WuphfWeb.Models.DTO;

namespace WuphfWeb.Services.IServices
{
    public interface IUtenteService
    {
        Task<T> UpdateProfilePicture<T>(ProfilePictureUpdateDTO profilePictureUpdateDTO, string token);
        Task<T> GetUser<T>(string username, string token);
        Task<T> Follow<T>(string username, string followerName, string token);
        Task<T> GetPostsOfUser<T>(string userName, string token);
        Task<T> FindSimilarUsernames<T>(string username, string token);
        Task<T> TelegramChatId<T>(UserDTO user, string token);
    }
}
