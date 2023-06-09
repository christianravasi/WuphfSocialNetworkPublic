using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WuphfApi.Models.DTO
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
        public DateTime DataCreazione { get; set; }
    }
}
