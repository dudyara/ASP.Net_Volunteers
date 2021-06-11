
namespace Volunteers.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Volunteers.DB;
    using Volunteers.Entities;
    using Volunteers.Services.Dto;
    /// <summary>
    /// AuthenticationService
    /// </summary>
    public class AuthorizationService : BaseManagerService<RegistrationToken>
    {
        private readonly UserManager<User> _userManager;
        /// <summary>
        /// AuthenticationService
        /// </summary>
        /// <param name="service">service</param>
        /// <param name="signInManager">signInManager</param>
        /// <param name="userManager">userManager</param>
        /// <param name="repository">repository</param>
        /// <param name="tokenRepository">tok</param>
        public AuthorizationService(
           [FromServices] OrganizationService service,
           SignInManager<User> signInManager,
           UserManager<User> userManager,
           IDbRepository<RegistrationToken> repository)
           : base(repository, signInManager, userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// GetToken
        /// </summary>
        /// <param name="service">service</param>
        /// <returns></returns>
        public string GetToken()
        {
            RegistrationToken registrationToken = new RegistrationToken();
            var token = Guid.NewGuid();
            registrationToken.Token = token.ToString();
            registrationToken.ExpireTime = DateTime.Now.AddHours(24);
            Repository.Add(registrationToken);
            Repository.SaveChangesAsync();
            return token.ToString();
        }

        /// <summary>
        /// AddUser
        /// </summary>
        /// <param name="organizationDto">organizationDto</param>
        /// <param name="token">token</param>
        /// <param name="organizationService">organizationService</param>
        /// <param name="idUser">idUser</param>
        /// <returns></returns>
        public async Task<long> AddUser(OrganizationCreateDto organizationDto, string token, OrganizationService organizationService)
        {
            /*  if (TokenRepository.Get(x => x.Token == token).First() != null)
              {*/
            var user = new User { Email = organizationDto.Mail, UserName = organizationDto.Name }; 
            var result = await _userManager.CreateAsync(user, organizationDto.Password);

            if (result.Succeeded)
            {
                return user.Id;
            }

            return 0;

            /*            }

                        return IdentityResult.Failed();*/
        }
    }
}
