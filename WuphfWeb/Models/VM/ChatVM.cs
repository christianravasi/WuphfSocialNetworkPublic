using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using WuphfWeb.Models.DTO;

namespace WuphfWeb.Models.VM
{
    public class ChatVM
    {
        [ValidateNever]
        public ChatDTO Chat { get; set; }
        [ValidateNever]
        public string Token { get; set; }
    }
}
