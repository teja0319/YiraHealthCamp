using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace YiraHealthCampManagerAPI.Models.Account
{
        public class LoginModel
        {
            [Required(ErrorMessage = "Username or password is Invalid")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Username or password is Invalid")]
            public string Password { get; set; }
        }

        public class LoginVerificationRequest
        {
            [Required(ErrorMessage = "Please Enter Verification Code", AllowEmptyStrings = false)]
            public string VerificationCode { get; set; }

        }

        public class LoginVerificationResponse
        {
            public string VerificationCode { get; set; }
            public string DoctorId { get; set; }

        }
        public class EmailRefreshRequest
        {
            public string Email { get; set; }
        }
}
