using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace WuphfApi.Models
{
    public class Visualizzato
    {
        public int Id { get; set; }
        public int FkPost { get; set; }
        public string FkUser { get; set; } = null!;
        [ForeignKey(nameof(FkPost))]
        [ValidateNever]
        public Post Post { get; set; }
        [ForeignKey(nameof(FkUser))]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
