namespace WuphfApi.Models.DTO
{
    public class LikeCreateDTO
    {
        public int? FkPost { get; set; }
        public int? FkCommento { get; set; }
        public string FkUser { get; set; } = null!;
    }
}
