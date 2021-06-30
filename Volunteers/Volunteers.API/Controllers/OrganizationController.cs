namespace Volunteers.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Volunteers.Entities;
    using Volunteers.Entities.Enums;
    using Volunteers.Services.Dto;
    using Volunteers.Services.Services;
     
    /// <summary>
    /// OrganizationController
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : Controller
    {
        private readonly ILogger _logger;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="logger">logger</param>
        public OrganizationController(ILogger<OrganizationController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Создание новой организации
        /// </summary>
        /// <param name="organizationDto">Dto</param>
        /// <param name="organizationService">Service</param>
        /// <param name="userId">orgId</param>
        [Authorize]
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

            _logger.LogInformation("Зарегистрирована организация " + organizationDto.Name + " " + DateTime.UtcNow.ToLongTimeString());
            return await organizationService.Create(organizationDto);
        }

        /// <summary>
        /// Изменение логотипа компании
        /// </summary>
        /// <param name="service">service</param>
        /// <param name="logoDto">orgDto</param>
        [Authorize(Roles = "Organization")]
        [HttpPut("logo")]
        public async Task<ActionResult<OrganizationDto>> ChangeLogo(
            [FromServices] OrganizationService service,
            [FromBody] OrganizationLogoDto logoDto)
        {
            var result = await service.ChangeLogo(logoDto);
            _logger.LogInformation("Изменен логотип организации " + DateTime.UtcNow.ToLongTimeString());
            return result ?? NotFound();
        }

        /// <summary>
        /// Получить список организаций.
        /// </summary>
        /// <param name="service">Сервис.</param>
        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        public async Task<ActionResult<List<OrganizationDto>>> Get(
            [FromServices] OrganizationService service)
        {
            var result = await service.GetAsync();
            _logger.LogInformation("Получен список организаций " + DateTime.UtcNow.ToLongTimeString());
            return result;
        }

        /// <summary>
        /// Получение организаций по id активностей
        /// </summary>
        /// <param name="service">service</param>
        /// <param name="ids">ids</param>
        [HttpGet("ids")]
        public async Task<ActionResult<List<OrganizationDto>>> GetByIds(
            [FromServices] OrganizationService service,
            [FromQuery] List<long> ids)
        {
            var result = await service.GetByIds(ids);
            _logger.LogInformation("Получены организации по id " + DateTime.UtcNow.ToLongTimeString());
            return result ?? NotFound();
        }

        /// <summary>
        /// Изменение организации
        /// </summary>
        /// <param name="service">service</param>
        /// <param name="orgDto">orgDto</param>
        [Authorize(Roles = Roles.Organization)]
        [HttpPut]
        public async Task<ActionResult<OrganizationDto>> Update(
            [FromServices] OrganizationService service,
            [FromBody] OrganizationDto orgDto)
        {
            var result = await service.Update(orgDto);
            _logger.LogInformation("Организация обновлена " + orgDto.Name + " " + DateTime.UtcNow.ToLongTimeString());
            return result;
        }

        /// <summary>
        /// Удаление организации
        /// </summary>
        /// <param name="service">service</param>
        /// <param name="id">id</param>
        /// <param name="requestService">requestService</param>
        /// <param name="userService">userService</param>
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete]
        public async Task<ActionResult<Organization>> Delete(
            [FromServices] OrganizationService service,
            [FromServices] RequestService requestService,
            [FromServices] UserService userService, 
            [FromQuery] long id)
        
            var result = await service.Delete(id, requestService, userService);
            _logger.LogInformation("Организация удалена " + DateTime.UtcNow.ToLongTimeString()); 
            return result ?? NotFound();
        }
    }
}
