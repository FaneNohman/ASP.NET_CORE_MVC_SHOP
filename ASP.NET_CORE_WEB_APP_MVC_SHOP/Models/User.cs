namespace ASP.NET_CORE_WEB_APP_MVC_SHOP.Models
{
    public class User
    {
        public Guid UserId { get; set;}

        public string? FirstName { get; set;}
        public string? LastName { get; set; }
        public string? Login { get; set; }
        public string Password { get; set; }
    }
}
