namespace WorldDomination.DelegatedAuthentication.Application.Authentication
{
    public class CustomJwt : Jwt
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}