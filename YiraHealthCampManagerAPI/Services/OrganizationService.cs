using YiraApi.Common.CustomException;
using YiraHealthCampManagerAPI.Interfaces.RepositoryInterfaces;
using YiraHealthCampManagerAPI.Interfaces.ServiceInterfaces;
using YiraHealthCampManagerAPI.Models.Account;
using YiraHealthCampManagerAPI.Models.Common;
using YiraHealthCampManagerAPI.Models.Organization;
using YiraHealthCampManagerAPI.Models.Request;
using YiraHealthCampManagerAPI.Repositories;

namespace YiraHealthCampManagerAPI.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IAccountService _accountService;
        private readonly IAccountRepository _accountRepository;
        public OrganizationService(IOrganizationRepository organizationRepository, IAccountService accountService, IAccountRepository accountRepository)
        {
            _organizationRepository = organizationRepository;
            _accountService = accountService;
            _accountRepository = accountRepository;
        }


        public async Task<Response<object>> AddUpdateOrganizationDetails(OrganizationRequestModel organizations)
        {
            var resp = new Response<object>
            {
                status = false,
                message = "Something went wrong. Please try again",
                data = null
            };

            if (!organizations.IsUpdate)
            {
                var CheckOrgNameExists = await _organizationRepository.IsOrganizationNameExists(organizations.OrganizationName);

                if (CheckOrgNameExists)
                {
                    resp.status = false;
                    resp.message = "Organization name already exists.";
                    resp.data = null;
                    return resp;
                }
            }

            try
            {
                var orgResult = await _organizationRepository.AddUpdateOrganizationDetails(organizations);

                if (orgResult > 0)
                {
                    if (organizations.OrganizationID == 0)
                    {
                        string pwd = "Admin.123";

                        RegisterUserModel userModel = new RegisterUserModel
                        {
                            Email = organizations.EmailID,
                            ConfirmPassword = pwd,
                            Password = pwd,
                            DateOfBirth = DateTime.Now,
                            Gender = "Male",
                            Name = organizations.AdminUserName,
                            PhoneNumber = organizations.PhoneNumber,
                            OrganizationId = 0,
                            CountryCode = "+91"
                        };

                        var applicationUser = await _accountService.CreateAspNetUserAsync(userModel, orgResult, "Admin");

                        if (applicationUser != null)
                        {
                            var orgAspIdUpdate = await _organizationRepository.UpdateAspnetUserID(orgResult, applicationUser.Id);

                            resp = new Response<object>
                            {
                                status = true,
                                message = "Organization and admin user created successfully",
                                data = new
                                {
                                    ID = orgResult,
                                    AdminUserID = applicationUser.Id,
                                    Status = true
                                }
                            };
                        }
                        else
                        {
                            resp = new Response<object>
                            {
                                status = false,
                                message = "Organization created, but admin user creation failed",
                                data = new
                                {
                                    ID = orgResult,
                                    AdminUserID = (string)null,
                                    Status = false
                                }
                            };
                        }
                    }
                    else 
                    {
                        resp = new Response<object>
                        {
                            status = true,
                            message = "Organization updated successfully",
                            data = new
                            {
                                ID = orgResult,
                                Status = true
                            }
                        };
                    }
                }
                else
                {
                    resp = new Response<object>
                    {
                        status = false,
                        message = "Failed to add or update organization",
                        data = null
                    };
                }
            }
            catch (Exception ex)
            {
                throw new ServiceException("AddOrganization", ex);
            }

            return resp;
        }


        public async Task<Response<object>> GetOrgTypes()
        {
            try
            {
                Response<object> response = new Response<object>();
                var orgTypes = await _organizationRepository.OrganizationTypes();
                if (orgTypes != null && orgTypes.Count > 0)
                {
                    response.status = true;
                    response.data = orgTypes;
                    response.message = "Organization types fetched successfully.";
                }
                else
                {
                    response.status = false;
                    response.data = null;
                    response.message = "No organization types found.";
                }
                return response;

            }
            catch (Exception ex)
            {
                return new Response<object>
                {
                    status = false,
                    message = "An error occurred while fetching organization types.",
                    data = null
                };

            }
        }

        public async Task<Response<object>> GetOrganizationsInfo()
        {
            try
            {
                Response<object> response = new Response<object>();
                var industries = await _organizationRepository.OrganizationInfo();
                if (industries != null)
                {
                    response.status = true;
                    response.data = industries;
                    response.message = "Industries fetched successfully.";
                }
                else
                {
                    response.status = false;
                    response.data = null;
                    response.message = "No industries found.";
                }
                return response;
            }
            catch (Exception ex)
            {
                return new Response<object>
                {
                    status = false,
                    message = "An error occurred while fetching industries.",
                    data = null
                };
            }
        }

        public async Task<Response<object>> GetOrganizations(int pageNumber, int pageSize,int industryId , string status , string search)
        {
            try
            {
                Response<object> response = new Response<object>();
                var organizations = await _organizationRepository.GetOrgnizations(pageNumber, pageSize , industryId , status , search);
                if (organizations != null && organizations.data != null)
                {
                    response.status = true;
                    response.data = organizations.data;
                    response.message = "Organizations fetched successfully.";
                }
                else
                {
                    response.status = false;
                    response.data = null;
                    response.message = "No organizations found.";
                }
                return response;
            }
            catch (Exception ex)
            {
                return new Response<object>
                {
                    status = false,
                    message = "An error occurred while fetching organizations.",
                    data = null
                };
            }
        }



    }

}
