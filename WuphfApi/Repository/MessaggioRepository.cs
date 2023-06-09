using WuphfApi.Data;
using WuphfApi.Models;
using WuphfApi.Repository.IRepository;

namespace WuphfApi.Repository
{
    public class MessaggioRepository : Repository<Messaggio>, IMessaggioRepository
    {
        private readonly ApplicationDbContext _db;
        public MessaggioRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ApplicationUser?> GetApplicationUserFromName(string name)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == name);
            if (user != null)
            {
                return user;
            }
            return null;
        }
        public async Task<IEnumerable<Messaggio>?> GetMessaggiOfChat(int idChat)
        {
            var messaggi = _db.Messaggi.Where(m => m.IdChat == idChat).ToList();
            return messaggi;
        }
        public async Task<Messaggio?> CreateMessaggio(string sender, string receiver, int idChat, string testo)
        {
            var senderUser = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == sender);
            var receiverUser = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == receiver);
            var chat = _db.Chats.FirstOrDefault(c => c.Id == idChat);
            if (senderUser != null && receiverUser != null && chat != null)
            {
                Messaggio messaggio = new Messaggio()
                {
                    Testo = testo,
                    Sender = senderUser.Id,
                    Receiver = receiverUser.Id,
                    IdChat = chat.Id,
                    DataInvio = DateTime.Now
                };
                await _db.Messaggi.AddAsync(messaggio);
                await _db.SaveChangesAsync();
                return messaggio;
            }
            return null;
        }
    }
}
