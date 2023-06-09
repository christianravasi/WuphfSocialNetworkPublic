using WuphfApi.Models;

namespace WuphfApi.Repository.IRepository
{
    public interface IChatRepository : IRepository<Chat>
    {
        Task<ApplicationUser?> GetApplicationUserFromName(string name);
        Task<Chat?> GetChat(int idChat);
        Task<IEnumerable<Chat>?> GetChatsOfUser(string name);
        Task<Chat?> CreateOrDeleteChat(string sender, string receiver);
    }
}
