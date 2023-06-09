using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WuphfApi.Data;
using WuphfApi.Models;
using WuphfApi.Models.DTO;
using WuphfApi.Repository.IRepository;

namespace WuphfApi.Repository
{
    public class UtenteRepository : IUtenteRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public UtenteRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<ApplicationUser> FindUser(string userId)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == userId);
            if (user != null)
            {
                return user;
            }
            return new ApplicationUser();
        }
        public async Task<bool> IsSeguito(string userName, string followerName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == userName);
            var follower = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == followerName);
            var segue = _db.Segue.Where(s => s.Follower == follower.Id && s.Following == user.Id).FirstOrDefault();
            if (segue == null)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> IsChat(string user1, string user2)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == user1);
            var follower = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == user2);
            var chat = _db.Chats.Where(s => (s.FkUser1 == follower.Id && s.FkUser2 == user.Id) || (s.FkUser2 == follower.Id && s.FkUser1 == user.Id)).FirstOrDefault();
            if (chat == null)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> UpdateProfilePicture(ProfilePictureUpdateDTO userDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == userDto.UserId);
            if (user != null)
            {
                user.FotoProfilo = userDto.FotoProfilo;
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<ApplicationUser> UserFollowers(string userName, string followerName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == userName);
            var follower = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == followerName);
            if (user != null && follower != null)
            {
                var segues = _db.Segue.Where(u => u.Following == user.Id).ToList();
                foreach (var segue in segues)
                {
                    var seguente = _db.ApplicationUsers.FirstOrDefault(u => u.Id == segue.Follower);
                    if (seguente != null)
                    {
                        user.Followers.Add(seguente);
                    }
                }
                if (!user.Followers.Contains(follower))
                {
                    await _db.Segue.AddAsync(new Segue()
                    {
                        Follower = follower.Id,
                        Following = user.Id
                    });
                    await _db.SaveChangesAsync();
                    return follower;
                }
                else
                {
                    var coppia = _db.Segue.Where(u => u.Follower == follower.Id && u.Following == user.Id).FirstOrDefault();
                    if (coppia != null)
                    {
                        _db.Segue.Remove(coppia);
                        await _db.SaveChangesAsync();
                        return follower;
                    }
                }
            }
            return new ApplicationUser();
        }
        public async Task<IEnumerable<ApplicationUser>?> FindSimilarUsers(string username)
        {
            var usersDb = _db.ApplicationUsers.ToList();
            List<ApplicationUser> users = new List<ApplicationUser>();
            foreach (var user in usersDb)
            {
                if (4 > DamerauLevenshtein.DamerauLevenshteinDistance(user.UserName, username))
                {
                    users.Add(user);
                }
            }
            return users;
        }
        public async Task<int> NumeroFollower(string username)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == username);
            if (user != null)
            {
                return _db.Segue.Where(s => s.Following == user.Id).ToList().Count();
            }
            return 0;
        }
		public async Task<int> NumeroFollowing(string username)
		{
			var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == username);
			if (user != null)
			{
				return _db.Segue.Where(s => s.Follower == user.Id).ToList().Count();
			}
			return 0;
		}
        public async Task<bool> CreateTelegramChatId(string username, int chatId)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == username);
            if (user != null)
            {
                user.TelegramChatId = chatId;
                _db.ApplicationUsers.Update(user);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
