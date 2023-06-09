using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WuphfApi.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Testo { get; set; } = null!;
        public string? Immagine { get; set; }
        public string? Video { get; set; }
        public DateTime DataCreazione { get; set; }
        public DateTime? DataUpdate { get; set; }
        public string? FkUser { get; set; }
        [ForeignKey(nameof(FkUser))]
        [ValidateNever]
        public ApplicationUser? User { get; set; }
        [ValidateNever]
        public IEnumerable<Like>? Likes { get; set; }
    }
}
