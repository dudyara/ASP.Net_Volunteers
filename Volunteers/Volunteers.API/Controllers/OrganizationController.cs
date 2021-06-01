namespace Volunteers.API.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentValidation.Results;
    using Microsoft.AspNetCore.Mvc;
    using Volunteers.Entities;
    using Volunteers.Services.Dto;
    using Volunteers.Services.Services; 

    /// <summary>
    /// OrganizationController
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrganizationController : Controller
    {
        /// <summary>
        /// Получить список организаций.
        /// </summary>
        /// <param name="service">Сервис.</param>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrganizationDto>>> Get(
            [FromServices] OrganizationService service)
        {
            var result = await service.Get();
            return result ?? NotFound();
        }

        /// <summary>
        /// Добавить новую организацию.
        /// </summary>
        /// <param name="org">organization.</param>
        /// <param name="service">service.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Organization>> Create(
            OrganizationDto org, [FromServices] OrganizationService service)
        {
            var result = await service.Create(org);
            return result ?? NotFound();
        }

        /// <summary>
        /// GetOrganization/id.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="service">Сервис.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<OrganizationDto>> GetById(
            long id, [FromServices] OrganizationService service)
        {
            var result = await service.GetById(id);
            return result ?? NotFound();
        }

        /// <summary>
        /// Получение организаций по id активностей
        /// </summary>
        /// <param name="service">service</param>
        /// <param name="ids">ids</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<OrganizationDto>>> GetByIds(
            [FromServices] OrganizationService service,
            [FromQuery] List<long> ids)
        {
            var result = await service.GetByIds(ids);
            if (result == null)
                return NotFound();
            return result;
        }
    }
}
