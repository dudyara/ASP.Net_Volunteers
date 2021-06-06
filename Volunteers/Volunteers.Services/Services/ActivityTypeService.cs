namespace Volunteers.Services.Services
{
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
    public class ActivityTypeService : BaseService<ActivityType>
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
             IValidator validator)
             : base(mapper, repository, validator)
        {
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="actDto">actDto</param>
        /// <returns></returns>
        public async Task<ActionResult<ActivityType>> Create(ActivityTypeCreateDto actDto)
        {
            var activityType = Mapper.Map<ActivityType>(actDto);
            await Repository.Add(activityType);
            return activityType;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="actDto">actDto</param>
        /// <returns></returns>
        public async Task<ActionResult<ActivityType>> Update(ActivityTypeDto actDto)
        {
            var activityType = Mapper.Map<ActivityType>(actDto);
            await Repository.Update(activityType);
            return activityType;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<ActionResult<ActivityType>> Delete(long id)
        {
            var activityType = await Repository.Get().FirstOrDefaultAsync(x => x.Id == id);
            await Repository.Delete(activityType);
            return activityType;
        }

            /// <summary>
            /// Виды активности
            /// </summary>
            /// <returns></returns>    
        public async Task<ActionResult<List<ActivityTypeDto>>> Get()
        {
            var activityTypes = await Repository.Get().ToListAsync();
            var activityTypeDtos = Mapper.Map<List<ActivityTypeDto>>(activityTypes);
            return activityTypeDtos;
        }
    }
}
