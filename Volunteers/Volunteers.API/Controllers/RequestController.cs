namespace Volunteers.API.Controllers
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;
    using Volunteers.Entities;
    using Volunteers.Entities.Enums;
    using Volunteers.Services.Dto;
    using Volunteers.Services.Services;

    /// <summary>
    /// RequestController - контроллер для работы с заявками.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : Controller
    {
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="webHostEnvironment">webHostEnviroment</param>
        public RequestController(ILogger<RequestController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            this._webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Создание заявки.
        /// </summary>
        /// <param name="request">request.</param>
        /// <param name="service">service.</param>
        [HttpPost]
        public async Task<ActionResult<Request>> Create(
            [FromBody] RequestCreateDto request,
            [FromServices] RequestService service)
        {
            try
            {
                var result = await service.Create(request);
                _logger.LogInformation("Создана заявка пользователя " + request.Name + " " + DateTime.UtcNow.ToLongTimeString());
                return result ?? NotFound();
            }
            catch (Exception e)
            {
                return BadRequest($"{e.Message}{Environment.NewLine} {e.StackTrace}");
            }
        }

        /// <summary>
        /// Получить пулл заявок по введенному статусу и id организации.
        /// </summary>
        /// <param name="service">Сервис.</param>
        /// <param name="filter">Фильтр.</param>
        [Authorize]
        [HttpGet]
        public async Task<ResultPart<RequestDto>> Get(
            [FromServices] RequestService service,
            [FromQuery] RequestFilterDto filter)
        {
            var result = await service.Get(filter);
            _logger.LogInformation("Получен список заявок " + DateTime.UtcNow.ToLongTimeString());
            return result;
        }

        /// <summary>
        /// Получить количество заявок разных организаций
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="service">Сервис.</param>
        [Authorize]
        [HttpGet("count")]
        public async Task<int[]> GetCount(
            [FromQuery] long id,
            [FromServices] RequestService service)
        {
            var result = await service.GetCount(id);
            _logger.LogInformation("Получено количество заявок " + DateTime.UtcNow.ToLongTimeString());
            return result;
        }

        /// <summary>
        /// Изменить статус заявки.
        /// </summary>
        /// <param name="reqDto">reqDto</param>
        /// <param name="service">service</param>
        [Authorize(Roles = Roles.Organization)]
        [HttpPut]
        public async Task<ActionResult<Request>> ChangeStatus(
            [FromBody] RequestChangeStatusDto reqDto,
            [FromServices] RequestService service)
        {
            var result = await service.ChangeStatus(reqDto);
            _logger.LogInformation("Изменен статус заявки " + reqDto.RequestId + " " + DateTime.UtcNow.ToLongTimeString());
            return result ?? NotFound();
        }

        /// <summary>
        /// Создание комментария.
        /// </summary>
        /// <param name="commentDto">commentDto</param>
        /// <param name="service">service</param>
        [Authorize(Roles = Roles.Organization)]
        [HttpPut("comment")]
        public async Task<ActionResult<Request>> CreateComment(
            [FromBody] RequestCreateComment commentDto,
            [FromServices] RequestService service)
        {
            var result = await service.CreateComment(commentDto);
            _logger.LogInformation("Изменен комментарий заявки " + commentDto.RequestId + " " + DateTime.UtcNow.ToLongTimeString());
            return result ?? NotFound();
        }

        /// <summary>
        /// Удаление заявки
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="service">service</param>
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete]
        public async Task<ActionResult<Request>> Delete(
            [FromQuery] long id,
            [FromServices] RequestService service)
        {
            var result = await service.Delete(id);
            _logger.LogInformation("Удалена заявка " + id + " " + DateTime.UtcNow.ToLongTimeString());
            return result ?? NotFound();
        }

        /// <summary>
        /// Получение экселя
        /// </summary>
        /// <param name="filter">filter</param>
        /// <param name="excelMakeService">excelMakeService</param>
        /// <param name="requestService">requestService</param>
        [HttpGet("getReport")]

        public async Task<FileResult> GetFilesAsync(
            [FromQuery] RequestFilterExcelDto filter,
            [FromServices] ExcelMakeService excelMakeService,
            [FromServices] RequestService requestService)
        {
            // var filePath = contentRootPath + @"\DATAExcel.xls";
            string fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Заявки.xls";

            string path = Path.Combine(_webHostEnvironment.ContentRootPath, "DATAExcel.xls");
            var requests = (await requestService.GetFilter(filter)).Result;
            excelMakeService.Export(requests, path);
            if (filter.Start != DateTime.MinValue)
            {
                fileName = $"Заявки от {filter.Start}.xls";
            }

            return PhysicalFile(path, fileType, fileName);
        }
    }
}