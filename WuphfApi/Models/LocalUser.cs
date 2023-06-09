namespace WuphfApi.Models
{
    public class LocalUser
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string? FotoProfilo { get; set; }
    }
}
