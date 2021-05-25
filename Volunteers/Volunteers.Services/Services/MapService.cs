/*namespace Volunteers.Services.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Volunteers.DB;
    using Volunteers.Entities;
    using Volunteers.Services.Dto;
    using Volunteers.Services.Mapper;

    /// <summary>
    /// Сервис, для работы с картой
    /// </summary>
    public class MapService : BaseService
    {
        private readonly IVolunteerMapper mapper;
        private readonly IDbRepository repository;

        /// <summary>
        /// MapService
        /// </summary>
        /// <param name="mapper">mapper</param>
        /// <param name="repository">repository</param>
        public MapService(IVolunteerMapper mapper, IDbRepository repository)
          : base(mapper, repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        /// <summary>
        /// Виды активности
        /// </summary>
        /// <returns></returns>    
        public async Task<ActionResult<List<ActivityTypeDto>>> GetActivityTypes()
        {
            var activityTypes = await repository.GetAll<ActivityType>().ToListAsync();
            var activityTypeDtos = mapper.Map<List<ActivityTypeDto>>(activityTypes);
            return activityTypeDtos;
        }

        /// <summary>
        /// Выдача организаций по типы их активности
        /// </summary>
        /// <param name="ids">id активность</param>
        /// <returns></returns>
        public async Task<ActionResult<List<OrganizationDto>>> GetById(List<long> ids)
        {
            var organizations = await repository.Get<Organization>().Include(c => c.ActivityTypes).ToListAsync();
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

            var organizationDtos = mapper.Map<List<OrganizationDto>>(result);
            return organizationDtos;
        }
    }
}
   */