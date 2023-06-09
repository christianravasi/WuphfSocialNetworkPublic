using WuphfApi.Data;
using WuphfApi.Models;
using WuphfApi.Repository.IRepository;

namespace WuphfApi.Repository
{
    public class CommentoRepository : Repository<Commento>, ICommentoRepository
    {
        private readonly ApplicationDbContext _db;
        public CommentoRepository(ApplicationDbContext db) : base(db)
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
    }
}
