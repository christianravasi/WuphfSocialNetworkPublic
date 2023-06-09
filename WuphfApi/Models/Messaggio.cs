using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace WuphfApi.Models
{
    public class Messaggio
    {
        public int Id { get; set; }
        public string Testo { get; set; } = null!;
        public string Sender { get; set; } = null!;
        public string Receiver { get; set; } = null!;
        public DateTime DataInvio { get; set; }
        public int IdChat { get; set; }
        [ForeignKey(nameof(Sender))]
        [ValidateNever]
        public ApplicationUser User1 { get; set; } = null!;
        [ForeignKey(nameof(Receiver))]
        [ValidateNever]
        public ApplicationUser User2 { get; set; } = null!;
        [ForeignKey(nameof(IdChat))]
        [ValidateNever]
        public Chat Chat { get; set; } = null!;
    }
}
