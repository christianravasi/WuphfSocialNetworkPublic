using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace WuphfWeb.Models.DTO
{
    public class MessaggioDTO
    {
        public int Id { get; set; }
        public string Testo { get; set; } = null!;
        public string Sender { get; set; } = null!;
        public string Receiver { get; set; } = null!;
        public int IdChat { get; set; }
        public DateTime DataInvio { get; set; }
    }
}
