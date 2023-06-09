using WuphfApi.Data;
using WuphfApi.Models;
using WuphfApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace WuphfApi.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly ApplicationDbContext _db;
        public PostRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        public async Task<Post> UpdateAsync(Post entity)
        {
            entity.DataUpdate = DateTime.Now;
            _db.Posts.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
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
        public async Task<IEnumerable<Commento>?> GetCommentiFromPost(int idPost)
        {
            var post = _db.Posts.FirstOrDefault(p => p.Id == idPost);
            if (post != null)
            {
                var commenti = _db.Commenti.Where(c => c.FkPost == post.Id);
                return commenti;
            }
            return null;
        }
        public async Task<IEnumerable<Post>?> GetPostsOfUser(string userName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(p => p.UserName == userName);
            if (user != null)
            {
                var posts = _db.Posts.Where(c => c.FkUser == user.Id);
                return posts;
            }
            return null;
        }
        public async Task<IEnumerable<Post>?> GetPostsOfFollowing(string followerName)
        {
            var follower = _db.ApplicationUsers.FirstOrDefault(p => p.UserName == followerName);
            if (follower != null)
            {
                var seguiti = _db.Segue.Where(s => s.Follower == follower.Id);
                List<Post> posts = new List<Post>();
                foreach (var seguito in seguiti)
                {
                    //posts.AddRange(_db.Posts.Where(p => p.FkUser == seguito.Following));
                    //var postsSeguiti = _db.Posts.Where(p => p.FkUser == seguito.Following).OrderBy(p => p.DataCreazione).Take(5);
                    var postsSeguiti = _db.Posts.Where(p => p.FkUser == seguito.Following).OrderBy(p => p.DataCreazione);
                    foreach (var post in postsSeguiti)
                    {
                        if (_db.Visualizzato.FirstOrDefault(v => v.FkPost == post.Id && v.FkUser == follower.Id) == null)
                        {
                            posts.Add(post);
                            if (posts.Count() >= 4)
                            {
                                break;
                            }
                        }
                    }
                }
                return posts;
            }
            return null;
        }
        public async Task<bool> UpdatePostsVisualizzatiDaUtente(IEnumerable<Post>? posts, string username)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == username);
            if (user != null && posts != null)
            {
                foreach (var post in posts)
                {
                    await _db.Visualizzato.AddAsync(new Visualizzato()
                    {
                        FkPost = post.Id, 
                        FkUser = user.Id
                    });
                }
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdatePostVisualizzatoDaUtente(int postId, string username)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == username);
            var post = _db.Posts.FirstOrDefault(u => u.Id == postId);
            if (user != null && post != null)
            {
                await _db.Visualizzato.AddAsync(new Visualizzato()
                {
                    FkPost = post.Id,
                    FkUser = user.Id
                });
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
