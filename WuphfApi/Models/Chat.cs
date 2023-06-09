using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace WuphfApi.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string FkUser1 { get; set; } = null!;
        public string FkUser2 { get; set; } = null!;
        [ForeignKey(nameof(FkUser1))]
        [ValidateNever]
        public ApplicationUser User1 { get; set; } = null!;
        [ForeignKey(nameof(FkUser2))]
        [ValidateNever]
        public ApplicationUser User2 { get; set; } = null!;
        [ValidateNever]
        public IEnumerable<Messaggio>? Messaggi { get; set; }
        [NotMapped]
        [ValidateNever]
        public string? Username1 { get; set; }
        [NotMapped]
        [ValidateNever]
        public string? Username2 { get; set; }
    }
}
