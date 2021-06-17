namespace Volunteers.API.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Volunteers.Entities;
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
        [HttpGet]
        public async Task<ActionResult<List<ActivityTypeDto>>> Get(
            [FromServices] ActivityTypeService service)
        {
            var result = await service.GetAsync();
            return result;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="actDto">actDto</param>
        /// <param name="service">service</param>
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
