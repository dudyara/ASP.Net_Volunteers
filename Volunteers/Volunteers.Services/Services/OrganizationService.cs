namespace Volunteers.Services.Services
{
    using System.Collections.Generic;
    using System.Linq;
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
        /// Create - получаем OrganizationDto в которых список айдишников activityTypes и список стрингов телефонов.
        /// Это все должно преобразоваться в список самих activityTypes и телефонов. для телефнов сделал, работает, но через for мне не нравится.
        /// </summary>
        /// <param name="orgDto">org.</param>
        public async Task<ActionResult<Organization>> Create(OrganizationDto orgDto)
        {
            var org = Mapper.Map<Organization>(orgDto);
            
            for (int i = 0; i < orgDto.Phones.Count; i++)
                org.PhoneNumbers.Add(new PhoneNumber() { Phone = orgDto.Phones[i] });
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

        /// <summary>
        /// Выдача организаций по типы их активности
        /// </summary>
        /// <param name="ids">id активность</param>
        /// <returns></returns>
        public async Task<ActionResult<List<OrganizationDto>>> GetByIds(List<long> ids)
        {
            var result = await Repository
                .Get()
                .Where(x => x.ActivityTypes.Any(at => ids.Contains(at.Id)))
                .ToListAsync();
            var organizationDtos = Mapper.Map<List<OrganizationDto>>(result);
            return organizationDtos;
        }
    }
}
