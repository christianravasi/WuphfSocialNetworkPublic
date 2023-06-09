using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace WuphfApi.Models.DTO
{
    public class LikeDTO
    {
        public int Id { get; set; }
        public int? FkPost { get; set; }
        public int? FkCommento { get; set; }
        public string FkUser { get; set; } = null!;
        [NotMapped]
        [ValidateNever]
        public bool IsUsers { get; set; }
    }
}
