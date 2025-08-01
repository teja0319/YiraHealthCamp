using YiraApi.Models.Authentication;
using YiraHealthCampManagerAPI.Models.Account;

namespace YiraHealthCampManagerAPI.Interfaces.RepositoryInterfaces
{
    public interface IAccountRepository
    {
        Task<UserData> GetUserData(string userId);
        Task<GetLogomainRes> GetLogo(int orgid);
        Task<UserData> UpdateUserAccessToken(string userId, string token);
        bool UpdateOrgUsersGroupsData(long profileId);
        Task<ApplicationUser> UpdateUserLoginDetails(string userId);
        long SaveNewUserToOrgUsers(int organizationId, string userId, string UserType);
        Task<ApplicationUser> FindByPhoneAsync(string phoneNumber);
        Task<bool> UpdateRefreshToken(ApplicationUser user);
    }
}
