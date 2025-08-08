using YiraHealthCampManagerAPI.Models.Common;
using YiraHealthCampManagerAPI.Models.Request;
using YiraHealthCampManagerAPI.Models.Response;

namespace YiraHealthCampManagerAPI.Interfaces.RepositoryInterfaces
{
    public interface IHealthCampRepository
    {
        Task<bool> CreateAndUpdateHealthCampRequest(HealthCampRequestModel healthCampRequest);
        Task<Response<object>> GetAllHealthCampRequestsByOrgId(int OrgId, string approvalStatus, int pageNumber = 1, int pageSize = 10);
        Task<HealthCampResponseModel> GetHealthCampRequestById(int id);
        Task<bool> HealthCampStatusUpdate(int camp, string ApprovalStatus);
        Task<OrgContactDetailsResponse> GetOrgContactDetails(int orgId);
        Task<DashboardStatsResponse> GetDashboardStatsAsync();
        Task<List<HealthCampServiceRequestResponse>> ActiveHealthCampServices();

    }
}
