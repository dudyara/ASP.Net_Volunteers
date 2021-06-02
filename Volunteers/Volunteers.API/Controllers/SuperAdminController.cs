namespace Volunteers.API.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Volunteers.Entities.Enums;
    using Volunteers.Services.Dto;
    using Volunteers.Services.Services;

    /// <summary>
    /// SuperAdminController
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SuperAdminController : Controller
    {
        /// <summary>
        /// Получить пулл заявок по введенному статусу и id организации.
        /// </summary>
        /// <param name="status">status</param>
        /// <param name="id">id</param>
        /// <param name="service">Сервис.</param>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestDto>>> Get(
            [FromQuery] RequestStatus status,
            [FromQuery] long id,
            [FromServices] SuperAdminService service)
        {
            var result = await service.Get(status, id);
            return result ?? NotFound();
        }
    }
}
