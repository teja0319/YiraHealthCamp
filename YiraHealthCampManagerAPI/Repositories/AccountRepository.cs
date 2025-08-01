using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YiraApi.Common.CustomException;
using YiraApi.Models.Authentication;
using YiraHealthCampManagerAPI.Context;
using YiraHealthCampManagerAPI.Interfaces.RepositoryInterfaces;
using YiraHealthCampManagerAPI.Models.Account;
using YiraHealthCampManagerAPI.Models.Common;

namespace YiraHealthCampManagerAPI.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        string repoName = "Account Repository";
        private readonly YiraDbContext _context;
        private readonly RoleManager<Microsoft.AspNetCore.Identity.IdentityRole> _roleManager;

        public AccountRepository(YiraDbContext context, RoleManager<Microsoft.AspNetCore.Identity.IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<GetLogomainRes> GetLogo(int orgid)
        {
            try
            {
                var sata = _context.GetLogoRes.FromSqlRaw($"Usp_GetOrgSpecificContentNew @Orgid = {orgid}");
                if (sata != null)
                {
                    return sata.AsEnumerable().FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new RepoException(repoName, ex);
            }
        }

        public long SaveNewUserToOrgUsers(int organizationId, string userId, string UserType)
        {
            try
            {
                bool saveSuccess = false;

                OrgUsers orgUsers;
                orgUsers = new OrgUsers
                {
                    UserId = userId,
                    OrganizationID = organizationId,
                    RoleId = UserType == "Doctor" ? ServiceConstants.YiraDoctorRoleId : ServiceConstants.YiraUserRoleId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };
                _context.OrgUsers.Add(orgUsers);
                _context.SaveChanges();
                if (orgUsers.ProfileID != 0)
                {
                    saveSuccess = true;
                }

                return saveSuccess ? orgUsers.ProfileID : 0;
            }
            catch (Exception ex)
            {
                throw new RepoException(repoName, ex);
            }
        }
        public async Task<UserData> GetUserData(string userId)
        {
            try
            {
                var sata = _context.UserData.FromSqlRaw($"usp_GetUserData @UserId = '{userId}'");
                if (sata is IQueryable<UserData>)
                {
                    return sata.AsEnumerable().FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new RepoException(repoName, ex);
            }
        }
        public async Task<ApplicationUser> FindByPhoneAsync(string phoneNumber)
        {
            try
            {
                var user = await _context.ApplicationUsers.Where(u => u.CountryCode + u.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw new RepoException(repoName, ex);
            }

        }



        public async Task<ApplicationUser> UpdateUserLoginDetails(string userId)
        {
            try
            {
                var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null)
                {
                    user.UserName = user.UniqueUserName;
                    user.NormalizedUserName = user.UniqueUserName;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new RepoException(repoName, ex);
            }
        }

        public async Task<bool> UpdateRefreshToken(ApplicationUser user)
        {
            try
            {
                var result = _context.ApplicationUsers.Where(x => x.Id == user.Id).FirstOrDefault();
                if (result != null)
                {
                    result.RefreshToken = user.RefreshToken;
                    result.LastLoginTime = DateTime.UtcNow;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                throw new RepoException(repoName, ex);
            }
        }

        public async Task<UserData> UpdateUserAccessToken(string userId, string token)
        {
            try
            {
                var updatetoken = _context.UserData.FromSqlRaw($"UpdateUserAccessToken @userId = '{userId}',@token='{token}'");
                if (updatetoken is IQueryable<UserData>)
                {
                    return updatetoken.AsEnumerable().FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new RepoException(repoName, ex);
            }
        }


        public bool UpdateOrgUsersGroupsData(long profileId)
        {
            try
            {
                var data = _context.Database.ExecuteSqlRawAsync(
                    $"INSERT INTO OrgUsersGroupsData (OrgUsersId, DID) VALUES ({profileId}, 0)");
                return data.Result > 0;
            }
            catch (Exception ex)
            {
                throw new RepoException(repoName, ex);
            }
        }

    }
}
