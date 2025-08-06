using YiraHealthCampManagerAPI.Models.Common;
using YiraHealthCampManagerAPI.Models.Request;
using YiraHealthCampManagerAPI.Models.Response;

namespace YiraHealthCampManagerAPI.Interfaces.ServiceInterfaces
{
    public interface IHealthCampService
    {
        Task<Response<object>> CreateHealthCampRequest(HealthCampRequestModel healthCampRequest);
        Task<Response<List<HealthCampResponseModel>>> GetAllHealthCampRequestsByOrgId(int OrgId , int pageNumber = 1, int pageSize = 10);
        Task<Response<HealthCampResponseModel>> GetHealthCampRequestById(int id);
        Task<Response<object>> HealthCampDataByStatusAndOrg(int OrgId, string ApprovalStatus);
        Task<Response<object>> HealthCampStatusUpdate(int camp, string ApprovalStatus);
        Task<Response<object>> GetDashboardStatsAsync();
        Task<Response<object>> GetHealthServices();
    }
}
