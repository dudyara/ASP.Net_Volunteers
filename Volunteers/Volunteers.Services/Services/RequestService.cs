namespace Volunteers.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using FluentValidation;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Volunteers.DB;
    using Volunteers.Entities;
    using Volunteers.Entities.Enums;
    using Volunteers.Services.Dto;
    using Volunteers.Services.Mapper;

    /// <summary>
    /// RequestService
    /// </summary>
    public class RequestService : BaseService<Request, RequestDto>
    {

        /// <summary>
        /// Метод заявок.
        /// </summary>
        /// <param name="mapper">Маппер.</param>
        /// <param name="repository">repository</param>
        /// <param name="validator">validator</param>
        public RequestService(IVolunteerMapper mapper, IDbRepository<Request> repository, IDtoValidator validator)
            : base(mapper, repository, validator)
        {
        }

        /// <summary>
        /// Возвращает список 
        /// </summary>
        /// <param name="filter">filter</param>
        public async Task<ResultPart<RequestDto>> Get(RequestFilterDto filter)
        {
            var result = await Repository.FromBuilder(_ => _
                .Equals(x => x.RequestStatus, filter.Status)
                .And.Conditional(filter.OrganizationId != 0)
                .Where(x => x.OrganizationId == filter.OrganizationId)
                .And.Conditional(filter.Status == RequestStatus.Execution)
                .Where(x => filter.Start <= x.Created && x.Created <= filter.End)
                .And.Conditional(filter.Status == RequestStatus.Done)
                .Where(x => filter.Start <= x.Completed && x.Completed <= filter.End))
                .OrderBy(x => x.Created)
                .GetResultPartAsync<RequestDto>(Mapper, filter.Skip, filter.Limit);
               
            return result;
        }

        /// <summary>
        /// GetCount
        /// </summary>
        /// <param name="orgId">orgId.</param>
        public async Task<int[]> GetCount(long orgId)
        {
            int[] result = new int[3];
            result[0] = await Repository
                .Get()
                .Where(c => c.RequestStatus == RequestStatus.Waiting)
                .CountAsync();
            if (orgId == 0)
            {
                result[1] = await Repository
                    .Get()
                    .Where(c => c.RequestStatus == RequestStatus.Execution)
                    .CountAsync();
                result[2] = await Repository
                    .Get()
                    .Where(c => c.RequestStatus == RequestStatus.Done)
                    .CountAsync();
            }
            else
            {
                result[1] = await Repository
                    .Get()
                    .Where(c => c.OrganizationId == orgId)
                    .Where(c => c.RequestStatus == RequestStatus.Execution)
                    .CountAsync();
                result[2] = await Repository
                    .Get()
                    .Where(c => c.OrganizationId == orgId)
                    .Where(c => c.RequestStatus == RequestStatus.Done)
                    .CountAsync();
            }

            return result;
        }

        /// <summary>
        /// Изменить статус заявки
        /// </summary>
        /// <param name="reqDto">reqDto</param>
        public async Task<ActionResult<Request>> ChangeStatus(RequestChangeStatusDto reqDto)
        {
            var request = await Repository.Get(x => (x.Id == reqDto.RequestId)).FirstOrDefaultAsync();
            switch (reqDto.RequestStatus)
            {
                case RequestStatus.Waiting:
                    request.OrganizationId = null;
                    request.RequestStatus = RequestStatus.Waiting;
                    break;
                case RequestStatus.Execution:
                    if (request.RequestStatus == RequestStatus.Waiting)
                        request.OrganizationId = reqDto.OrganizationId;
                    request.RequestStatus = RequestStatus.Execution;
                    request.Completed = null;
                    break;
                case RequestStatus.Done:
                    request.RequestStatus = RequestStatus.Done;
                    request.Completed = DateTime.Now;
                    break;
            }

            await Repository.UpdateAsync(request);
            return request;
        }

        /// <summary>
        /// Написать комментарий
        /// </summary>
        /// <param name="commentDto">commentDto</param>
        public async Task<ActionResult<Request>> CreateComment(RequestCreateComment commentDto)
        {
            var request = await Repository.Get(x => (x.Id == commentDto.RequestId)).FirstOrDefaultAsync();
            request.Comment = commentDto.Comment;
            await Repository.UpdateAsync(request);
            return request;
        }

        /// <summary>
        /// Удалить заявку.
        /// </summary>
        /// <param name="id">id.</param>
        public async Task<ActionResult<Request>> Delete(long id)
        {
            var request = await Repository.Get().FirstOrDefaultAsync(x => x.Id == id);
            await DeleteAsync(id);
            return request;
        }

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="requestDto">request.</param>
        public async Task<ActionResult<Request>> Create(RequestCreateDto requestDto)
        {
            var request = Mapper.Map<Request>(requestDto);
            request.Created = DateTime.Now;
            request.RequestStatus = RequestStatus.Waiting;
            await Repository.AddAsync(request);
            return request;
        }
    }
}