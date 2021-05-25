namespace Volunteers.Services
{
    using Mapper;
    using Volunteers.DB;

    /// <summary>
    /// Base service
    /// </summary>
    public abstract class BaseService
    {
        /// <summary>
        /// Repository
        /// </summary>
        private IDbRepository repository;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="mapper">Mapper</param>
        /// <param name="repository">Repository</param>
        protected BaseService(IVolunteerMapper mapper, IDbRepository repository)
        {
            Mapper = mapper;
            this.repository = repository;
        }

        /// <summary>
        /// Mapper.
        /// </summary>
        protected IVolunteerMapper Mapper { get; }
    }
}
