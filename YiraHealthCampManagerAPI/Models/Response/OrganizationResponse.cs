namespace YiraHealthCampManagerAPI.Models.Response
{
    public class OrganizationResponse
    {
        public int OrganizationID { get; set; }
        public int CampsCmplted { get; set; }
        public string OrganizationName { get; set; }
        public string Industry { get; set; }
        public string PhoneNumber { get; set; }
        public string? EmailID { get; set; }
        public int Employees { get; set; }
        public string? AdminUserName { get; set; }
        public int? HealthScore { get; set; }
        public DateTime? LastCamp { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool? isActive { get; set; }
    }

    public class OrganizationTypesResponse
    {
        public int OrganizationTypeId { get; set; }
        public string OrganizationTypeName { get; set; }
    }

    public class  AllOrgsInfo
    {
        public OrganizationsCount organizationsCount { get; set; }
        public List<OrganizationTypesCount> OrganizationTypes { get; set; }

    }

    public class OrganizationTypesCount
    {
        public int OrganizationCount { get; set; }
        public string OrganizationType { get; set; }
    }


    public class OrganizationsbyType
    {
        public List<OrganizationTypesResponse> OrganizationTypes { get; set; }
    }

    public class OrganizationsCount
    {
        public int TotalEmployees { get; set; }
        public int ActiveOrganizations { get; set; }
        public int TotalOrganizations { get; set; }
        public int InActive { get; set; }
        public int PendingApproval { get; set; }
    }


}
