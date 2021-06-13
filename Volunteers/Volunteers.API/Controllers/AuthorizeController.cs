namespace Volunteers.API.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Volunteers.Entities;
    using Volunteers.Services.Dto;
    using Volunteers.Services.Services;

    /// <summary>
    /// Authorization controller
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly ILogger<AuthorizeController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeController"/> class.
        /// </summary>
        /// <param name="logger">logger</param>
        public AuthorizeController(
            ILogger<AuthorizeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Test add new user
        /// </summary>
        /// <param name="authorizationService">authorizationService</param>
        /// <param name="organizationService">organizationService</param>
        /// <param name="dto">dto</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<string>> Register(
            RegistrationDto dto,
            [FromServices] AuthorizationService authorizationService,
            [FromServices] OrganizationService organizationService,
            [FromQuery] string token)
        {
            // Пока задается вручную
            // var idUser = 2154;
            var result = await authorizationService.AddUser(dto, token, organizationService);
            if (result != 0)
            {
                // Надо разобраться с id
                /*organizationService.Create(orgDto, result);*/
                return "OK";
            }

            return "Bad Request";
        }

        /// <summary>
        /// GetToken
        /// </summary>
        /// <param name="service">service</param>
        /// <returns></returns>
        [HttpGet]

        public ActionResult<string> GetToken([FromServices] AuthorizationService service)
        {
            return service.GetToken();
        }

        /// <summary>
        /// Authenticate
        /// </summary>
        /// <param name="loginDto">loginDto</param>
        /// <param name="jWTAuthenticationManager">jWTAuthenticationManager</param>
        /// <param name="authenticationManager">authneticationManager</param>
        /// <param name="authenticationService">authenticationService</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AuthenticateAsync([FromBody] LoginDto loginDto, [FromServices] AuthorizationService authenticationService)
        {
            var token = await authenticationService.AuthenticateAsync(loginDto.Email, loginDto.Password);

            if (token == null)
                return Unauthorized();

            return Ok(token);
        }
    }
}
