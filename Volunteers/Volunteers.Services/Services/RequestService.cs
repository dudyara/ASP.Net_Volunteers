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
        /// GetRequests.
        /// </summary>
        public async Task<ActionResult<IEnumerable<RequestDto>>> Get()
        {
            var requests = await Repository.GetAll().ToListAsync();
            var requestsDto = Mapper.Map<List<RequestDto>>(requests);
            return requestsDto;
        }

        /// <summary>
        /// GetPull.
        /// </summary>
        public async Task<ActionResult<IEnumerable<RequestDto>>> GetPull()
        {
            var requests = await Repository.Get(x => x.RequestStatus == RequestStatus.Waiting).ToListAsync();
            var requestsDto = Mapper.Map<List<RequestDto>>(requests);
            return requestsDto;
        }

        /// <summary>
        /// GetOneRequest
        /// </summary>
        /// <param name="id">id.</param>
        public async Task<ActionResult<RequestDto>> GetById(long id)
        {
            var request = await Repository.Get(x => x.Id == id)
                .FirstOrDefaultAsync();
            var requestDto = Mapper.Map<RequestDto>(request);
            return requestDto;
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
            request.RequestPriority = RequestPriority.Undefined;
            await Repository.Add(request);
            await Repository.SaveChangesAsync();
            return request;
        }
    }
}