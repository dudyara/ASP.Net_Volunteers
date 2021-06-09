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
        /// <param name="orgDto">organizationDto</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IdentityResult>> Register(
            OrganizationCreateDto orgDto,
            [FromServices] AuthorizationService authorizationService,
            [FromServices] OrganizationService organizationService,
            [FromQuery] string token)
        {
            // Пока задается вручную
            var idUser = 2154;
            var result = await authorizationService.AddUser(idUser, orgDto, token, organizationService);
            if (result.Succeeded)
            {
                // Надо разобраться с id
                organizationService.Create(orgDto, idUser);
            }

            return result;
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
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate([FromBody] LoginDto loginDto, [FromServices] JWTAuthenticationManager authenticationManager)
        {
            var token = authenticationManager.AuthenticateAsync(loginDto.Email, loginDto.Password);

            if (token == null)
                return Unauthorized();

            return Ok(token);
        }
    }
}
