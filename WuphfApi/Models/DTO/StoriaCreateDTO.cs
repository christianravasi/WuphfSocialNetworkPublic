namespace WuphfApi.Models.DTO
{
    public class StoriaCreateDTO
    {
        public string Testo { get; set; } = null!;
        public string? Immagine { get; set; }
        public string? Video { get; set; }
        public string? FkUser { get; set; }
    }
}
