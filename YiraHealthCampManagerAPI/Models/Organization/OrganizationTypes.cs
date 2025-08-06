using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YiraHealthCampManagerAPI.Models.Organization
{
    [Table("OrganizationTypes")]
    public class OrganizationTypes
    {
        [Key]

        public int Id { get; set; }
        public string TypeName { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

    }
}
