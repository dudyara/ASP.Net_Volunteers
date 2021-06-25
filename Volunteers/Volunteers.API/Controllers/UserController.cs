namespace Volunteers.API.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Volunteers.Services.Dto;
    using Volunteers.Services.Services;

    /// <summary>
    /// Authorization controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="logger">logger</param>
        public UserController(
            ILogger<UserController> logger)
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
        [HttpPost("register")]
        public async Task<ActionResult<string>> RegisterUser(
            RegistrationDto dto,
            [FromQuery] string token,
            [FromQuery] long? orgId,
            [FromServices] UserService authorizationService)
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

            _logger.LogInformation("Зарегистрирован пользователь " + dto.Email + " " + DateTime.UtcNow.ToLongTimeString());
            return BadRequest();
        }

        /// <summary>
        /// GetToken
        /// </summary>
        /// <param name="service">service</param>
        /// <param name="id">orgId</param>
        [HttpGet]
        public async Task<ActionResult<string>> GetToken(
            [FromServices] UserService service,
            long? id)
        {
            var link = await service.GenerateLink(id);
            _logger.LogInformation("Выдан токен " + DateTime.UtcNow.ToLongTimeString());
            return link;
        }

        /// <summary>
        /// Authenticate
        /// </summary>
        /// <param name="loginDto">loginDto</param>
        /// <param name="authenticationService">authenticationService</param>
        [AllowAnonymous]
        [HttpPost("auth")]
        public async Task<IActionResult> AuthenticateAsync(
            [FromBody] LoginDto loginDto,
            [FromServices] UserService authenticationService)
        {
            var token = await authenticationService.AuthenticateAsync(loginDto.Email, loginDto.Password); 

            if (token == null)
                return Unauthorized();

            _logger.LogInformation("Авторизован пользователь " + loginDto.Email + " " + DateTime.UtcNow.ToLongTimeString());
            return Ok(token);
        }
    }
}