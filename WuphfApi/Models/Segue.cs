using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace WuphfApi.Models
{
    public class Segue
    {
        public int Id { get; set; }
        public string Follower { get; set; } = null!;
        public string Following { get; set; } = null!;
        [ForeignKey(nameof(Follower))]
        [ValidateNever]
        public ApplicationUser User1 { get; set; } = null!;
        [ForeignKey(nameof(Following))]
        [ValidateNever]
        public ApplicationUser User2 { get; set; } = null!;
    }
}
