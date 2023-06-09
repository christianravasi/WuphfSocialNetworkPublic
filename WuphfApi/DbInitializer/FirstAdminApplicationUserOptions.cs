namespace WuphfApi.DbInitializer
{
    public class FirstAdminApplicationUserOptions
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string StreetAddress { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FotoProfilo { get; set; } = string.Empty;
        public string TelegramToken { get; set; } = string.Empty;
        public bool IsEmailConfirmed { get; set; } = true;

    }
}
