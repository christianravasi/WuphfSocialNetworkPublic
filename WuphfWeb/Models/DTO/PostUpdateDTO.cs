using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WuphfWeb.Models.DTO
{
    public class PostUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Testo { get; set; } = null!;
        public string? Immagine { get; set; }
        public string? Video { get; set; }
        [NotMapped]
        [ValidateNever]
        public string Username { get; set; }
        [NotMapped]
        [ValidateNever]
        public string? FotoProfilo { get; set; }
        [NotMapped]
        [ValidateNever]
        public DateTime DataCreazione { get; set; }
    }
}
