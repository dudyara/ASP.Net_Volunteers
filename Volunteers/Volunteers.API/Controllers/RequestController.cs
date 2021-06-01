namespace Volunteers.API.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Volunteers.Entities;
    using Volunteers.Entities.Enums;
    using Volunteers.Services.Dto;
    using Volunteers.Services.Services;

    /// <summary>
    /// RequestController - контроллер для работы с заявками.
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RequestController : Controller
    {
        /// <summary>
        /// Получить пулл заявок по введенному статусу и id организации.
        /// </summary>
        /// <param name="status">status</param>
        /// <param name="id">id</param>
        /// <param name="service">Сервис.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<RequestDto>>> Get(
            [FromQuery] RequestStatus status,
            [FromQuery] long id,
            [FromServices] RequestService service)
        {
            var result = await service.Get(status, id);
            return result ?? NotFound();
        }

        /// <summary>
        /// Изменить статус заявки.
        /// </summary>
        /// <param name="requestId">requestId.</param>
        /// <param name="status">status.</param>
        /// <param name="organizationId">organizationId.</param>
        /// <param name="service">service</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<Request>> ChangeStatus(
            [FromQuery] long requestId,
            [FromQuery] RequestStatus status,
            [FromQuery] long organizationId,
            [FromServices] RequestService service)
        {
            var result = await service.ChangeStatus(requestId, status, organizationId);
            return result ?? NotFound();
        }

        /// <summary>
        /// CreateComment.
        /// </summary>
        /// <param name="requestId">requestId.</param>
        /// <param name="comment">comment.</param>
        /// <param name="service">service</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<Request>> CreateComment(
            [FromQuery] long requestId,
            [FromBody] string comment,
            [FromServices] RequestService service)
        {
            var result = await service.CreateComment(requestId, comment);
            return result ?? NotFound();
        }

        /// <summary>
        /// CreateRequest.
        /// </summary>
        /// <param name="request">request.</param>
        /// <param name="service">service.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Request>> Create(
            [FromBody] CreateRequestDto request,
            [FromServices] RequestService service)
        {
            var result = await service.Create(request);
            return result ?? NotFound();
        }
    }
}
