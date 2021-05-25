namespace Volunteers.Services
{
    using Mapper;
    using Volunteers.DB;
    using Volunteers.Entities;

    /// <summary>
    /// Base service
    /// </summary>
    /// <typeparam name="TEntity">Entity</typeparam>
    public abstract class BaseService<TEntity>
    where TEntity : class, IEntity
    {
        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="mapper">Mapper</param>
        /// <param name="repository">Repository</param>
        protected BaseService(IVolunteerMapper mapper, IDbRepository<TEntity> repository)
        {
            Mapper = mapper;
            Repository = repository;
        }

        /// <summary>
        /// Mapper.
        /// </summary>
        protected IVolunteerMapper Mapper { get; }

        /// <summary>
        /// Repository.
        /// </summary>
        protected IDbRepository<TEntity> Repository { get; }
    }
}
