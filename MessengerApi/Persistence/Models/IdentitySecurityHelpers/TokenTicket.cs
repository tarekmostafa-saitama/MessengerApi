namespace MessengerApi.Persistence.Models.IdentitySecurityHelpers
{
    public class TokenTicket
    {
        public string UserName { get; set; }
        public string Role { get; set; }
        public bool Authorized { get; set; }
    }
}