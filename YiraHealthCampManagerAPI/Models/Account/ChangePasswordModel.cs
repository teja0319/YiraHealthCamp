using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YiraHealthCampManagerAPI.Models.Account
{
    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string userID { get; set; }
    }
}
