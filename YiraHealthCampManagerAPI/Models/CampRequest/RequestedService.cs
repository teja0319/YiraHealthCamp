using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YiraHealthCampManagerAPI.Models.CampRequest
{
    [Table("RequestedServices",Schema = "HCM")]
    public class RequestedService
    {
        [Key]
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int HealthCampRequestId { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

    }

}
