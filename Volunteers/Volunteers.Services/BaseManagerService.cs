namespace Volunteers.Services
{
    using Microsoft.AspNetCore.Identity;
    using Volunteers.DB;
    using Volunteers.Entities;

    /// <summary>
    /// Base service
    /// </summary>
    /// <typeparam name="TEntity">Entity</typeparam>
    public abstract class BaseManagerService<TEntity>
    where TEntity : class, IEntity
    {
        /// <summary>
        /// BaseManager
        /// </summary>
        /// <param name="repository">repository</param>
        /// <param name="signInManager">signInManager</param>
        /// <param name="userManager">userManager</param>
        /// <param name="validator">validator</param>
        protected BaseManagerService(
            IDbRepository<TEntity> repository,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IDtoValidator validator)
        {
            Repository = repository;
            Validator = validator;
        }

        /// <summary>
        /// Repository.
        /// </summary>
        protected IDbRepository<TEntity> Repository { get; set; }

        /// <summary>
        /// Validator
        /// </summary>
        protected IDtoValidator Validator { get; }
    }
}
