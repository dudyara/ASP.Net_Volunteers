namespace Volunteers.API.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
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
        /// <param name="dto">dto</param>
        /// <param name="authorizationService">authorizationService</param>
        /// <param name="organizationService">organizationService</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        public async Task<ActionResult<string>> RegisterUser(
            RegistrationDto dto,
            [FromServices] AuthorizationService authorizationService,
            [FromServices] OrganizationService organizationService)
        {
            var result = await authorizationService.AddUser(dto, organizationService);
            if (result != 0)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// RegisterOrganization
        /// </summary>
        /// <param name="organizationDto">Dto</param>
        /// <param name="organizationService">Service</param>
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
        [HttpGet]
        public ActionResult<string> GetToken([FromServices] AuthorizationService service, long id)
        {
            return service.GenerateLink(id);
        }

        /// <summary>
        /// Authenticate
        /// </summary>
        /// <param name="loginDto">loginDto</param>
        /// <param name="authenticationService">authenticationService</param>
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
