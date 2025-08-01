using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YiraHealthCampManagerAPI.Models.Account
{
    [Table("DeletedAccountsLog")]
    public class DeletedAccountsLog
    {
        [Key]
        public int Id { get; set; }
        public string OldEmail { get; set; }
        public string NewEmail { get; set; }
        public string OldPhone { get; set; }
        public string NewPhone { get; set; }
        public string UserId { get; set; }
        public DateTime DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        public string Reason { get; set; }
    }
}
