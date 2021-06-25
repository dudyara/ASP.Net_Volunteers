namespace Volunteers.Services.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using FluentValidation;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Volunteers.DB;
    using Volunteers.Entities;
    using Volunteers.Entities.Enums;
    using Volunteers.Entities.Models;
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
        /// GetPull.
        /// </summary>
        /// <param name="status">status</param>
        /// <param name="orgId">id</param>
        /// <param name="requestDto">requestDto</param>
        public async Task<ActionResult<TotalRequestDto>> Get(RequestStatus status, long orgId, RequestGetWithFiltersDto requestDto)
        {
            var totalRequestDto = new TotalRequestDto();
            var filter_result = Enumerable.Empty<Request>().AsQueryable();

            // Фильтруем по статусу
            if (status.Equals(RequestStatus.Waiting))
            {
                filter_result = Repository.Get();
            }
            else if (status.Equals(RequestStatus.Execution))
            {
                if (requestDto.StartDate != null)
                {
                    filter_result = Repository.Get().Where(n => n.Created >= requestDto.StartDate && n.Created <= requestDto.FinalDate).Where(s => s.RequestStatus == status);
                }
                else
                {
                    filter_result = Repository.Get().Where(n => n.Created <= requestDto.FinalDate).Where(s => s.RequestStatus == status);
                }
            }
            else if (status.Equals(RequestStatus.Done))
            {
                if (requestDto.StartDate != null)
                {
                    filter_result = Repository.Get().Where(n => n.Deleted >= requestDto.FinalDate && n.Deleted <= requestDto.FinalDate).Where(s => s.RequestStatus == status);
                }
                else
                {
                    filter_result = Repository.Get().Where(n => n.Deleted <= requestDto.FinalDate).Where(s => s.RequestStatus == status);
                }
            }

            // фильтрация по организации
            if (orgId != 0)
            {
                filter_result = filter_result.Where(p => p.OrganizationId == orgId);
            }

            // пагинация
            var result = PagedList<Request>.ToPagedList(filter_result, requestDto.PageNumber, requestDto.PageSize);
            totalRequestDto.Count = filter_result.Count();
            totalRequestDto.FinalDate = requestDto.FinalDate;
            totalRequestDto.StartDate = requestDto.StartDate;
            totalRequestDto.HasNext = result.HasNext;
            totalRequestDto.HasPrevious = result.HasPrevious;

            totalRequestDto.RequestDtos = await filter_result.ProjectTo<RequestDto>(Mapper.ConfigurationProvider).ToListAsync();

            return totalRequestDto;
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
