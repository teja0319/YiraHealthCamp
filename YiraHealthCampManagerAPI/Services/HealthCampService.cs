using YiraHealthCampManagerAPI.Interfaces.RepositoryInterfaces;
using YiraHealthCampManagerAPI.Interfaces.ServiceInterfaces;
using YiraHealthCampManagerAPI.Models.CampRequest;
using YiraHealthCampManagerAPI.Models.Common;
using YiraHealthCampManagerAPI.Models.Request;
using YiraHealthCampManagerAPI.Models.Response;

namespace YiraHealthCampManagerAPI.Services
{
    public class HealthCampService : IHealthCampService
    {

        private readonly IHealthCampRepository _healthCampRepository;
        public HealthCampService(IHealthCampRepository healthCampRepository)
        {
            _healthCampRepository = healthCampRepository;
        }
        public async Task<Response<object>> CreateHealthCampRequest(HealthCampRequestModel healthCampRequest)
        {
            Response<object> response = new Response<object>();
            try
            {
                if (healthCampRequest == null)
                {
                    response.status = false;
                    response.message = "Invalid health camp request.";
                    return response;
                }
                 bool result = await _healthCampRepository.CreateAndUpdateHealthCampRequest(healthCampRequest);
                if (result)
                {
                    response.status = true;
                    response.message = "Health camp request created successfully.";
                }
                else
                {
                    response.status = false;
                    response.message = "Failed to create health camp request.";
                }
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "An error occurred while creating the health camp request.";
                return response;

            }
        }

        public async Task<Response<object>> GetHealthServices()
        {
            Response<object> response = new Response<object>();
            try
            {
                var healthCampServices = await _healthCampRepository.ActiveHealthCampServices();
                if (healthCampServices != null && healthCampServices.Count > 0)
                {
                    response.status = true;
                    response.data = healthCampServices;
                    response.message = "Health camp services retrieved successfully.";
                }
                else
                {
                    response.status = false;
                    response.data = new List<HealthCampServiceRequestResponse>();
                    response.message = "No active health camp services found.";
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "An error occurred while retrieving health camp services.";
            }
            return response;
        }

        public async Task<Response<List<HealthCampResponseModel>>> GetAllHealthCampRequestsByOrgId(int OrgId)
        {
            Response<List<HealthCampResponseModel>> response = new Response<List<HealthCampResponseModel>>();
            try
            {
                var healthCampRequests = await _healthCampRepository.GetAllHealthCampRequestsByOrgId(OrgId);
                if (healthCampRequests != null && healthCampRequests.Count > 0)
                {
                    response.status = true;
                    response.data = healthCampRequests;
                    response.message = "Health camp requests retrieved successfully.";
                }
                else
                {
                    response.status = false;
                    response.data = new List<HealthCampResponseModel>();
                    response.message = "No health camp requests found for the organization.";
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "An error occurred while retrieving health camp requests.";
            }
            return response;
        }

        public async Task<Response<HealthCampResponseModel>> GetHealthCampRequestById(int id)
        {
            Response<HealthCampResponseModel> response = new Response<HealthCampResponseModel>();
            try
            {
                var healthCampRequest = await _healthCampRepository.GetHealthCampRequestById(id);
                if (healthCampRequest != null)
                {
                    response.status = true;
                    response.data = healthCampRequest;
                    response.message = "Health camp request retrieved successfully.";
                }
                else
                {
                    response.status = false;
                    response.data = null;
                    response.message = "Health camp request not found.";
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "An error occurred while retrieving the health camp request.";
            }
            return response;
        }

        public async Task<Response<object>> HealthCampStatusUpdate(int camp, string ApprovalStatus)
        {
            Response<object> response = new Response<object>();
            try
            {
                bool result = await _healthCampRepository.HealthCampStatusUpdate(camp, ApprovalStatus);
                if (result)
                {
                    response.status = true;
                    response.data = true;
                    response.message = "Health camp status updated successfully.";
                }
                else
                {
                    response.status = false;
                    response.data = false;
                    response.message = "Failed to update health camp status.";
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.data = false;
                response.message = "An error occurred while updating the health camp status.";
            }
            return response;
        }

        public async Task<Response<object>> HealthCampDataByStatusAndOrg(int OrgId, string ApprovalStatus)
        {
           Response<object> response = new Response<object>();

            try
            {
                var healthCampRequests = await _healthCampRepository.HealthCampDataByStatusAndOrg(OrgId, ApprovalStatus);
                if (healthCampRequests != null && healthCampRequests.Count > 0)
                {
                    response.status = true;
                    response.data = healthCampRequests;
                    response.message = "Health camp requests retrieved successfully.";
                }
                else
                {
                    response.status = false;
                    response.data = healthCampRequests;
                    response.message = "No health camp requests found for the organization with the specified status.";
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = "An error occurred while retrieving health camp requests by status and organization.";
            }
            return response;
        }

        public async Task<Response<object>> GetDashboardStatsAsync()
        {
            Response<object> response = new Response<object>();

            try
            {

                var DashboardStats = await _healthCampRepository.GetDashboardStatsAsync();
                if (DashboardStats != null)
                {
                    response.status = true;
                    response.data = DashboardStats;
                    response.message = "Dashboard statistics retrieved successfully.";
                }
                else
                {
                    response.status = false;
                    response.data = null;
                    response.message = "No dashboard statistics found.";
                }
                return response;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.data = null;
                response.message = "An error occurred while retrieving dashboard statistics.";
                return response;

            }
        }


    }
}
