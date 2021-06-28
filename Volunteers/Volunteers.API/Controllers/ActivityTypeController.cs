namespace Volunteers.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="logger">logger</param>
        public ActivityTypeController(ILogger<ActivityTypeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Создание типов активностей компаний
        /// </summary>
        /// <param name="actDto">actDto</param>
        /// <param name="service">service</param>      
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ActivityTypeDto>> Create(
            [FromBody] ActivityTypeDto actDto,
            [FromServices] ActivityTypeService service)
        {
            var result = await service.AddAsync(actDto);
            _logger.LogInformation("Создан тип помощи " + actDto.TypeName + " " + DateTime.UtcNow.ToLongTimeString());
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
            _logger.LogInformation("Получены типы помощи " + DateTime.UtcNow.ToLongTimeString());
            return result;
        }

        /// <summary>
        /// Изменение типов активности компании
        /// </summary>
        /// <param name="actDto">actDto</param>
        /// <param name="service">service</param>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<ActivityTypeDto>> Update(
            [FromBody] ActivityTypeDto actDto,
            [FromServices] ActivityTypeService service)
        {
            var result = await service.UpdateAsync(actDto);
            _logger.LogInformation("Обновлен тип помощи " + actDto.TypeName + " " + DateTime.UtcNow.ToLongTimeString());
            return result;
        }

        /// <summary>
        /// Удаление типа активности
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
            _logger.LogInformation("Удален тип помощи " + id + " " + DateTime.UtcNow.ToLongTimeString());
            return result ?? NotFound();
        }
    }
}
