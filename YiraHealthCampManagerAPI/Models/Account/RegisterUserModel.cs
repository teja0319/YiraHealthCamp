using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YiraHealthCampManagerAPI.Models.Account
{
    public class RegisterUserModel
    {
        [Required(ErrorMessage = "{0} is required.")]
        //[StringLength(20)] --Removing in Feature-V9 because unable to upload users from excel if this enabled.
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        //[Required(ErrorMessage = "{0} is required.")]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "{0} is required.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public string CountryCode { get; set; }

        //[Required(ErrorMessage = "{0} is required.")]
        public DateTime DateOfBirth { get; set; }

        //[Required(ErrorMessage = "{0} is required.")]
        public string Gender { get; set; }

        [StringLength(15, ErrorMessage = "Must be between 5 and 15 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} is required.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} is required.")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password does not match.")]
        public string ConfirmPassword { get; set; }

        public int OrganizationId { get; set; }
        public string UserType { get; set; }
        public string Specialization { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Address { get;set; }
        public string Pincode { get; set; }
        public string Stream { get; set; }
        public string BloodGroup { get; set; }
        public string Section { get; set; }
        public string Year { get; set; }
        public string EmployeeId { get; set; }
        public string RelationToIP { get; set; }
        public string UID { get; set; }
        public string IPNO { get; set; }
        public string AadharNo { get; set; }
        public string EmployerName { get; set; }
        public string CompanyAddress { get; set; }
        public string HomeAddress { get; set; }
        public string AlternateNumber { get; set; }
        public string GovtIDNumber { get; set; }

    }

    public class RegisterUserModelWeb
    {
        [Required(ErrorMessage = "{0} is required.")]
        //[StringLength(20)] --Removing in Feature-V9 because unable to upload users from excel if this enabled.
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        //[Required(ErrorMessage = "{0} is required.")]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "{0} is required.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public string CountryCode { get; set; }

        //[Required(ErrorMessage = "{0} is required.")]
        public DateTime DateOfBirth { get; set; }

        //[Required(ErrorMessage = "{0} is required.")]
        public string Gender { get; set; }

        [StringLength(15, ErrorMessage = "Must be between 5 and 15 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} is required.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} is required.")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password does not match.")]
        public string ConfirmPassword { get; set; }
        public int OrganizationId { get; set; }
        public string UserType { get; set; }
        public string Specialization { get; set; }
        public bool WelcomeEmail { get; set; }
        public bool Questionnaire { get; set; }
        public int Age { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public List<string> DoctorIds { get; set; }
        public string BloodGroup { get; set; }
        public string Stream { get; set; }
        public string Class { get; set; }
        public string Year { get; set; }
        public string EmployeeId { get; set; }
        public string RelationToIP { get; set; }
        public string UID { get; set; }
        public string IPNO { get; set; }
        public string AadharNo { get; set; }
        public string EmployerName { get; set; }
        public string CompanyAddress { get; set; }
        public string HomeAddress { get; set; }
        public string AlternateNumber { get; set; }

        //new variables
        public DateTime? CampDate { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string LanNumber { get; set; }
        public DateTime? WhatsappReportSentDate { get; set; }
        public string WhatsappReportSentTime { get; set; }
        public string WhatsappRemarks { get; set; }
        public DateTime? SmsReportSentDate { get; set; }
        public string SmsReportSentTime { get; set; }
        public string SmsRemarks { get; set; }
        public string ReportReadingStatus { get; set; }
        public int FormId { get; set; }
        public int PatientId { get; set; }
        public bool IsMedibuddy { get; set; } = false;
        public string GovtIDNumber { get; set; }
        public string Address { get; set; }

    }

    public class UpdateUserProfileWebRequest  : RegisterUserModelWeb
    {
        public string UserId { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class UpdateUserProfileWebRequest_V1 
    {
        public string UserId { get; set; }
        public string UpdatedBy { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string CountryCode { get; set; }
        public string Department { get; set; }
        public string Role { get; set; }
        public string Gender { get; set; }
    }

    public class OTPResp
    {   
        public string JobId { get; set; }
        public string Ack { get; set; }
        public string mobileNo { get; set; }
    }
    public class InternationalOTPResp
    {
        public string Status { get; set; }
        public string Details { get; set; }
    }
    public class SMSStrikerOTPResp
    {
        public string JobId { get; set; }
        public string Ack { get; set; }
        public string mobileNo { get; set; }
    }

    public class SendOTPModel
    {
        [DataType(DataType.EmailAddress)]
        //[Required(ErrorMessage = "{0} is required", AllowEmptyStrings = false)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "{0} is required", AllowEmptyStrings = false)]
        public string PhoneNumber { get; set; }

        public string type { get; set; }
    }

    [Table("SPCL_Workers_information")]
    public class SPCLWorkerInformation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Sr.No")]
        public string SrNo { get; set; }

        [Column("ID NO")]
        public string IDNo { get; set; }

        [Column("EMPLOYEE NAME")]
        public string EmployeeName { get; set; }

        [Column("GENDER")]
        public string Gender { get; set; }

        [Column("DESIGNATION")]
        public string Designation { get; set; }

        [Column("DATE OF BIRTH")]
        [DataType(DataType.Date)]
        public string DateOfBirth { get; set; }

        [Column("DATE OF JOINING")]
        [DataType(DataType.Date)]
        public string DateOfJoining { get; set; }

        [Column("LOCATION")]
        public string Location { get; set; }

        [Column("SITE CODE")]
        public string SiteCode { get; set; }

        [Column("SITE NAME")]
        public string SiteName { get; set; }

        [Column("SUB CONTRACTOR NAME")]
        public string SubContractorName { get; set; }

        [Column("SUB CONTRACTOR ADRESS")]
        public string SubContractorAddress { get; set; }

        [Column("AADHAAR Card No")]
        public string AadhaarCardNo { get; set; }

        [Column("PAN CARD No")]
        public string PanCardNo { get; set; }

        [Column("MOBILE No")]
        public string MobileNo { get; set; }

        public bool IsUserRegistered { get;set;  }
    }

    [Table("SPCL_STAFF_INFORMATION")]
    public class SPCLStaffInformation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("S.no")]
        public string SNo { get; set; }

        [Column("ERP NO")]
        public string ErpNo { get; set; }

        [Column("NAME")]
        public string Name { get; set; }

        [Column("Cadre")]
        public string Cadre { get; set; }

        [Column("Designation")]
        public string Designation { get; set; }

        [Column("Department")]
        public string Department { get; set; }

        [Column("Contact details")]
        public string ContactDetails { get; set; }

        [Column("Gender")]
        public string Gender { get; set; }

        [Column("IsUserRegistered")]
        public bool IsUserRegistered { get; set; } = false; // Default value
    }

    [Keyless]
    public class CardioScore
    {
        public string Name { get; set; }
        public string UID { get; set; }
        public decimal Score { get; set; }
        public string Category { get; set; }
        public string RiskType { get; set; }
    }
    public class PhoneNumberEdit
    {
        public string UserId { get; set; }
        
    }
}
