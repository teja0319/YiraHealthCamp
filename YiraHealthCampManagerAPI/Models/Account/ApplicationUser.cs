using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace YiraHealthCampManagerAPI.Models.Account
{
    public class ApplicationUser : IdentityUser
    {
        public int? IsFirstTimeUser { get; set; }
        public string? OTP { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int? Age { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool Status { get; set; }
        public string? EmployeeID { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ProfileID { get; set; }
        public int? OrganizationID { get; set; }
        public string UserType { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime? PasswordResetDate { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public double? BMI { get; set; }
        public int? Gender { get; set; }
        public string? ImagePath { get; set; }
        public string? UniqueUserName { get; set; }
        public string? CountryCode { get; set; }

        public string? QRCodeImageUrl { get; set; }
        public string? Specialization { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public string? UserSignature { get; set; }
        public bool? PatientView { get; set; }
        public string? Token { get; set; }
        public string? HeightUnit { get; set; }
        public string? WeightUnit { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Address { get; set; }
        public string? Pincode { get; set; }
        public string? LicenseNumber { get; set; }
        public string? Stream { get; set; }
        public string? BloodGroup { get; set; }
        public string? Class { get; set; }
        public string? Section { get; set; }
        public bool  HealthConnectEnabled { get; set; }
        public string? RelationToIP { get; set; }
        public string? UID { get; set; }
        public string? IPNO { get; set; }
        public string? AadharNo { get; set; }
        public string? EmployerName { get; set; }
        public string? CompanyAddress { get; set; }
        public string? HomeAddress { get; set; }
        public string? AlternateMobile { get; set; }
        public string? NormalizedEmail { get; set; }
        public bool IsMedicalDoctor { get; set; }
        public DateTime? TokenExpirationTime { get; set; }
    }

    public static class IdentityExtensions
    {
        public static int GetOrganizationID(this IIdentity identity)
        {
            return Convert.ToInt32(((ClaimsIdentity)identity).FindFirst("OrganizationID").Value);
        }
        public static int GetName(this IIdentity identity)
        {
            return Convert.ToInt32(((ClaimsIdentity)identity).FindFirst("Name").Value);
        }
        public static int GetProfileID(this IIdentity identity)
        {
            return Convert.ToInt32(((ClaimsIdentity)identity).FindFirst("ProfileID").Value);
        }

        public static int GetUserType(this IIdentity identity)
        {
            return Convert.ToInt32(((ClaimsIdentity)identity).FindFirst("UserType").Value);
        }
        //public static int GetUserID(this IIdentity identity)
        //{
        //    return Convert.ToInt32(((ClaimsIdentity)identity).FindFirst("Id").Value);
        //}
        public static string GetLoginUserName(this IIdentity identity)
        {
            string UserName = ((ClaimsIdentity)identity).FindFirst("UserName").Value;
            return UserName;
        }
        public static string GetUserID(this IIdentity identity)
        {
            string UserID = ((ClaimsIdentity)identity).FindFirst("UserID").Value;
            //return Convert.ToInt32(UserID);
            return UserID;
        }
        public static bool PatientView(this IIdentity identity)
        {
            return Convert.ToBoolean(((ClaimsIdentity)identity).FindFirst("PatientView").Value);
        }
        public static bool CanAccessToAllNotes(this IIdentity identity)
        {
            return Convert.ToBoolean(((ClaimsIdentity)identity).FindFirst("CanAccessToAllNotes").Value);
        }
    }
}
