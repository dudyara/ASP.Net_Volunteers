namespace Volunteers.API.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Volunteers.Entities;
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
        /// GetRequests.
        /// </summary>
        /// <param name="service">Сервис.</param>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestDto>>> Get(
            [FromServices] RequestService service)
        {
            var result = await service.Get();
            if (result == null)
                return NotFound();
            return result;
        }

        /// <summary>
        /// GetPull.
        /// </summary>
        /// <param name="service">Сервис.</param>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestDto>>> GetPull(
            [FromServices] RequestService service)
        {
            var result = await service.GetPull();
            if (result == null)
                return NotFound();
            return result;
        }

        /// <summary>
        /// GetRequests/id.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="service">Сервис.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestDto>> GetById(
            long id, [FromServices] RequestService service)
        {
            var result = await service.GetByID(id);
            if (result == null)
                return NotFound();
            return result;
        }

        /// <summary>
        /// PostRequest.
        /// </summary>
        /// <param name="request">request.</param>
        /// <param name="service">service.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Request>> Create(
            CreateRequestDto request, [FromServices] RequestService service)
        {
            var result = await service.Create(request);

            if (result == null)
            {
                BadRequest("Result is null");
            }

            return Ok(result);
        }
    }
}
