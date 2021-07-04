namespace Volunteers.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using FluentValidation;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Volunteers.DB;
    using Volunteers.Entities;
    using Volunteers.Services.Dto;
    using Volunteers.Services.Mapper;

    /// <summary>
    /// Сервис для работы с видами деятельности компаний
    /// </summary>
    public class ActivityTypeService : BaseService<ActivityType, ActivityTypeDto>
    {
        /// <summary>
        /// Конструктор ActivityType
        /// </summary>
        /// <param name="mapper">mapper</param>
        /// <param name="repository">repository</param>
        /// <param name="validator">validator</param>
        public ActivityTypeService(
             IVolunteerMapper mapper,
             IDbRepository<ActivityType> repository,
             IDtoValidator validator)
             : base(mapper, repository, validator)
        {
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="filter">filter</param>
        /// <returns></returns>
        public async Task<List<ActivityTypeDto>> Get(ActivityTypeFilterDto filter)
        {
            var result = new List<ActivityTypeDto>();
            if (filter.WithOrganizations == true)
            {
                result = await Repository
                    .Get()
                    .Where(a => a.Organizations.Count > 0)
                    .ProjectTo<ActivityTypeDto>(Mapper.ConfigurationProvider)
                    .ToListAsync();
            }
            else
            {
                result = await GetAsync();
            }

            return result;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">id</param>
        public async Task<ActionResult<ActivityType>> Delete(long id)
        {
            var activityType = await Repository.Get().FirstOrDefaultAsync(x => x.Id == id);
            await DeleteAsync(id);
            return activityType;
        }
    }
}
