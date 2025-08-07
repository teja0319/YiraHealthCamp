using YiraHealthCampManagerAPI.Models.Common;
using YiraHealthCampManagerAPI.Models.Request;
using YiraHealthCampManagerAPI.Models.Response;

namespace YiraHealthCampManagerAPI.Interfaces.RepositoryInterfaces
{
    public interface IOrganizationRepository
    {
        Task<int> AddUpdateOrganizationDetails(OrganizationRequestModel organizationRequestModel);
        Task<bool> IsOrganizationNameExists(string orgName);
        Task<List<OrganizationTypesResponse>> OrganizationTypes();
        Task<AllOrgsInfo> OrganizationInfo();
        Task<bool> UpdateAspnetUserID(long organizationID, string aspnetUserID);
        Task<Response<object>> GetOrgnizations(int pageNumber, int pageSize, int industryId, string status, string searchTerm);
    }
}
