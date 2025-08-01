using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YiraHealthCampManagerAPI.Models.CampRequest
{
    [Table("HealthCampRequest",Schema ="HCM")]
    public class HealthCampRequest
    {
        [Key]
        public int Id { get; set; }
        public string ApprovalStatus { get; set; }
        public int OrgId { get; set; }
        public string CampName { get; set; }
        public int? EmployeesCount { get; set; }
        public string CampDuration { get; set; }
        public DateTime? PreferredDate { get; set; }
        public string SpecialMedicalRequirements { get; set; }
        public string AvailableFacilities { get; set; }
        public string AdditionalNote { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
