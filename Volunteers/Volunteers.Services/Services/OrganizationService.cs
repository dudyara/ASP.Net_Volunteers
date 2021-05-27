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

            var phones = new List<PhoneNumber>();
            List<ActivityType> activities = await Repository
                .Get(o => o.ActivityType)
                .Where(o => o.ActivityTypes.Any(x => orgDto.Activities.Contains(x.Id)))
                .ToListAsync();

            for (int i = 0; i < orgDto.Phones.Count; i++)
                phones.Add(new PhoneNumber() { Phone = orgDto.Phones[i] });

            org.PhoneNumbers = phones;
            org.ActivityTypes = activities;
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
            var organizations = await Repository.Get().Include(c => c.ActivityTypes).ToListAsync();
            List<Organization> result = new List<Organization>();
            int c = 0;
            for (int i = 0; i < organizations.Count; i++)
            {
                for (int j = 0; j < organizations[i].ActivityTypes.Count; j++)
                {
                    for (int k = 0; k < ids.Count; k++)
                    {
                        if (organizations[i].ActivityTypes[j].Id == ids[k])
                        {
                            result.Add(organizations[i]);
                            goto LoopEnd;
                        }
                    }
                }

            LoopEnd: c++;
            }

            var organizationDtos = Mapper.Map<List<OrganizationDto>>(result);
            return organizationDtos;
        }
    }
}
