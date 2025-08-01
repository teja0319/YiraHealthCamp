using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YiraHealthCampManagerAPI.Models.CampRequest
{
    [Table("HealthServices", Schema = "HCM")]
    public class HealthService
    {
        [Key]
        public int ServiceID { get; set; }
        public string ServiceName { get; set; }
        public bool? Status { get; set; }
    }

}
