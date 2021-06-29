namespace Volunteers.API.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Volunteers.Entities;
    using Volunteers.Entities.Enums;
    using Volunteers.Services.Dto;
    using Volunteers.Services.Services;

    /// <summary>
    /// ActivityType Controller контроллер
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityTypeController : Controller
    {
        /// <summary>
        /// Получение типов активностей компаний
        /// </summary>
        /// <param name="actDto">actDto</param>
        /// <param name="service">service</param>      
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<ActionResult<ActivityTypeDto>> Create(
            [FromBody] ActivityTypeDto actDto,
            [FromServices] ActivityTypeService service)
        {
            var result = await service.AddAsync(actDto);
            return result;
        }

        /// <summary>
        /// Получение типов активностей компаний
        /// </summary>
        /// <param name="service">service</param>
        /// <param name="filter">filter</param>
        [HttpGet]
        public async Task<ActionResult<List<ActivityTypeDto>>> Get(
            [FromServices] ActivityTypeService service,
            [FromQuery] ActivityTypeFilterDto filter)
        {
            var result = await service.Get(filter);
            return result;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="actDto">actDto</param>
        /// <param name="service">service</param>
        [Authorize(Roles = Roles.Admin)]
        [HttpPut]
        public async Task<ActionResult<ActivityTypeDto>> Update(
            [FromBody] ActivityTypeDto actDto,
            [FromServices] ActivityTypeService service)
        {
            var result = await service.UpdateAsync(actDto);
            return result;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="service">service</param>
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete]
        public async Task<ActionResult<ActivityType>> Delete(
            [FromQuery] long id,
            [FromServices] ActivityTypeService service)
        {
            var result = await service.Delete(id);
            return result ?? NotFound();
        }
    }
}
