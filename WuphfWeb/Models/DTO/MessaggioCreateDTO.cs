namespace WuphfWeb.Models.DTO
{
    public class MessaggioCreateDTO
    {
        public string Testo { get; set; } = null!;
        public string Sender { get; set; } = null!;
        public string Receiver { get; set; } = null!;
        public int IdChat { get; set; }
    }
}
