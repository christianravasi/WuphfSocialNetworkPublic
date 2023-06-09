using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace WuphfWeb.Models.DTO
{
    public class ChatDTO
    {
        public int Id { get; set; }
        public string FkUser1 { get; set; } = null!;
        public string FkUser2 { get; set; } = null!;
        [NotMapped]
        [ValidateNever]
        public IEnumerable<MessaggioDTO>? Messaggi { get; set; }
        [NotMapped]
        [ValidateNever]
        public string Username1 { get; set; } = null!;
        [NotMapped]
        [ValidateNever]
        public string Username2 { get; set; } = null!;
        [NotMapped]
        [ValidateNever]
        public bool? IsUser1 { get; set; }
    }
}
