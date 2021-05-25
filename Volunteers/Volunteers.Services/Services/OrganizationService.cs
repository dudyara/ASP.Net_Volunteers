namespace Volunteers.Services.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Volunteers.DB;
    using Volunteers.Entities;
    using Volunteers.Services.Dto;
    using Volunteers.Services.Mapper;

    /// <summary>
    /// OrganizationService.
    /// </summary>
    public class OrganizationService : BaseService
    {
        private readonly IVolunteerMapper mapper;
        private readonly IDbRepository repository;

        /// <summary>
        /// OrganizationService.
        /// </summary>
        /// <param name="mapper">mapper.</param>
        /// <param name="repository">repository.</param>
        public OrganizationService(IVolunteerMapper mapper, IDbRepository repository)
            : base(mapper, repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <returns>.</returns>
        public async Task<ActionResult<IEnumerable<OrganizationDto>>> Get()
        {
            var orgs = await repository.Get<Organization>().ToListAsync();
            var orgDto = mapper.Map<List<OrganizationDto>>(orgs);
            return orgDto;
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="orgDto">org.</param>
        public async Task<ActionResult<Organization>> Create(OrganizationDto orgDto)
        {
            if (string.IsNullOrEmpty(orgDto.Name))
            {
                return null;
            }

            var org = mapper.Map<Organization>(orgDto);
            await repository.Add(org);
            await repository.SaveChangesAsync();
            return org;
        }

        /// <summary>
        /// GetOneOrganization
        /// </summary>
        /// <param name="id">id.</param>
        public async Task<ActionResult<OrganizationDto>> GetByID(long id)
        {
            var org = await repository.Get<Organization>().FirstOrDefaultAsync(x => x.Id == id);
            var orgDto = mapper.Map<OrganizationDto>(org);
            return orgDto;
        }
    }
}
