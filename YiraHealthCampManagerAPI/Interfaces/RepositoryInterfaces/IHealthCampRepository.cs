using YiraHealthCampManagerAPI.Models.Request;
using YiraHealthCampManagerAPI.Models.Response;

namespace YiraHealthCampManagerAPI.Interfaces.RepositoryInterfaces
{
    public interface IHealthCampRepository
    {
        Task<bool> CreateAndUpdateHealthCampRequest(HealthCampRequestModel healthCampRequest);
        Task<List<HealthCampResponseModel>> GetAllHealthCampRequestsByOrgId(int OrgId);
        Task<HealthCampResponseModel> GetHealthCampRequestById(int id);
        Task<bool> HealthCampStatusUpdate(int camp, string ApprovalStatus);
        Task<List<HealthCampResponseModel>> HealthCampDataByStatusAndOrg(int OrgId, string ApprovalStatus);
        Task<DashboardStatsResponse> GetDashboardStatsAsync();

    }
}
