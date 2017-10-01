namespace WorldDomination.DelegatedAuthentication.WebApi.Models
{
    public class ApplicationSettings
    {
        public string Auth0Authority { get; set; }
        public string Auth0Audience { get; set; }
        public string Auth0Secret { get; set; }
        public string CustomAuthority { get; set; }
        public string CustomAudience { get; set; }
        public string CustomSecret { get; set; }
    }
}