namespace Volunteers.Services.Services
{
    using Volunteers.DB;
    using Volunteers.Entities;
    using Volunteers.Services.Dto;
    using Volunteers.Services.Mapper;

    /// <summary>
    /// ExcelService
    /// </summary>
    public class ExcelService : BaseService<Request, RequestDto>
    {
        /// <summary>
        /// Метод заявок.
        /// </summary>
        /// <param name="mapper">Маппер.</param>
        /// <param name="repository">repository</param>
        /// <param name="validator">validator</param>
        public ExcelService(IVolunteerMapper mapper, IDbRepository<Request> repository, IDtoValidator validator)
            : base(mapper, repository, validator)
        {
        }
    }
}
