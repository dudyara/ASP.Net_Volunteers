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
    /// SuperAdminService
    /// </summary>
    public class SuperAdminService : BaseService<Request>
    {
        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="mapper">Маппер.</param>
        /// <param name="repository">repository</param>
        /// <param name="validator">validator</param>
        public SuperAdminService(IVolunteerMapper mapper, IDbRepository<Request> repository, IValidator validator)
            : base(mapper, repository, validator)
        {
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="status">status</param>
        /// <param name="orgId">orgId</param>
        /// <returns></returns>
        public async Task<ActionResult<IEnumerable<RequestDto>>> Get(RequestStatus status, long orgId)
        {
            List<Request> requests = new List<Request>();
            if ((status == 0) && (orgId == 0))
            {
                requests = await Repository.Get().ToListAsync();
            }
            else
            if (status == 0)
            {
                requests = await Repository
                    .Get()
                    .Where(r => r.OrganizationId == orgId)
                    .ToListAsync();
            } 
            else
            if (orgId == 0)
            {
                requests = await Repository
                    .Get()
                    .Where(r => r.RequestStatus == status)
                    .ToListAsync();
            }
            else
            {
                requests = await Repository
                    .Get()
                    .Where(r => r.OrganizationId == orgId)
                    .Where(r => r.RequestStatus == status)
                    .ToListAsync();
            }

            var requestsDto = Mapper.Map<List<RequestDto>>(requests);
            return requestsDto;
        }
    }
}
