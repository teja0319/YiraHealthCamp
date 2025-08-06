using YiraHealthCampManagerAPI.Models.Common;
using YiraHealthCampManagerAPI.Models.Request;

namespace YiraHealthCampManagerAPI.Interfaces.ServiceInterfaces
{
    public interface IOrganizationService 
    {
        Task<Response<object>> AddUpdateOrganizationDetails(OrganizationRequestModel organizationRequestModel);
        Task<Response<object>> GetOrgTypes();
        Task<Response<object>> GetOrganizationsInfo();
        Task<Response<object>> GetOrganizations(int pageNumber, int pageSize, int industryId, string status, string search);

    }
}
