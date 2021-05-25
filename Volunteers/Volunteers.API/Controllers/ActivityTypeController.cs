namespace Volunteers.API.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
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
        /// <param name="service">service</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<ActivityTypeDto>>> Get(
            [FromServices] ActivityTypeService service)
        {
            var result = await service.GetActivityTypes();
            if (result == null)
                return NotFound();
            return result;
        }
    }
}
