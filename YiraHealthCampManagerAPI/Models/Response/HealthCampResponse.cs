namespace YiraHealthCampManagerAPI.Models.Response
{
    public class HealthCampResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class HealthCampResponseModel
    {
        public int Id { get; set; }
        public string ApprovalStatus { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string CampName { get; set; }
        public int OrgId { get; set; }
        public string UserId { get; set; }
        public int? EmployeesCount { get; set; }
        public string CampDuration { get; set; }
        public DateTime? PreferredDate { get; set; }
        public string SpecialMedicalRequirements { get; set; }
        public string AvailableFacilities { get; set; }
        public string AdditionalNote { get; set; }
        public List<HealthCampServiceRequestResponse> ServiceRequest { get; set; }
    }

    public class HealthCampServiceRequestResponse
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
    }

    public class DashboardStatsResponse
    {
        public int TotalRequests { get; set; }
        public int RequestsChange { get; set; }
        public double ApprovalRate { get; set; }
        public double ApprovalRateChange { get; set; }
        public double AvgProcessingTime { get; set; }
        public double ProcessingTimeChange { get; set; }
        public List<RequestedServiceStat> MostRequestedServices { get; set; }
    }

    public class RequestedServiceStat
    {
        public string ServiceName { get; set; }
        public int Count { get; set; }
    }



}
