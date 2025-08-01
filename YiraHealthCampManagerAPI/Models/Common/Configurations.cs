namespace YiraHealthCampManagerAPI.Models.Common
{
    public class JWTTokenConfigCreds
    {
        public JWTTokenConfigCreds()
        {

        }

        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
