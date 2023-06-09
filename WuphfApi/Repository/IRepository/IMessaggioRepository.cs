using WuphfApi.Models;

namespace WuphfApi.Repository.IRepository
{
    public interface IMessaggioRepository : IRepository<Messaggio>
    {
        Task<ApplicationUser?> GetApplicationUserFromName(string name);
        Task<IEnumerable<Messaggio>?> GetMessaggiOfChat(int idChat);
        Task<Messaggio?> CreateMessaggio(string sender, string receiver, int idChat, string testo);
    }
}
