namespace Volunteers.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
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
        /// Create
        /// </summary>
        /// <param name="actDto">actDto</param>
        public async Task<ActionResult<ActivityTypeDto>> Create(ActivityTypeDto actDto)
        {
            return await AddAsync(actDto);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="actDto">actDto</param>
        public async Task<ActionResult<ActivityTypeDto>> Update(ActivityTypeDto actDto)
        {
            return await UpdateAsync(actDto);
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

            /// <summary>
            /// Виды активности
            /// </summary>
        public async Task<ActionResult<List<ActivityTypeDto>>> Get()
        {
            return await GetAsync();
        }
    }
}
