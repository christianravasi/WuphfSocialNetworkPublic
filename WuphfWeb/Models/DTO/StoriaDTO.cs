using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace WuphfWeb.Models.DTO
{
    public class StoriaDTO
    {
        public int Id { get; set; }
        public string Testo { get; set; } = null!;
        public string? Immagine { get; set; }
        public string? Video { get; set; }
        public string FkUser { get; set; } = null!;
        [NotMapped]
        [ValidateNever]
        public bool IsUsers { get; set; }
        [NotMapped]
        [ValidateNever]
        public string Username { get; set; }
        [NotMapped]
        [ValidateNever]
        public string? FotoProfilo { get; set; }
    }
}
