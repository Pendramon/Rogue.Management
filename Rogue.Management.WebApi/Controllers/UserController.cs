using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rogue.Management.Service.Interfaces;
using Rogue.Management.View.Model;

namespace Rogue.Management.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var errors = new Dictionary<string, object>();
                foreach (var keyModelStatePair in this.ModelState)
                {
                    if (keyModelStatePair.Value.Errors.Count > 0)
                    {
                        errors[keyModelStatePair.Key] = keyModelStatePair.Value.Errors.Select(error => error.ErrorMessage);
                    }
                }

                return this.BadRequest(errors);
            }

            var result = await this.userService.RegisterAsync(model);

            if (result.Errors.Any())
            {
                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.IsNullOrWhiteSpace(error.Field) ? string.Empty : error.Field, error.Message);
                }

                return this.Conflict(this.ModelState);
            }

            return result.Succeeded ? this.Ok(result.Result) : this.StatusCode(500);
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var result = await this.userService.LoginAsync(model);

            if (result.Errors.Any())
            {
                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.IsNullOrWhiteSpace(error.Field) ? string.Empty : error.Field, error.Message);
                }

                return this.Unauthorized(this.ModelState);
            }

            return result.Succeeded ? this.Ok(result.Result) : this.StatusCode(500);
        }
    }
}