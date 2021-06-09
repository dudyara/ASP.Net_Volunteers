namespace Volunteers.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
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
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// BaseManager
        /// </summary>
        /// <param name="repository">repository</param>
        /// <param name="signInManager">signInManager</param>
        /// <param name="userManager">userManager</param>
        protected BaseManagerService(
            IDbRepository<TEntity> repository,
            SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            signInManager = _signInManager;
            userManager = _userManager;
            Repository = repository;
        }

        /// <summary>
        /// Repository.
        /// </summary>
        protected IDbRepository<TEntity> Repository { get; set; }
    }
}
