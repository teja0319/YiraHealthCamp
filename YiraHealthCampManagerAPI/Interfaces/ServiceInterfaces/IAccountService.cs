using YiraHealthCampManagerAPI.Models.Account;
using YiraHealthCampManagerAPI.Models.Common;

namespace YiraHealthCampManagerAPI.Interfaces.RepositoryInterfaces
{
    public interface IAccountService
    {
        Task<Response<object>> Login(LoginModel model, string code = "");
        Task<Response<object>> RegisterUserWeb(RegisterUserModelWeb registerUserModelweb);
        Task<ApplicationUser> CreateAspNetUserAsync(RegisterUserModel model, int organizationId, string roleName = "", int Age = 0);
    }
}
