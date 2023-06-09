using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using WuphfWeb.Models.DTO;

namespace WuphfWeb.Models.VM
{
    public class ProfileVM
    {
        [ValidateNever]
        public UserDTO User { get; set; }
        public ProfilePictureUpdateDTO ProfilePictureUpdateDTO { get; set; }
    }
}
