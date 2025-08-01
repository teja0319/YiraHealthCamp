using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YiraHealthCampManagerAPI.Interfaces.RepositoryInterfaces;
using YiraHealthCampManagerAPI.Models.Account;

namespace YiraHealthCampManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public readonly IAccountService _accountService;
        public AccountController(IAccountService accountService) {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model, string code = "")
        {
            var a = await _accountService.Login(model, code);
            return Ok(a);
        }


        [AllowAnonymous]
        [Route("RegisterUserWeb")]
        [HttpPost]
        public async Task<IActionResult> RegisterUserWeb([FromBody] RegisterUserModelWeb registerUserModel)
        {
            return Ok(await _accountService.RegisterUserWeb(registerUserModel));
        }


    }
}
