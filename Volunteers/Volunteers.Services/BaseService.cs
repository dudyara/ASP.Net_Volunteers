namespace Volunteers.Services
{
    using FluentValidation;
    using Mapper;
    using Volunteers.DB;
    using Volunteers.Entities;
    using Volunteers.Services.Dto;

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
        /// <param name="validator">validator</param>
        protected BaseService(IVolunteerMapper mapper, IDbRepository<TEntity> repository, IValidator validator)
        {
            Mapper = mapper;
            Repository = repository;
            Validator = validator;
        }

        /// <summary>
        /// Mapper.
        /// </summary>
        protected IVolunteerMapper Mapper { get; }

        /// <summary>
        /// Repository.
        /// </summary>
        protected IDbRepository<TEntity> Repository { get; }

        /// <summary>
        /// Validator.
        /// </summary>
        protected IValidator Validator { get; }

        /* /// <summary>
        /// GetById
        /// </summary>
        /// <returns></returns>
        public TDto GetById() { }

        /// <summary>
        /// Add
        /// </summary>
        /// <returns></returns>
        public TDto Add() 
        {
            var result = Repository.Add();
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <returns></returns>
        public TDto Update() { }

        /// <summary>
        /// Delete
        /// </summary>
        /// <returns></returns>
        public TDto Delete() { }*/
    }
}
