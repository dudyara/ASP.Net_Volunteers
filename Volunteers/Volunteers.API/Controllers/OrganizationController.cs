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
            return result ?? NotFound();
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="service">service</param>
        /// <param name="orgDto">orgDto</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<Organization>> Update(
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
        /// <returns></returns>
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
