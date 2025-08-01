namespace YiraHealthCampManagerAPI.Models.Request
{
    public class HealthCampRequestModel
    {
        public int Id { get; set; }
        public int OrgId { get; set; }
        public string UserId { get; set; }
        public int? EmployeesCount { get; set; }
        public string CampName { get; set; }
        public string CampDuration { get; set; }
        public DateTime? PreferredDate { get; set; }
        public string SpecialMedicalRequirements { get; set; }
        public string AvailableFacilities { get; set; }
        public string AdditionalNote { get; set; }
        public List<HealthCampServiceRequest> ServiceRequest { get; set; }
    }

    public class GetHealthCampData
    {
        public int OrgId { get; set; }
        public string ApprovalStatus { get; set; }
    }

    public class UpdateHealthCampDataStatus
    {
        public int Id { get; set; }
        public string ApprovalStatus { get; set; }
    }


    public class HealthCampServiceRequest
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
    }
}
