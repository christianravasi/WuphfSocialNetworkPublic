using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using WuphfWeb.Models.DTO;

namespace WuphfWeb.Models.VM
{
    public class PostVM
    {
        [ValidateNever]
        public PostDTO Post { get; set; }
        [ValidateNever]
        public IEnumerable<CommentoDTO>? Commenti { get; set; }
        public CommentoCreateDTO? CommentoCreateDTO { get; set; }
    }
}
