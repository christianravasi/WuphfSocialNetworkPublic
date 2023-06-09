using WuphfApi.Data;
using WuphfApi.Models;
using WuphfApi.Repository.IRepository;

namespace WuphfApi.Repository
{
    public class StoriaRepository : Repository<Storia>, IStoriaRepository
    {
        private readonly ApplicationDbContext _db;
        public StoriaRepository(ApplicationDbContext db) : base(db)
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
        public async Task<string> GetUsernameFromId(string userId)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => userId == u.Id);
            return user.UserName;
        }
        public async Task<string?> GetFotoProfiloFromId(string userId)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => userId == u.Id);
            return user.FotoProfilo;
        }
        public async Task<IEnumerable<Storia>?> GetStorieOfFollowing(string followerName)
        {
            var follower = _db.ApplicationUsers.FirstOrDefault(p => p.UserName == followerName);
            if (follower != null)
            {
                var seguiti = _db.Segue.Where(s => s.Follower == follower.Id);
                List<Storia> storie = new List<Storia>();
                foreach (var seguito in seguiti)
                {
                    storie.AddRange(_db.Storie.Where(p => p.FkUser == seguito.Following && p.DataCreazione > DateTime.Now.AddDays(-1)));
                }
                return storie;
            }
            return null;
        }
    }
}
