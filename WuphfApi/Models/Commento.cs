using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace WuphfApi.Models
{
    public class Commento
    {
        public int Id { get; set; }
        public string Testo { get; set; } = null!;
        public DateTime DataCreazione { get; set; }
        public string FkUser { get; set; }
        [ForeignKey(nameof(FkUser))]
        [ValidateNever]
        public ApplicationUser User { get; set; }
        public int FkPost { get; set; }
        [ForeignKey(nameof(FkPost))]
        [ValidateNever]
        public Post Post { get; set; }
        [ValidateNever]
        public IEnumerable<Like>? Likes { get; set; }
    }
}
