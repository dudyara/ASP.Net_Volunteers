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
        public async Task<ActionResult<string>> RegisteUser(
            RegistrationDto dto,
            [FromServices] AuthorizationService authorizationService,
            [FromServices] OrganizationService organizationService)
        {
            var result = await authorizationService.AddUser(dto, organizationService);
            if (result != 0)
            {
                return "OK";
            }

            return "Bad Request";
        }

        /// <summary>
        /// RegistraterOrganization
        /// </summary>
        /// <param name="organizationDto">organizationDto</param>
        /// <param name="organizationService">organizationService</param>
        /// <returns></returns>

        [HttpPost]

        public async Task<ActionResult<OrganizationDto>> RegisterOrganization(
            OrganizationDto organizationDto,
            [FromServices] OrganizationService organizationService)
        {
            var dto = await organizationService.Create(organizationDto);

            return dto;
        } 

        /// <summary>
        /// GetToken
        /// </summary>
        /// <param name="service">service</param>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet]

        public ActionResult<string> GetToken([FromServices] AuthorizationService service, long id)
        {
            return service.GenerateLink(id);
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
