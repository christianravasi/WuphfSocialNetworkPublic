using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace WuphfApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = null!;
        public string TelegramToken { get; set; } = null!;
        public int? TelegramChatId { get; set; }
        public string? FotoProfilo { get; set; }
        [ValidateNever]
        public IEnumerable<Post>? Posts { get; set; }
        [ValidateNever]
        public IEnumerable<Commento>? Commenti { get; set; }
        [ValidateNever]
        public IEnumerable<Like>? Likes { get; set; }
        [ValidateNever]
        [NotMapped]
        public IEnumerable<Chat>? Chats { get; set; }
        [ValidateNever]
        [NotMapped]
        public IEnumerable<Messaggio>? Messaggi { get; set; }
        [NotMapped]
        [ValidateNever]
        public List<ApplicationUser> Followers { get; set; } = new List<ApplicationUser>();
        [NotMapped]
        [ValidateNever]
        public List<ApplicationUser> Following { get; set; } = new List<ApplicationUser>();
    }
}
