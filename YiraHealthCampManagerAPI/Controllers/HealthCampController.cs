using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using YiraHealthCampManagerAPI.Interfaces.ServiceInterfaces;
using YiraHealthCampManagerAPI.Models.Request;

namespace YiraHealthCampManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCampController : ControllerBase
    {
        private readonly IHealthCampService _healthCampService;
        public HealthCampController(IHealthCampService healthCampService)
        {
            _healthCampService = healthCampService;
        }

        [HttpPost]
        [Route("CreateHealthCamp")]
        public async Task<IActionResult> CreateHealthCamp([FromBody] HealthCampRequestModel healthCampRequest)
        {
            if (healthCampRequest == null)
            {
                return BadRequest("Invalid health camp request data.");
            }
            var response = await _healthCampService.CreateHealthCampRequest(healthCampRequest);
            return Ok(response);
        }

        [HttpPost]
        [Route("GetAllHealthCampRequestsByOrgId")]
        public async Task<IActionResult> GetAllHealthCampRequestsByOrgId(int OrgId , int pageNumber = 1, int pageSize = 10)
        {
            var response = await _healthCampService.GetAllHealthCampRequestsByOrgId(OrgId);
            return Ok(response);
        }
        [HttpGet]
        [Route("GetHealthCampRequestById/{id}")]
        public async Task<IActionResult> GetHealthCampRequestById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid health camp request ID.");
            }
            var response = await _healthCampService.GetHealthCampRequestById(id);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetHealthServices")]
        public async Task<IActionResult> GetHealthServices()
        {
            var response = await _healthCampService.GetHealthServices();
            return Ok(response);
        }
        [HttpPost]
        [Route("HealthCampStatusUpdate")]
        public async Task<IActionResult> HealthCampStatusUpdate(UpdateHealthCampDataStatus updateHealthCampDataStatus)
        {
            var response = await _healthCampService.HealthCampStatusUpdate(updateHealthCampDataStatus.Id, updateHealthCampDataStatus.ApprovalStatus);
            return Ok(response);
        }

        [HttpPost]
        [Route("HealthCampDataByStatusAndOrg")]
        public async Task<IActionResult> HealthCampDataByStatusAndOrg(GetHealthCampData campData)
        {
            var response = await _healthCampService.HealthCampDataByStatusAndOrg(campData.OrgId, campData.ApprovalStatus);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetDashboardStats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var response = await _healthCampService.GetDashboardStatsAsync();
            if (response.status)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
