using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace WuphfWeb.Models.DTO
{
    public class UserDTO
    {
        public string ID { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string TelegramToken { get; set; } = null!;
        public int TelegramChatId { get; set; }
        public string? FotoProfilo { get; set; }
        [ValidateNever]
        [NotMapped]
        public bool IsSeguito { get; set; }
        [NotMapped]
        [ValidateNever]
        public bool IsChat { get; set; }
		[NotMapped]
		[ValidateNever]
        public int NumeroPost { get; set; }
		[NotMapped]
		[ValidateNever]
        public int NumeroFollower { get; set; }
		[NotMapped]
		[ValidateNever]
        public int NumeroFollowing { get; set; }
	}
}
