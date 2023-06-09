namespace WuphfApi.Models.DTO
{
    public class CommentoCreateDTO
    {
        public string Testo { get; set; } = null!;
        public DateTime DataCreazione { get; set; }
        public string FkUser { get; set; }
        public int FkPost { get; set; }
    }
}
