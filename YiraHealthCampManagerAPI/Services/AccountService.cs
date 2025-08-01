using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using YiraApi.Common.CustomException;
using YiraHealthCampManagerAPI.Interfaces.RepositoryInterfaces;
using YiraHealthCampManagerAPI.Models.Account;
using YiraHealthCampManagerAPI.Models.Common;
using YiraHealthCampManagerAPI.Models.Common.Enum;

namespace YiraHealthCampManagerAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAccountRepository _accountRepository;
        private protected readonly JWTTokenConfigCreds _jWTTokenConfig;

        string serviceName = "AccountService";


        public AccountService(UserManager<ApplicationUser> userManager,IAccountRepository accountRepository, JWTTokenConfigCreds jWTTokenConfig)
            //IConfiguration configuration,
            //IOptions<OrganizationIdConfig> organizationIdConfig)
        {
            _userManager = userManager;
            _accountRepository = accountRepository;
            //_configuration = configuration;
            _jWTTokenConfig = jWTTokenConfig;
        }


        public async Task<Response<object>> RegisterUserWeb(RegisterUserModelWeb registerUserModelweb)
        {
            try
            {
                 Response<object> resp = new Response<object>
                 {
                    status = false,
                    message = "Something went wrong. Please try again."
                 };

                if (!string.IsNullOrEmpty(registerUserModelweb.Email))
                {
                    var existingUser = await _userManager.Users.Where(u => u.Email.ToLower() == registerUserModelweb.Email.ToLower()).ToListAsync();
                    if (existingUser.Count >= 1)
                    {
                        resp.message = $"The email address '{registerUserModelweb.Email}' is already registered.";
                        return resp;
                    }
                }
                DateTime cuurentdate = DateTime.Now;
                if (cuurentdate.ToString("yyyy/MM/dd").Equals(registerUserModelweb.DateOfBirth.ToString("yyyy/MM/dd")))
                {
                    DateTime birthDate = cuurentdate.AddYears(-registerUserModelweb.Age);
                    registerUserModelweb.DateOfBirth = Convert.ToDateTime(birthDate.ToString("yyyy-MM-dd 00:00:00"));
                }
                RegisterUserModel registerUserModel = new RegisterUserModel();
                var logo = await _accountRepository.GetLogo(registerUserModelweb.OrganizationId);

                registerUserModel.Password = registerUserModelweb.Password;
                registerUserModel.ConfirmPassword = registerUserModelweb.ConfirmPassword;
                registerUserModel.Name = registerUserModelweb.Name;
                registerUserModel.Gender = registerUserModelweb.Gender;
                registerUserModel.Email = string.IsNullOrEmpty(registerUserModelweb.Email) ? registerUserModelweb.PhoneNumber + "@yira.ai" : registerUserModelweb.Email;
                registerUserModel.DateOfBirth = registerUserModelweb.DateOfBirth;
                registerUserModel.PhoneNumber = registerUserModelweb.PhoneNumber;
                registerUserModel.CountryCode = registerUserModelweb.CountryCode;
                registerUserModel.OrganizationId = registerUserModelweb.OrganizationId;
                registerUserModel.UserType = registerUserModelweb.UserType;
                registerUserModel.Specialization = registerUserModelweb.Specialization;
                registerUserModel.FirstName = registerUserModelweb.FirstName;
                registerUserModel.LastName = registerUserModel.LastName;
                registerUserModel.BloodGroup = registerUserModelweb.BloodGroup;
                registerUserModel.Section = registerUserModelweb.Class;
                registerUserModel.Stream = registerUserModelweb.Stream;
                registerUserModel.Year = registerUserModelweb.Year;
                registerUserModel.RelationToIP = registerUserModelweb.RelationToIP;
                registerUserModel.UID = registerUserModelweb.UID;
                registerUserModel.IPNO = registerUserModelweb.IPNO;
                registerUserModel.AadharNo = registerUserModelweb.AadharNo;
                registerUserModel.EmployerName = registerUserModelweb.EmployerName;
                registerUserModel.CompanyAddress = registerUserModelweb.CompanyAddress;
                registerUserModel.HomeAddress = registerUserModelweb.HomeAddress;
                registerUserModel.AlternateNumber = registerUserModelweb.AlternateNumber;

                int organizationId = registerUserModel.OrganizationId == 0 ?
                    Convert.ToInt32(1) : registerUserModel.OrganizationId;

                var applicationUser = await CreateAspNetUserAsync(registerUserModel, organizationId, "", registerUserModelweb.Age);

                resp = await PopulateResponse(applicationUser, organizationId);

                return resp;
            }
            catch (Exception ex)
            {
                throw new ServiceException(serviceName + " Method name- RegisterUserWeb - " + JsonConvert.SerializeObject(registerUserModelweb), ex);
            }
        }

        public async Task<Response<object>> PopulateResponse(ApplicationUser applicationUser, int organizationId)
        {
            Response<object> resp;
            resp = new Response<object>
            {
                status = false,
                message = "Registration failed. Please contact yiralife"
            };
            if (applicationUser != null)
            {
                long profileId = _accountRepository.SaveNewUserToOrgUsers(organizationId, applicationUser.Id, applicationUser.UserType);
                bool status = _accountRepository.UpdateOrgUsersGroupsData(profileId);

                if (profileId > 0 && status)
                {
                    resp = new Response<object>
                    {
                        status = true,
                        message = "Registration Completed Successfully"
                    };
                }

            }
            else
            {
                resp = new Response<object>
                {
                    status = false,
                    message = "This email is already registered. Please enter a different email address."
                };
            }
            return resp;
        }
        private int AgeCaluculator(DateTime dateOfBirth)
        {
            int age = 0;
            try
            {
                age = DateTime.Now.Year - dateOfBirth.Year;
                if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                    age = age - 1;
            }
            catch (Exception)
            {
                age = 0;
            }
            return age;

        }

        public async Task<ApplicationUser> CreateAspNetUserAsync(RegisterUserModel model, int organizationId, string roleName = "", int Age = 0)
        {
            try
            {
                string email = String.Empty;
                email = model.Email;
                ApplicationUser applicationUser;
                int age = Age == 0 ? Convert.ToInt32(AgeCaluculator(model.DateOfBirth)) : Age;
                applicationUser = new ApplicationUser
                {
                    ProfileID = 0,
                    UserType = String.IsNullOrEmpty(model.UserType) ? EnumUserTypes.yirauser.ToString() : model.UserType,
                    EmployeeID = string.IsNullOrEmpty(model.EmployeeId) ? "0" : model.EmployeeId,
                    NormalizedUserName = string.IsNullOrEmpty(email) ? model.PhoneNumber + "@yira.ai" : email,
                    UserName = string.IsNullOrEmpty(email) ? model.PhoneNumber + "@yira.ai" : email,
                    Name = model.Name,
                    Gender = model.Gender == "Male" ? Convert.ToInt32(EnumGenderTypes.Male) : model.Gender == "Female" ? Convert.ToInt32(EnumGenderTypes.Female) : Convert.ToInt32(EnumGenderTypes.NG),
                    OrganizationID = organizationId,
                    Status = true,
                    DateOfBirth = model.DateOfBirth,
                    NormalizedEmail = email,
                    IsMedicalDoctor = false,
                    Email = email,
                    EmailConfirmed = true,
                    ImagePath = (model.OrganizationId == 11) ? "https://yiraapp.blob.core.windows.net/appassets/nutrifullogo.png" : "",
                    Specialization = String.IsNullOrEmpty(model.Specialization) ? "0" : model.Specialization,
                    PhoneNumber = model.PhoneNumber,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    CountryCode = model.CountryCode,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    Height = 0,
                    Weight = 0,
                    BMI = 0,
                    Age = age,
                    RefreshToken = "",
                    RefreshTokenExpiryTime = DateTime.Now,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    Pincode = model.Pincode,
                    Stream = model.Stream,
                    Section = model.Section,
                    Class = model.Year,
                    BloodGroup = model.BloodGroup,
                    RelationToIP = model.RelationToIP,
                    UID = model.UID,
                    IPNO = model.IPNO,
                    AadharNo = model.AadharNo,
                    EmployerName = model.EmployerName,
                    HealthConnectEnabled = false,
                    CompanyAddress = model.CompanyAddress,
                    HomeAddress = model.HomeAddress,
                    AlternateMobile = model.AlternateNumber,
                    TokenExpirationTime = null,

                };
                //log the request came from web app
                var result = await _userManager.CreateAsync(applicationUser, model.Password);

                //log the status of the user creation
                if (result.Succeeded)
                {
                    if (String.IsNullOrEmpty(roleName) && (applicationUser.UserType != "Doctor") && String.IsNullOrEmpty(applicationUser.UserType))
                    {
                        roleName = EnumUserTypes.yirauser.ToString();
                    }
                    else if (applicationUser.UserType == "Doctor")
                    {
                        roleName = EnumUserTypes.Doctor.ToString();
                    }
                    else
                    {
                        roleName = roleName == "Admin" ? EnumUserTypes.Admin.ToString() : EnumUserTypes.yirauser.ToString();
                    }

                    IdentityResult addResult = await _userManager.AddToRoleAsync(applicationUser, roleName);
                    //update username and uniqueusername values same as unique username
                    await _accountRepository.UpdateUserLoginDetails(applicationUser.Id);
                    return applicationUser;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new ServiceException(serviceName + "CreateAspNetUserAsync method payload" + JsonConvert.SerializeObject(model), ex);
            }
        }


        public async Task<Response<object>> Login(LoginModel model, string code = "")
        {
            try
            {
                ApplicationUser user;
                Response<object> resp;

                resp = new Response<object>
                {
                    status = false,
                    message = "Invalid username / password",
                    data = null
                };

                if (model.Username.All(char.IsDigit) && !new EmailAddressAttribute().IsValid(model.Username))
                {
                    user = await _accountRepository.FindByPhoneAsync(model.Username);
                }
                else if (new EmailAddressAttribute().IsValid(model.Username))
                {
                    user = await _userManager.FindByEmailAsync(model.Username);
                    if (user == null)
                    {
                        user = await _userManager.FindByNameAsync(model.Username);
                    }
                }
                else
                {
                    user = await _userManager.FindByNameAsync(model.Username);
                }

                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password) && user.Status != false)
                {
                    if (user.TokenExpirationTime < DateTime.UtcNow || user.TokenExpirationTime == null)
                    {
                        var refreshTokenResponse = await _accountRepository.UpdateRefreshToken(user);
                        user.RefreshToken = GenerateRefreshToken();
                        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(30);
                        user.TokenExpirationTime = DateTime.Now.AddDays(30);
                        await _userManager.UpdateAsync(user);

                        var token = await CreateOAuthToken(user);
                        resp = await UserAuthentication(model, code, resp, user, token);
                    }
                    else
                    {
                        resp = await RefreshAuthentication(resp, user);
                    }
                }
                return resp;
            }
            catch (Exception ex)
            {
                throw new ServiceException(serviceName, ex);
            }
        }


        private async Task<Response<object>> UserAuthentication(LoginModel model, string code, Response<object> resp, ApplicationUser user, JwtSecurityToken token /*= null*/)
        {
            if (user != null && user.EmailConfirmed)
            {
                //var userRoles = await _userManager.GetRolesAsync(user);
                if (code == "yiraLifeMobileLogin")
                {
                    var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
                    var resaccess = await _accountRepository.UpdateUserAccessToken(user.Id, accessToken);
                    var data = await _accountRepository.GetUserData(user.Id);
                    if (data != null && data.Data != null)
                    {
                        data.Data = data.Data.Replace("\"{", "{");
                        data.Data = data.Data.Replace("\\\"", "\"");
                        data.Data = data.Data.Replace("}\"", "}");
                        data.Data = data.Data.Replace("token", accessToken);
                        data.Data = data.Data.Replace("expired", token.ValidTo.ToString());

                        resp = new Response<object>
                        {
                            status = true,
                            message = "Successfully Logged In.",
                            data = JsonConvert.DeserializeObject(data.Data)
                        };
                    }
                }
                else
                {
                    resp = new Response<object>
                    {
                        status = true,
                        message = "Successfully Logged In."
                    };
                }
            }
            else
            {
                resp = new Response<object>
                {
                    status = false,
                    message = "Please verify your email address"
                    //TODO: Need to send back mail Id if email is not verified.
                };
            }
            return resp;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        private async Task<JwtSecurityToken> CreateOAuthToken(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        //new Claim(ClaimTypes.Email, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };
            if (!string.IsNullOrEmpty(user.Email))
            {
                authClaims.Add(new Claim(ClaimTypes.Email, user.Email));
            }
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWTTokenConfig.Secret));

            var token = new JwtSecurityToken(
                issuer: _jWTTokenConfig.Issuer,
                audience: _jWTTokenConfig.Audience,
                expires: DateTime.Now.AddDays(2),
                claims: authClaims,
                //signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha512Signature)
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha512)
                );

            return token;
        }

        private async Task<Response<object>> RefreshAuthentication(Response<object> resp, ApplicationUser user)
        {
            if (user != null && user.EmailConfirmed)
            {
                var resaccess = await _accountRepository.UpdateUserAccessToken(user.Id, user.Token.ToString());
                var data = await _accountRepository.GetUserData(user.Id);

                if (data != null && data.Data != null)
                {
                    data.Data = data.Data.Replace("\"{", "{");
                    data.Data = data.Data.Replace("\\\"", "\"");
                    data.Data = data.Data.Replace("}\"", "}");
                    data.Data = data.Data.Replace("token", user.Token.ToString());
                    data.Data = data.Data.Replace("expired", user.TokenExpirationTime.ToString());

                    resp = new Response<object>
                    {
                        status = true,
                        message = "Successfully Logged In.",
                        data = JsonConvert.DeserializeObject(data.Data)
                    };
                }
            }
            else
            {
                resp = new Response<object>
                {
                    status = false,
                    message = "Please verify your email address"
                };
            }
            return resp;
        }

    }
}
