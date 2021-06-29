
namespace Volunteers.API.Controllers
{
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
        /// OrganizationController
        /// </summary>
        /// <param name="logger">logger</param>
        public OrganizationController(ILogger<OrganizationController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// RegisterOrganization
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

            return await organizationService.Create(organizationDto);
        }

        /// <summary>
        /// ChangeLogo
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
            return result ?? NotFound();
        }

        /// <summary>
        /// Update
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
            return result;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="service">service</param>
        /// <param name="id">id</param>
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete]
        public async Task<ActionResult<Organization>> Delete(
            [FromServices] OrganizationService service,
            [FromQuery] long id)
        {
            var result = await service.Delete(id);
            return result ?? NotFound();
        }
    }
}
