using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YiraHealthCampManagerAPI.Models.Organization
{
    [Table("Organizations")]
    public class Organizations
    {
        [Key]
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public string PhoneNumber { get; set; }
        public string? EmailID { get; set; }
        public int NoUsers { get; set; }
        public string? AspnetUserID { get; set; }
        public string? AdminUserName { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int StatusTypeID { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool? isActive { get; set; }
        public string? Theme { get; set; }
        public string? Logo { get; set; }
        public int DashBoardTemplate { get; set; }
        public int SetupStatus { get; set; }
        public string UrlName { get; set; }
        public string? AndroidTheme { get; set; }
        public bool SecondaryTests { get; set; }
        public string? OrgQRCode { get; set; }
        public int? OrgTypeId { get; set; }
        public int? OrganizationPlanId { get; set; }
        public string? TimeZone { get; set; }
        public int? CountryCodeId { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? Address { get; set; }
        public string? HIPNumber { get; set; }
        public string? Description { get; set; }
        public string? City { get; set; }
    }
}
