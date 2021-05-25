namespace Volunteers.Services.Services
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using AutoMapper;
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
    public class RequestService : BaseService
    {
        private IDbRepository repository;
        private IVolunteerMapper mapper;

        /// <summary>
        /// Метод заявок.
        /// </summary>
        /// <param name="mapper">Маппер.</param>
        /// <param name="repository">repository</param>
        public RequestService(IVolunteerMapper mapper, IDbRepository repository)
            : base(mapper, repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        /// <summary>
        /// GetRequests.
        /// </summary>
        public async Task<ActionResult<IEnumerable<RequestDto>>> Get()
        {
            var requests = await repository.Get<Request>().ToListAsync();
            var requestsDto = mapper.Map<List<RequestDto>>(requests);
            return requestsDto;
        }

        /// <summary>
        /// GetPull.
        /// </summary>
        public async Task<ActionResult<IEnumerable<RequestDto>>> GetPull()
        {
            var requests = await repository.Get<Request>(x => x.RequestStatus == RequestStatus.Waiting).ToListAsync();
            var requestsDto = mapper.Map<List<RequestDto>>(requests);
            return requestsDto;
        }

        /// <summary>
        /// GetOneRequest
        /// </summary>
        /// <param name="id">id.</param>
        public async Task<ActionResult<RequestDto>> GetByID(long id)
        {
            var request = await repository.Get<Request>().FirstOrDefaultAsync(x => x.Id == id);
            var requestDto = mapper.Map<RequestDto>(request);
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

            var request = mapper.Map<Request>(requestDto);
            request.StartDate = DateTime.Now;
            request.RequestStatus = RequestStatus.Waiting;
            request.RequestPriority = RequestPriority.Undefined;
            await repository.Add(request);
            await repository.SaveChangesAsync();
            return request;
        }
    }
}
