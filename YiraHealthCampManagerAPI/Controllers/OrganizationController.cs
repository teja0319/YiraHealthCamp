using Microsoft.AspNetCore.Mvc;
using YiraHealthCampManagerAPI.Interfaces.ServiceInterfaces;
using YiraHealthCampManagerAPI.Models.Request;

namespace YiraHealthCampManagerAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;
        public OrganizationController(IOrganizationService organizationService) {

            _organizationService = organizationService;

        }

        [HttpPost]
        [Route("AddUpdateOrganizationDetails")]
        public async Task<IActionResult> AddUpdateOrganizationDetails([FromBody] OrganizationRequestModel organizationRequestModel)
        {
            var response = await _organizationService.AddUpdateOrganizationDetails(organizationRequestModel);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetOrganizationTypes")]
        public async Task<IActionResult> GetOrganizationTypes()
        {
            var response = await _organizationService.GetOrgTypes();
            return Ok(response);
        }

        [HttpGet]
        [Route("GetOrganizations")]
        public async Task<IActionResult> GetOrganizations(int pageNumber = 1, int pageSize = 10, int industryId = 0 ,string status = null , string search = null)
        {
                var response = await _organizationService.GetOrganizations(pageNumber, pageSize , industryId , status, search);
                return Ok(response);
        }

        [HttpGet]
        [Route("GetOrganizationsInfo")]
        public async Task<IActionResult> GetOrganizationsInfo()
        {
            var response = await _organizationService.GetOrganizationsInfo();
            return Ok(response);

        }


    }
}
