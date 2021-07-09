namespace Volunteers.API.Controllers
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Volunteers.Entities.Enums;
    using Volunteers.Services.Dto;
    using Volunteers.Services.Services;

    /// <summary>
    /// Authorization controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly string _newPath2;
        private readonly ILogger<UserController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="hostEnvironment">hostEnviroment</param>
        public UserController(
            ILogger<UserController> logger, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
            _newPath2 = Path.GetFullPath(Path.Combine(_hostEnvironment.ContentRootPath, @"..\"));
        }

        /// <summary>
        /// Создание нового пользователя
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
            if (!await authorizationService.CheckRegistrationToken(token, orgId))
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
        /// Получение токена
        /// </summary>
        /// <param name="service">service</param>
        /// <param name="id">orgId</param>
        [HttpGet("token")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<string>> GetToken(
            [FromServices] UserService service,
            long? id)
        {
            var link = await service.GenerateLink(id);
            _logger.LogInformation("Выдан токен " + DateTime.UtcNow.ToLongTimeString());
            return link;
        }

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="loginDto">loginDto</param>
        /// <param name="authenticationService">authenticationService</param>
        [AllowAnonymous]
        [HttpPost("auth")]
        public async Task<IActionResult> AuthenticateAsync(
            [FromBody] LoginDto loginDto,
            [FromServices] UserService authenticationService)
        {
            AuthenticateDto token;
            token = await authenticationService.AuthenticateAsync(loginDto.Email, loginDto.Password);

            if (token == null)
                return Unauthorized();

            _logger.LogInformation("Авторизован пользователь " + loginDto.Email + " " + DateTime.UtcNow.ToLongTimeString());
            return Ok(token);
        }

        /// <summary>
        /// Изменение пароля
        /// </summary>
        /// <param name="dto">dto</param>
        /// <param name="service">service</param>
        [Authorize]
        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangePassword(
        [FromBody] PasswordDto dto,
        [FromServices] UserService service)
        {
            var result = await service.ChangePassword(dto);
            if (result.Succeeded == false)
                return BadRequest("Неверный пароль");
            _logger.LogInformation("Изменен пароль пользователя " + dto.UserId + " " + DateTime.UtcNow.ToLongTimeString());
            return Ok();
        }

        /// <summary>
        /// GetPolycy
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPolicy")]
        public FileResult GetPolicy()
        {
            var filePath = _newPath2 + @"Volunteers.DB\Policy_service.pdf";
            return PhysicalFile(filePath, "application/pdf", "Policy_service.pdf");
        }

        /// <summary>
        /// GetAgreement
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAgreement")]
        public FileResult GetAgreement()
        {
            var filePath = _newPath2 + @"Volunteers.DB\Agreement_service.pdf";
            return PhysicalFile(filePath, "application/pdf", "Agreement_service.pdf");
        }
    }
}