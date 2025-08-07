using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using YiraApi.Common.CustomException;
using YiraHealthCampManagerAPI.Context;
using YiraHealthCampManagerAPI.Interfaces.RepositoryInterfaces;
using YiraHealthCampManagerAPI.Models.Common;
using YiraHealthCampManagerAPI.Models.Common.Enum;
using YiraHealthCampManagerAPI.Models.Organization;
using YiraHealthCampManagerAPI.Models.Request;
using YiraHealthCampManagerAPI.Models.Response;

namespace YiraHealthCampManagerAPI.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly YiraDbContext _context;
        private static readonly Random _random = new Random();
        public OrganizationRepository(YiraDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddUpdateOrganizationDetails(OrganizationRequestModel organizationRequestModel)
        {
            try
            {
                var existingOrg = await _context.organizations.FindAsync(organizationRequestModel.OrganizationID);
                ApprovalStatus approvalStatus = organizationRequestModel.status != null
                                    ? (ApprovalStatus)Enum.Parse(typeof(ApprovalStatus), organizationRequestModel.status, true)
                                    : ApprovalStatus.Pending;

                if (existingOrg != null)
                {
                    existingOrg.OrganizationName = organizationRequestModel.OrganizationName;
                    existingOrg.Address = organizationRequestModel.Address;
                    existingOrg.PhoneNumber = organizationRequestModel.PhoneNumber;
                    existingOrg.EmailID = organizationRequestModel.EmailID;
                    existingOrg.UpdatedDate = DateTime.UtcNow;
                    existingOrg.AdminUserName = organizationRequestModel.AdminUserName;
                    existingOrg.NoUsers = organizationRequestModel.NoUsers;
                    existingOrg.UrlName = organizationRequestModel.OrganizationName;
                    existingOrg.CountryCodeId = organizationRequestModel.CountryCodeId;
                    existingOrg.Status = approvalStatus.ToString();

                    _context.organizations.Update(existingOrg);
                    await _context.SaveChangesAsync();
                    return existingOrg.OrganizationID;
                }
                else
                {
                    var newOrg = new Organizations
                    {
                        OrganizationName = organizationRequestModel.OrganizationName,
                        Address = organizationRequestModel.Address,
                        PhoneNumber = organizationRequestModel.PhoneNumber,
                        OrgKey = await GenerateUniqueOrganizationIDAsync(),
                        EmailID = organizationRequestModel.EmailID,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow,
                        isActive = false,
                        Status = ApprovalStatus.Pending.ToString(),
                        CreatedBy = "System",
                        UpdatedBy = "System",
                        StatusTypeID = 1,
                        NoUsers = organizationRequestModel.NoUsers,
                        RegistrationDate = DateTime.UtcNow,
                        UrlName = organizationRequestModel.OrganizationName,
                        SecondaryTests = false,
                        AdminUserName = organizationRequestModel.AdminUserName,
                        SetupStatus = 0,
                        DashBoardTemplate = 0,
                        CountryCodeId = organizationRequestModel.CountryCodeId,
                        OrgTypeId = organizationRequestModel.OrgTypeId,
                    };
                    await _context.organizations.AddAsync(newOrg);
                    await _context.SaveChangesAsync();
                    return newOrg.OrganizationID;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public async Task<AllOrgsInfo> OrganizationInfo()
        {
            try
            {
                var getOrginationsData = await _context.organizations.ToListAsync();
                var activeOrgs = getOrginationsData.Count(s => s.Status == ApprovalStatus.Active.ToString());
                var pendingOrgs = getOrginationsData.Count(s => s.Status == ApprovalStatus.Pending.ToString());
                var totalorgs = getOrginationsData.Count();
                var totalEmployees = getOrginationsData.Sum(s => s.NoUsers);
                var inaactiveOrgs = getOrginationsData.Count(s => s.Status == ApprovalStatus.InActive.ToString());

                var getIndustryData = await (from orgs in _context.organizations
                                            join ind in _context.OrganizationTypes on orgs.OrgTypeId equals ind.Id
                                            group orgs by ind.TypeName into industryGroup
                                            select new OrganizationTypesCount
                                            {
                                               OrganizationCount  = industryGroup.Count(),
                                                OrganizationType = industryGroup.Key
                                            }).ToListAsync();

                var OrganizationsCount =  new OrganizationsCount
                {
                    ActiveOrganizations = activeOrgs,
                    PendingApproval = pendingOrgs,
                    TotalOrganizations = totalorgs,
                    TotalEmployees = totalEmployees,
                    InActive = inaactiveOrgs
                };
                var allOrgsInfo = new AllOrgsInfo
                {
                    organizationsCount = OrganizationsCount,
                    OrganizationTypes = getIndustryData
                };

                return allOrgsInfo;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<List<OrganizationTypesResponse>> OrganizationTypes()
        {
            try
            {
                return await _context.OrganizationTypes.Where(org => org.Status).Select(org => new OrganizationTypesResponse
                {
                    OrganizationTypeId = org.Id,
                    OrganizationTypeName = org.TypeName
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Response<object>> GetOrgnizations(int pageNumber, int pageSize , int industryId ,string status,string searchTerm)
        {
            try
            {
                Response<object> response = new Response<object>();
                ApprovalStatus approvalStatus = status != null
                    ? (ApprovalStatus)Enum.Parse(typeof(ApprovalStatus), status, true)
                    : ApprovalStatus.Active;

                var query = from orgs in _context.organizations
                            join hcmp in _context.HealthCampRequest
                                on orgs.OrganizationID equals hcmp.OrgId into hcmpGroup
                            from hcmp in hcmpGroup.DefaultIfEmpty()
                            join hserreq in _context.RequestedService
                                on hcmp.Id equals hserreq.HealthCampRequestId into hserreqGroup
                            from hserreq in hserreqGroup.DefaultIfEmpty()
                            join hser in _context.HealthService
                                on hserreq.ServiceId equals hser.ServiceID into hserGroup
                            from hser in hserGroup.DefaultIfEmpty()
                            join ind in _context.OrganizationTypes
                                on orgs.OrgTypeId equals ind.Id into indGroup
                            from ind in indGroup.DefaultIfEmpty()
                            where (industryId == 0 || (ind != null && ind.Id == industryId)) &&
                                  (status == null || (orgs.Status == approvalStatus.ToString()))
                                  && (string.IsNullOrEmpty(searchTerm) ||orgs.OrganizationName.Contains(searchTerm))
                            group new { orgs, hcmp, hserreq, hser, ind } by new
                            {
                                orgs.OrganizationID,
                                orgs.OrganizationName,
                                orgs.AdminUserName,
                                orgs.isActive,
                                orgs.PhoneNumber,
                                orgs.EmailID,
                                orgs.NoUsers,
                                orgs.CreatedDate,
                                orgs.Status
                            } into orgGroup
                            select new OrganizationResponse
                            {
                                OrganizationID = orgGroup.Key.OrganizationID,
                                Status = orgGroup.Key.Status,
                                OrganizationName = orgGroup.Key.OrganizationName,
                                AdminUserName = orgGroup.Key.AdminUserName,
                                Industry = orgGroup.FirstOrDefault().ind.TypeName,
                                isActive = orgGroup.Key.isActive,
                                CampsCmplted = orgGroup
                                            .Where(x => x.hcmp != null && x.hcmp.ApprovalStatus == ApprovalStatus.Completed.ToString())
                                            .Select(x => x.hcmp.Id)
                                            .Distinct()
                                            .Count(),
                                HealthScore = 0,
                                PhoneNumber = orgGroup.Key.PhoneNumber,
                                EmailID = orgGroup.Key.EmailID,
                                Employees = orgGroup.Key.NoUsers,
                                CreatedDate = orgGroup.Key.CreatedDate,
                                RegistrationDate = orgGroup.Key.CreatedDate,
                            };

                int totalCount = await query.CountAsync();
                int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                var pagedData = await query
                    .OrderByDescending(o => o.CreatedDate) 
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                response.data = new
                {
                    totalPages,
                    totalCount,
                    currentPage = pageNumber,
                    pageSize,
                    results = pagedData
                };

                return response;
            }
            catch (Exception ex)
            {
                return new Response<object>()
                {
                    status = false
                };
            }
        }

        public async Task<bool> UpdateAspnetUserID(long organizationID, string aspnetUserID)
        {
            try
            {
                var organization = _context.organizations.Find(Convert.ToInt32(organizationID));
                organization.AspnetUserID = aspnetUserID;
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                return false;
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
                return false;
            }
        }


        public string GenerateRandomOrganizationID()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                return new string(Enumerable.Repeat(chars, 8)
                    .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        public async Task<string> GenerateUniqueOrganizationIDAsync()
        {
            string code;
            bool exists;
            do
            {
                code = GenerateRandomOrganizationID();
                exists = await _context.organizations.AnyAsync(o => o.OrgKey == code);
            }
            while (exists);

            return code;
        }


    }
}
