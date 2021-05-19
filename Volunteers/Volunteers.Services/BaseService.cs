namespace Volunteers.Services
{
    using Mapper;

    /// <summary>
    /// Base service
    /// </summary>
    public abstract class BaseService
    {
        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="mapper">Mapper.</param>
        protected BaseService(IVolunteerMapper mapper)
        {
            Mapper = mapper;
        }

        /// <summary>
        /// Mapper.
        /// </summary>
        protected IVolunteerMapper Mapper { get; }
    }
}
