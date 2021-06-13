
namespace Volunteers.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using Volunteers.DB;
    using Volunteers.Entities;
    using Volunteers.Services.Dto;
    /// <summary>
    /// AuthenticationService
    /// </summary>
    public class AuthorizationService : BaseManagerService<RegistrationToken>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
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
            _signInManager = signInManager;
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
        /// <param name="dto">dto</param>
        /// <param name="token">token</param>
        /// <param name="organizationService">organizationService</param>
        /// <returns></returns>
        public async Task<long> AddUser(RegistrationDto dto, string token, OrganizationService organizationService)
        {
            /*  if (TokenRepository.Get(x => x.Token == token).First() != null)
              {*/
            var user = new User { Email = dto.Email, UserName = dto.Email }; 
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                return user.Id;
            }

            return 0;

            /*            }

                        return IdentityResult.Failed();*/
        }

        /// <summary>
        /// AuthentucateAsync
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns></returns>
        public async Task<string> AuthenticateAsync(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
            var tokenKey = "This is my test private key";
            if (result.Succeeded)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(tokenKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, "Organization")
                   }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha512Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var result1 = tokenHandler.WriteToken(token);
                return result1;
            }

            return "Bad Request";
        }
    }
}
