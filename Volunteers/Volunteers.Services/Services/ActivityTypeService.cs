
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
    /// Сервис для работы с видами деятельности компаний
    /// </summary>

    public class ActivityTypeService : BaseService<ActivityType>
    {
        /// <summary>
        /// Конструктор ActivityType
        /// </summary>
        /// <param name="mapper">mapper</param>
        /// <param name="repository">repository</param>
        public ActivityTypeService(
             IVolunteerMapper mapper,
             IDbRepository<ActivityType> repository)
             : base(mapper, repository)
        {
        }

        /// <summary>
        /// Виды активности
        /// </summary>
        /// <returns></returns>    
        public async Task<ActionResult<List<ActivityTypeDto>>> GetActivityTypes()
        {
            var activityTypes = await Repository.GetAll().ToListAsync();
            var activityTypeDtos = Mapper.Map<List<ActivityTypeDto>>(activityTypes);
            return activityTypeDtos;
        }
    }
}
