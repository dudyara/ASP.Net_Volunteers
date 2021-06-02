namespace Volunteers.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
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
    public class RequestService : BaseService<Request>
    {
        /// <summary>
        /// Метод заявок.
        /// </summary>
        /// <param name="mapper">Маппер.</param>
        /// <param name="repository">repository</param>
        /// <param name="validator">validator</param>
        public RequestService(IVolunteerMapper mapper, IDbRepository<Request> repository, IValidator validator)
            : base(mapper, repository, validator)
        {
        }

        /// <summary>
        /// GetPull.
        /// </summary>
        /// <param name="status">status</param>
        /// <param name="orgId">id</param>
        public async Task<ActionResult<IEnumerable<RequestDto>>> Get(RequestStatus status, long orgId)
        {
            List<Request> requests = new List<Request>();
            if ((status == 0) && (orgId == 0))
            {
                requests = await Repository.Get().Include(u => u.Organization).ToListAsync();
            }
            else
            if (status == 0)
            {
                requests = await Repository
                    .Get()
                    .Where(r => r.OrganizationId == orgId)
                    .Include(u => u.Organization)
                    .ToListAsync();
            }
            else
            if (orgId == 0)
            {
                requests = await Repository
                    .Get()
                    .Where(r => r.RequestStatus == status)
                    .Include(u => u.Organization)
                    .ToListAsync();
            }
            else
            {
                requests = await Repository
                    .Get()
                    .Where(r => r.OrganizationId == orgId)
                    .Where(r => r.RequestStatus == status)
                    .Include(u => u.Organization)
                    .ToListAsync();
            }

            var requestsDto = Mapper.Map<List<RequestDto>>(requests);
            return requestsDto;
        }

        /// <summary>
        /// GetCount
        /// </summary>
        /// <param name="orgId">orgId.</param>

        public async Task<int[]> GetCount(long orgId)
        {
            int[] result = new int[3];
            result[0] = await Repository
                .GetAll()
                .Where(c => c.RequestStatus == RequestStatus.Waiting)
                .CountAsync();
            if (orgId == 0)
            {
                result[1] = await Repository
                    .GetAll()
                    .Where(c => c.RequestStatus == RequestStatus.Execution)
                    .CountAsync();
                result[2] = await Repository
                    .GetAll()
                    .Where(c => c.RequestStatus == RequestStatus.Done)
                    .CountAsync();
            }
            else
            {
                result[1] = await Repository
                    .GetAll()
                    .Where(c => c.OrganizationId == orgId)
                    .Where(c => c.RequestStatus == RequestStatus.Execution)
                    .CountAsync();
                result[2] = await Repository
                    .GetAll()
                    .Where(c => c.RequestStatus == RequestStatus.Done)
                    .CountAsync();
            }

            return result;
        }

        /// <summary>
        /// Изменить статус заявки
        /// </summary>
        /// <param name="requestId">requestId</param>
        /// <param name="status">status</param>
        /// <param name="organizationId">organizationId</param>
        public async Task<ActionResult<Request>> ChangeStatus(long requestId, RequestStatus status, long organizationId)
        {
            var request = await Repository.Get(x => (x.Id == requestId)).FirstOrDefaultAsync();
            switch (status)
            {
                case RequestStatus.Waiting:
                    request.OrganizationId = null;
                    request.RequestStatus = RequestStatus.Waiting;
                    break;
                case RequestStatus.Execution:
                    if (request.RequestStatus == RequestStatus.Waiting)
                        request.OrganizationId = organizationId;
                    request.RequestStatus = RequestStatus.Execution;
                    request.FinishDate = null;
                    break;
                case RequestStatus.Done:
                    request.RequestStatus = RequestStatus.Done;
                    request.FinishDate = DateTime.Now;
                    break;
            }

            await Repository.Update(request);
            await Repository.SaveChangesAsync();
            return request;
        }

        /// <summary>
        /// Написать комментарий
        /// </summary>
        /// <param name="requestId">requestId</param>
        /// <param name="comment">comment</param>
        public async Task<ActionResult<Request>> CreateComment(long requestId, string comment)
        {
            var request = await Repository.Get(x => (x.Id == requestId)).FirstOrDefaultAsync();
            request.Comment = comment;
            await Repository.Update(request);
            await Repository.SaveChangesAsync();
            return request;
        }

        /// <summary>
        /// PostRequests
        /// </summary>
        /// <param name="requestDto">request.</param>
        public async Task<ActionResult<Request>> Create(CreateRequestDto requestDto)
        {
            if (string.IsNullOrEmpty(requestDto.FIO))
            {
                return null;
            }

            var request = Mapper.Map<Request>(requestDto);
            request.StartDate = DateTime.Now;
            request.RequestStatus = RequestStatus.Waiting;
            await Repository.Add(request);
            await Repository.SaveChangesAsync();
            return request;
        }
    }
}