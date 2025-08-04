namespace YiraHealthCampManagerAPI.Models.Request
{
    public class OrganizationRequestModel
    {
        public int? OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public string PhoneNumber { get; set; }
        public string? EmailID { get; set; }
        public string? AdminUserName { get; set; }
        public int NoUsers { get; set; }
        public string? AspnetUserID { get; set; }
        public string? Logo { get; set; }
        public string? TimeZone { get; set; }
        public int? CountryCodeId { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
    }
}
