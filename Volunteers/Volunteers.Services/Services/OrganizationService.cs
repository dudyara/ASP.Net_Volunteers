namespace Volunteers.Services.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Volunteers.DB;
    using Volunteers.Entities;
    using Volunteers.Services.Dto;
    using Volunteers.Services.Mapper;

    /// <summary>
    /// OrganizationService.
    /// </summary>
    public class OrganizationService : BaseService<Organization>
    {

        /// <summary>
        /// OrganizationService.
        /// </summary>
        /// <param name="mapper">mapper.</param>
        /// <param name="repository">repository.</param>
        public OrganizationService(
            IVolunteerMapper mapper,
            IDbRepository<Organization> repository)
            : base(mapper, repository)
        {
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <returns>.</returns>
        public async Task<ActionResult<IEnumerable<OrganizationDto>>> Get()
        {
            var orgs = await Repository.GetAll().ToListAsync();
            var orgDto = Mapper.Map<List<OrganizationDto>>(orgs);
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

            var org = Mapper.Map<Organization>(orgDto);
            await Repository.Add(org);
            await Repository.SaveChangesAsync();
            return org;
        }

        /// <summary>
        /// GetOneOrganization
        /// </summary>
        /// <param name="id">id.</param>
        public async Task<ActionResult<OrganizationDto>> GetById(long id)
        {
            var org = await Repository.Get().FirstOrDefaultAsync(x => x.Id == id);
            var orgDto = Mapper.Map<OrganizationDto>(org);
            return orgDto;
        }
    }
}
