namespace Volunteers.Services.Services
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using Volunteers.DB;
    using Volunteers.Entities;
    using Volunteers.Services.Dto;

    /// <summary>
    /// AuthenticationService
    /// </summary>
    public class AuthorizationService : BaseManagerService<RegistrationToken>
    {
        /// <summary>
        /// BaseLink
        /// </summary>
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IDbRepository<Organization> _organizationRepo;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// AuthenticationService
        /// </summary>
        /// <param name="signInManager">signInManager</param>
        /// <param name="userManager">userManager</param>
        /// <param name="repository">repository</param>
        /// <param name="organizationRepo">organizationRepo</param>
        /// <param name="configuration">configuration</param>
        public AuthorizationService(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IDbRepository<RegistrationToken> repository,
            IDbRepository<Organization> organizationRepo,
            IConfiguration configuration)
            : base(repository, signInManager, userManager)
        {
            _userManager = userManager;
            _organizationRepo = organizationRepo;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        /// <summary>
        /// CheckRegistrationToken
        /// </summary>
        /// <param name="token">token</param>
        public async Task<bool> CheckRegistrationToken(string token) => (await Repository.Get(s => s.Token == token).FirstOrDefaultAsync()) != null;

        /// <summary>
        /// GetToken
        /// </summary>
        /// <param name="organizationId">id организации</param>
        public async Task<string> GenerateLink(long? organizationId = null)
        {
            // Создаем токен регистрации
            var registrationToken = RegistrationToken.GenerateToken();
            await Repository.AddAsync(registrationToken);
            await Repository.SaveChangesAsync();

            // создаем ссылку, где указываем токен и id организации, если он есть
            var link = $"{_configuration.GetValue<string>("BaseLink")}Authorize/RegisterUser?";

            if (organizationId.HasValue)
            {
                var organization = await _organizationRepo.Get(x => x.Id == organizationId).FirstOrDefaultAsync();
                if (organization.RegistrationTokenId != null)
                {
                    var token = await Repository.Get(x => x.Id == organization.RegistrationTokenId).Select(t => t.Token).FirstOrDefaultAsync();

                    link += $"token={token}&id={organizationId}";
                    return link;
                }

                link += $"token={registrationToken.Token}&id={organizationId}";
                organization.RegistrationTokenId = registrationToken.Id;
                await Repository.SaveChangesAsync();
                return link;
            }

            return link += $"token={registrationToken.Token}";
        }

        /// <summary>
        /// AddUser
        /// </summary>
        /// <param name="dto">dto organization</param>
        /// <param name="organizationId">id organization</param>
        public async Task<RegistrationDto> AddUserAsync(RegistrationDto dto, long? organizationId = null)
        {
            var user = new User { Email = dto.Email, UserName = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                if (organizationId.HasValue)
                {
                    var organization = await _organizationRepo.Get(x => x.Id == organizationId).FirstOrDefaultAsync();
                    if (organization != null)
                    {
                        organization.UserId = user.Id;
                        await _organizationRepo.SaveChangesAsync();
                        return dto;
                    }

                    throw new ArgumentException("Задан неверный id организации");
                }

                return dto;
            }

            throw new ArgumentException("Неправильно задан email или пароль");
        }

        /// <summary>
        /// AuthenticateAsync
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
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
                    Subject = new ClaimsIdentity(new[]
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
                var total_token = tokenHandler.WriteToken(token);
                return total_token;
            }

            return null;
        }
    }
}