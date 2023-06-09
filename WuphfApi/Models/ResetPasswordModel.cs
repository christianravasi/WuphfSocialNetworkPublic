using System.ComponentModel.DataAnnotations;

namespace WuphfApi.Models
{
    public class ResetPasswordModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmNewPassword { get; set; }
        public string Token { get; set; }
    }
}
