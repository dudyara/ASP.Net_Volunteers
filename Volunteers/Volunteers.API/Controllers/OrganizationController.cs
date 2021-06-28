
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
        /// Получить список организаций.
        /// </summary>
        /// <param name="service">Сервис.</param>
        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        public async Task<ActionResult<List<OrganizationDto>>> Get(
            [FromServices] OrganizationService service)
        {
            var result = await service.Get();
            return result ?? NotFound();
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
            return result ?? NotFound();
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
