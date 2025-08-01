using System.ComponentModel.DataAnnotations;

namespace YiraHealthCampManagerAPI.Models.Account
{
    public class Deleteuserrequest
  
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
