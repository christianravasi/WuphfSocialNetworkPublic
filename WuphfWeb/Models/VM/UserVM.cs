using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using WuphfWeb.Models.DTO;

namespace WuphfWeb.Models.VM
{
    public class UserVM
    {
        [ValidateNever]
        public UserDTO UserDTO { get; set; }
        [ValidateNever]
        public SegueDTO SegueDTO { get; set; }
        [ValidateNever]
        public IEnumerable<PostDTO> Posts { get; set; }
        [ValidateNever]
        public ChatCreateDTO? ChatCreateDTO { get; set; }
    }
}
