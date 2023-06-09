using WuphfApi.Data;
using WuphfApi.Models;
using WuphfApi.Repository.IRepository;

namespace WuphfApi.Repository
{
    public class ChatRepository : Repository<Chat>, IChatRepository
    {
        private readonly ApplicationDbContext _db;
        public ChatRepository(ApplicationDbContext db) : base(db)
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
        public async Task<Chat?> CreateOrDeleteChat(string username1, string username2)
        {
            var user1 = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == username1);
            var user2 = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == username2);
            if(user1 != null && user2 != null)
            {
                var chat = _db.Chats.FirstOrDefault(c => (c.FkUser1 == user1.Id && c.FkUser2 == user2.Id) || (c.FkUser1 == user2.Id && c.FkUser2 == user1.Id));
                if (chat != null)
                {
                    _db.Chats.Remove(chat);
                    await _db.SaveChangesAsync();
                    return new Chat();
                }
                else
                {
                    var newChat = new Chat()
                    {
                        FkUser1 = user1.Id,
                        FkUser2 = user2.Id
                    };
                    _db.Chats.Add(newChat);
                    await _db.SaveChangesAsync();
                    return newChat;
                }
            }
            return null;
        }
        public async Task<Chat?> GetChat(int idChat)
        {
            var chat = _db.Chats.FirstOrDefault(c => c.Id == idChat);
            if (chat != null)
            {
                //chat.Messaggi = await GetMessaggiOfChat(chat.Id);
                chat.Username1 = _db.ApplicationUsers.FirstOrDefault(u => u.Id == chat.FkUser1).UserName;
                chat.Username2 = _db.ApplicationUsers.FirstOrDefault(u => u.Id == chat.FkUser2).UserName;
            }
            return chat;
        }
        public async Task<IEnumerable<Chat>?> GetChatsOfUser(string name)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == name);
            if (user != null)
            {
                var chats = new List<Chat>();
                chats.AddRange(_db.Chats.Where(c => c.FkUser1 == user.Id || c.FkUser2 == user.Id).ToList());
                foreach (var chat in chats)
                {
                    chat.Username1 = _db.ApplicationUsers.FirstOrDefault(u => u.Id == chat.FkUser1).UserName;
                    chat.Username2 = _db.ApplicationUsers.FirstOrDefault(u => u.Id == chat.FkUser2).UserName;
                }
                //foreach (var item in chats)
                //{
                //    item.Messaggi = await GetMessaggiOfChat(item.Id);
                //}
                return chats;
            }
            return null;
        }
        public async Task<IEnumerable<Messaggio>?> GetMessaggiOfChat(int idChat)
        {
            var messaggi = _db.Messaggi.Where(m => m.IdChat == idChat);
            return messaggi;
        }
    }
}
