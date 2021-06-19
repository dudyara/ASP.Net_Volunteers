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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ActivityTypeController : Controller
    {
        /// <summary>
        /// Получение типов активностей компаний
        /// </summary>
        /// <param name="actDto">actDto</param>
        /// <param name="service">service</param>
        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ActivityTypeDto>> Create(
            [FromBody] ActivityTypeDto actDto,
            [FromServices] ActivityTypeService service)
        {
            var result = await service.Create(actDto);
            return result ?? NotFound();
        }

        /// <summary>
        /// Получение типов активностей компаний
        /// </summary>
        /// <param name="service">service</param>
        [HttpGet]
        public async Task<ActionResult<List<ActivityTypeDto>>> Get(
            [FromServices] ActivityTypeService service)
        {
            var result = await service.Get();
            return result ?? NotFound();
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="actDto">actDto</param>
        /// <param name="service">service</param>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<ActivityTypeDto>> Update(
            [FromBody] ActivityTypeDto actDto,
            [FromServices] ActivityTypeService service)
        {
            var result = await service.Update(actDto);
            return result ?? NotFound();
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="service">service</param>
        [Authorize(Roles = "Admin")]
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
