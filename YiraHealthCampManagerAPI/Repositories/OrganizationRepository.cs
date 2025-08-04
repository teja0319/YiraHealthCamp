using Microsoft.EntityFrameworkCore;
using YiraHealthCampManagerAPI.Context;
using YiraHealthCampManagerAPI.Interfaces.RepositoryInterfaces;
using YiraHealthCampManagerAPI.Models.Organization;
using YiraHealthCampManagerAPI.Models.Request;

namespace YiraHealthCampManagerAPI.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly YiraDbContext _context;
        public OrganizationRepository(YiraDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddUpdateOrganizationDetails(OrganizationRequestModel organizationRequestModel)
        {
            try
            {
                var existingOrg = await _context.organizations.FindAsync(organizationRequestModel.OrganizationID);
                if (existingOrg != null)
                {
                    existingOrg.OrganizationName = organizationRequestModel.OrganizationName;
                    existingOrg.Address = organizationRequestModel.Address;
                    existingOrg.PhoneNumber = organizationRequestModel.PhoneNumber;
                    existingOrg.EmailID = organizationRequestModel.EmailID;
                    existingOrg.UpdatedDate = DateTime.UtcNow;
                    _context.organizations.Update(existingOrg);
                }
                else
                {
                    var newOrg = new Organizations
                    {
                        OrganizationName = organizationRequestModel.OrganizationName,
                        Address = organizationRequestModel.Address,
                        PhoneNumber = organizationRequestModel.PhoneNumber,
                        EmailID = organizationRequestModel.EmailID,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow,
                        isActive = false,
                        CreatedBy = "System",
                        UpdatedBy = "System",
                        StatusTypeID = 1,
                        NoUsers = organizationRequestModel.NoUsers,
                        RegistrationDate = DateTime.UtcNow,
                        UrlName = organizationRequestModel.OrganizationName,
                        SecondaryTests = false,
                        AdminUserName = organizationRequestModel.AdminUserName,
                        SetupStatus = 0,
                        DashBoardTemplate = 1,

                    };
                    await _context.organizations.AddAsync(newOrg);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding/updating organization details.", ex);
            }

        }

        public async Task<bool> IsOrganizationNameExists(string orgName)
        {
            try
            {
                return await _context.organizations.AnyAsync(org => org.OrganizationName.ToLower() == orgName.ToLower());
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while checking if the organization exists.", ex);
            }
        }

    }
}
