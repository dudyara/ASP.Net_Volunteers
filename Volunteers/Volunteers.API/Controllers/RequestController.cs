﻿namespace Volunteers.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
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
        /// <summary>
        /// CreateRequest.
        /// </summary>
        /// <param name="request">request.</param>
        /// <param name="service">service.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Request>> Create( 
            [FromBody] RequestCreateDto request,
            [FromServices] RequestService service)
        {
            try
            {
                var result = await service.Create(request);
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
            return await service.Get(filter);
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
            return result;
        }

        /// <summary>
        /// Изменить статус заявки.
        /// </summary>
        /// <param name="reqDto">reqDto</param>
        /// <param name="service">service</param>
        /// <returns></returns>
        [Authorize(Roles = "Organization")]
        [HttpPut]
        public async Task<ActionResult<Request>> ChangeStatus(
            [FromBody] RequestChangeStatusDto reqDto,
            [FromServices] RequestService service)
        {
            var result = await service.ChangeStatus(reqDto);
            return result ?? NotFound();
        }

        /// <summary>
        /// CreateComment.
        /// </summary>
        /// <param name="commentDto">commentDto</param>
        /// <param name="service">service</param>
        /// <returns></returns>
        [Authorize(Roles = "Organization")]
        [HttpPut("comment")]
        public async Task<ActionResult<Request>> CreateComment(
            [FromBody] RequestCreateComment commentDto,
            [FromServices] RequestService service)
        {
            var result = await service.CreateComment(commentDto);
            return result ?? NotFound();
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="service">service</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<ActionResult<Request>> Delete(
            [FromQuery] long id,
            [FromServices] RequestService service)
        {
            var result = await service.Delete(id);
            return result ?? NotFound();
        }
    }
}
