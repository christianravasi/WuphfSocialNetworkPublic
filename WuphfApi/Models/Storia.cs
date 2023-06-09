using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace WuphfApi.Models
{
    public class Storia
    {
        public int Id { get; set; }
        public string Testo { get; set; } = null!;
        public string? Immagine { get; set; }
        public string? Video { get; set; }
        public DateTime DataCreazione { get; set; }
        public string FkUser { get; set; } = null!;
        [ForeignKey(nameof(FkUser))]
        [ValidateNever]
        public ApplicationUser User { get; set; }
    }
}
