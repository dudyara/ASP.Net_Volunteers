/*namespace Volunteers.API.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Volunteers.Services.Dto;
    using Volunteers.Services.Services;

    /// <summary>
    /// Контроллер карты
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class MapController : Controller
    {
        /// <summary>
        /// Получение типов активностей компаний
        /// </summary>
        /// <param name="service">service</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<ActivityTypeDto>>> Get(
            [FromServices] MapService service)
        {
            var result = await service.GetActivityTypes();
            if (result == null)
                return NotFound();
            return result;
        }

        /// <summary>
        /// Получение организаций по id активностей
        /// </summary>
        /// <param name="service">service</param>
        /// <param name="ids">ids</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<OrganizationDto>>> GetOrganizations(
            [FromServices] MapService service,
            [FromQuery] List<long> ids)
        {
            var result = await service.GetById(ids);
            if (result == null)
                return NotFound();
            return result;
        }
    }
}*/
