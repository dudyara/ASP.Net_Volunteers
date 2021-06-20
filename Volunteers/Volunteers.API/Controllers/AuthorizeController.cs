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
        /// <param name="token">token</param>
        /// <param name="orgId">orgId</param>
        /// <param name="authorizationService">authorizationService</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>  
        [HttpPost]
        public async Task<ActionResult<string>> RegisterUser(
            RegistrationDto dto,
            [FromQuery] string token,
            [FromQuery] long? orgId,
            [FromServices] AuthorizationService authorizationService)
        {
            if (!await authorizationService.CheckRegistrationToken(token))
            {
                return BadRequest();
            }
            
            if (orgId.HasValue)
            {
                return Ok(await authorizationService.AddUserAsync(dto, orgId));
            }

            var result = await authorizationService.AddUserAsync(dto);

            if (result == dto)
            {
                return Ok(result);
            }

            return BadRequest();
        }

        /// <summary>
        /// RegisterOrganization
        /// </summary>
        /// <param name="organizationDto">Dto</param>
        /// <param name="organizationService">Service</param>
        /// <param name="userId">orgId</param>
        [Authorize(Roles = "Admin, Organization")] 
        [HttpPost]
        public async Task<ActionResult<OrganizationDto>> RegisterOrganization(
            OrganizationDto organizationDto,
            [FromServices] OrganizationService organizationService,
            [FromQuery] long? userId)
        {
            if (userId.HasValue)
            {
                return await organizationService.Create(organizationDto, (long)userId);
            }

            return await organizationService.Create(organizationDto);
        }

        /// <summary>
        /// GetToken
        /// </summary>
        /// <param name="service">service</param>
        /// <param name="id">orgId</param>
        [HttpGet]
        public async Task<ActionResult<string>> GetToken(
            [FromServices] AuthorizationService service,
            long? id)
        {
            var link = await service.GenerateLink(id);
            return link;
        }

        /// <summary>
        /// Authenticate
        /// </summary>
        /// <param name="loginDto">loginDto</param>
        /// <param name="authenticationService">authenticationService</param>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AuthenticateAsync(
            [FromBody] LoginDto loginDto,
            [FromServices] AuthorizationService authenticationService)
        {
            var token = await authenticationService.AuthenticateAsync(loginDto.Email, loginDto.Password); 

            if (token == null)
                return Unauthorized();

            return Ok(token);
        }
    }
}