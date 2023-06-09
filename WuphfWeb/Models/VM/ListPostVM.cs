using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using WuphfWeb.Models.DTO;

namespace WuphfWeb.Models.VM
{
    public class ListPostVM
    {
        [ValidateNever]
        public IEnumerable<PostDTO>? Posts { get; set; }
        public int PostId { get; set; }
        [ValidateNever]
        public IEnumerable<StoriaDTO>? Storie { get; set; }
    }
}
