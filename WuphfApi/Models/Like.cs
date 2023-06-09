using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace WuphfApi.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int? FkPost { get; set; }
        [ForeignKey(nameof(FkPost))]
        [ValidateNever]
        public Post? Post { get; set; }
        public int? FkCommento { get; set; }
        [ForeignKey(nameof(FkCommento))]
        [ValidateNever]
        public Commento? Commento { get; set; }
        public string FkUser { get; set; } = null!;
        [ForeignKey(nameof(FkUser))]
        [ValidateNever]
        public ApplicationUser User { get; set; }
    }
}
