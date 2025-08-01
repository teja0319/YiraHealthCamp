using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YiraHealthCampManagerAPI.Models.Account
{
    public class AccountModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    [Keyless]
    public class GetLogomainRes
    {
        public string Logo { get; set; }
        public string OrgContent { get; set; }
        public string QuestionniarePdfUrl { get; set; }
        public string QuestionnaireFileName { get; set; }
        public int OrganizationID { get; set; }
        public string OrganizationEMailId { get; set; }
        public string Subject { get; set; }
        public string Emailalias { get; set; }
        public string Host { get; set; }
        public string FromEmail { get; set; }

        public string AppPassword { get; set; }

    }
    public class SavePasswordModel
    {
        [Required(ErrorMessage = "{0} is required")]
        public string userName { get; set; }

        [StringLength(15, ErrorMessage = "Must be between 5 and 15 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} is required")]
        public string password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} is required")]
        [Compare("password", ErrorMessage = "Password and Confirm Password does not match")]
        public string confirmPassword { get; set; }

        public string secretKey { get; set; }
    }

    public class TokenResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string UserImageURL { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string UserId { get; set; }
    }
    public class RefreshTokenRequest
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }

    public class AppConfiguration
    {
        public string Secret { get; set; }
    }
    public class ThirdPartyGeneratetokenReq
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string OrganizationName { get; set; }
        [Required]
        public string MobileNo { get; set; }
    }
    public class UserProfileCreationReq
    {
        public string Name { get; set; }
        public string AdmissionNo { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Gender { get; set; }
    }
    [Keyless]
    public class GetAdmissionNobyUserId
    {
        public string Id { get; set; }
    }
    public class GetuseridbyAdmissionNo
    {
        public string Id { get; set; }
    }
    public class OrgBaseConfig
    {
        //public int ID { get; set; }
        public int OrganizationID { get; set; }
        //public string ControlType { get; set; }
        //public bool Status { get; set; }
        public string ScreenName { get; set; }
    }

    [Keyless]
    public class Getorgbaseconfigres
    {
        public string configres { get; set; }
    }
    public class WelcomeEmailsendReq
    {
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }

    public class UserRequest
    {
        [Required]

        public string UserId { get; set; }
    }

    public class APIRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }

    public class UserPeriodsRequest
    {
        [Required]

        public string UserId { get; set; }


    }




    public class UserTokenRequest
    {
        [Required]
        public string Token { get; set; }
        public string DoctorId { get; set; }
        public string UserId { get; set; }
    }

    //  Pregnancy Due Date Model
    [Keyless]

    public class GetUserPregnancyResponse
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public DateTime DueDate { get; set; }
        public string WeeksDaysMonths { get; set; }
        public string Trimester { get; set; }
        public string AverageHeightWeight { get; set; }
        public string ConceptionDate { get; set; }
        public string PercentageCompleted { get; set; }
        public string PregnancyDetails { get; set; }
        [NotMapped]
        public List<GetUserPregnancyResponseDetail> PregnancyDetailsModel { get; set; }
    }

    [Keyless]
    public class GetAPIDetails
    {
        public string Data { get; set; }
        //public string Path { get; set; }
        //public string RequestBody { get; set; }
        //public string Response { get; set; }

        //public string ResponseStatusCode { get; set; }
        //public DateTime RequestedOn { get; set; }
    }



    //public string Path { get; set; }
    //public string RequestBody { get; set; }
    //public string Response { get; set; }

    //public string ResponseStatusCode { get; set; }
    //public DateTime RequestedOn { get; set; }



    public class APILog
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public string RequestBody { get; set; }
        public string Response { get; set; }
        public string QueryString { get; set; }
        public int ResponseStatusCode { get; set; }
        public DateTime RequestedOn { get; set; }
    }

    //  Periods Due Date Model
    [Keyless]

    public class GetUserPeriodsResponse
    {
        public int OUTPUT { get; set; }
        public string UserId { get; set; }
        public string PeriodsDetails { get; set; }
        [NotMapped]
        public List<GetUserPeriodsResponseDetail> PeriodsDetailsModel { get; set; }

    }
    public class GetUserDetailsResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public int Specialization { get; set; } // this property applicable only for doctor
        public string PatientName { get; set; }// this property applicable only for doctor
        public string PatientEmail { get; set; }
        public string PatientId { get; set; }
        public int PatientAge { get; set; }
        public int PatientGender { get; set; }
        public string PatientCountryCode { get; set; }
        public string PatientPhoneNumber { get; set; }
        public string SpecializationName { get; set; }
    }
    public class OrgUsers
    {
        [Key]
        public Int64 ProfileID { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; } //= "f650f0b6-b001-4e3f-bedf-72affb602f04";
        public int OrganizationID { get; set; }
        public string CreatedBy { get; set; } = "System";
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }


    public class GetUserPregnancyResponseDetail
    {
        public int ID { get; set; }
        public int Week { get; set; }
        public string Date { get; set; }
        public string Trimester { get; set; }

        [JsonProperty("Important Milestones")]
        public string ImportantMilestones { get; set; }
    }



    //public class GetAPIDetails
    //{
    //    public string Method { get; set; }
    //    public string Path { get; set; }
    //    public string RequestBody { get; set; }
    //    public string Response { get; set; }

    //    public string ResponseStatusCode { get; set; }
    //    public DateTime RequestedOn { get; set; }
    //}


    public class GetUserPeriodsResponseDetail
    {
        public int ID { get; set; }

        [JsonProperty("Period Days")]
        public string PeriodDays { get; set; }

        [JsonProperty("Most Probable Ovulation Days")]
        public string MostProbableOvulationDays { get; set; }
    }

    public class scoreCalculationModel
    {
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
        public string TotalScore { get; set; }

        public string CategoryType { get; set; }
        public bool sendEmail { get; set; }

    }

    public class GetTotalScoreCalculation
    {
        public string UserId { get; set; }
        public string TotalScore { get; set; }

        public string CategoryType { get; set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class UpdateUserNamemodel
    {
        public int OrganizationId { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public string userName { get; set; }

    }
}
