namespace Volunteers.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
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
        public RequestService(IVolunteerMapper mapper, IDbRepository<Request> repository)
            : base(mapper, repository)
        {
        }

        /// <summary>
        /// GetPull.
        /// </summary>
        /// <param name="status">status</param>
        /// <param name="orgId">id</param>
        public async Task<ActionResult<IEnumerable<RequestDto>>> GetPull(RequestStatus status, long orgId)
        {
            List<Request> requests = new List<Request>();
            switch (status)
            {
                case RequestStatus.Waiting:
                    requests = await Repository.Get(x => x.RequestStatus == RequestStatus.Waiting).ToListAsync();
                    break;
                default:
                    requests = await Repository.Get(x => (x.Organization.Id == orgId) &&
                        (x.RequestStatus == status)).ToListAsync();
                    break;
            }

            var requestsDto = Mapper.Map<List<RequestDto>>(requests);
            return requestsDto;
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