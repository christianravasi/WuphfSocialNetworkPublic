using Microsoft.Extensions.Hosting;
using WuphfApi.Data;
using WuphfApi.Migrations;
using WuphfApi.Models;
using WuphfApi.Repository.IRepository;

namespace WuphfApi.Repository
{
    public class LikeRepository : Repository<Like>, ILikeRepository
    {
        private readonly ApplicationDbContext _db;
        public LikeRepository(ApplicationDbContext db) : base(db)
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
        public async Task<IEnumerable<Like>?> GetLikesOfPost(int idPost)
        {
            return _db.Likes.Where(l => l.FkPost == idPost);           
        }
        public async Task<IEnumerable<Like>?> GetLikesOfComment(int idCommento)
        {
            return _db.Likes.Where(l => l.FkCommento == idCommento);
        }
        public async Task<bool> LikeToPost(int idPost, string userId)
        {
            var post = _db.Posts.FirstOrDefault(p => p.Id == idPost);
            if (post != null)
            {
                var likeDb = _db.Likes.FirstOrDefault(l => l.FkUser == userId && l.FkPost == idPost);
                if (likeDb == null)
                {
                    var like = new Like()
                    {
                        FkUser = userId,
                        FkPost = post.Id
                    };
                    await _db.Likes.AddAsync(like);
                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    _db.Likes.Remove(likeDb);
                    await _db.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> LikeToComment(int idCommento, string userId)
        {
            var commento = _db.Commenti.FirstOrDefault(p => p.Id == idCommento);
            if (commento != null)
            {
                var likeDb = _db.Likes.FirstOrDefault(l => l.FkUser == userId && l.FkCommento == idCommento);
                if (likeDb == null)
                {
                    var like = new Like()
                    {
                        FkUser = userId,
                        FkCommento = commento.Id
                    };
                    await _db.Likes.AddAsync(like);
                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    _db.Likes.Remove(likeDb);
                    await _db.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> RemoveLikesFromPost(int idPost)
        {
            try
            {
                var likes = _db.Likes.Where(p => p.FkPost == idPost);
                foreach (var item in likes)
                {
                    _db.Likes.Remove(item);
                }
                var comments = _db.Commenti.Where(c => c.FkPost == idPost);
                foreach (var commento in comments)
                {
                    var likeCommenti = _db.Likes.Where(p => p.FkCommento == commento.Id);
                    foreach (var item in likeCommenti)
                    {
                        _db.Likes.Remove(item);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> RemoveLikesFromComment(int idCommento)
        {
            try
            {
                var likes = _db.Likes.Where(p => p.FkCommento == idCommento);
                foreach (var item in likes)
                {
                    _db.Likes.Remove(item);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
