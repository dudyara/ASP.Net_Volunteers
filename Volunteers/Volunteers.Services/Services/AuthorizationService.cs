namespace Volunteers.Services.Services
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
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
            await Repository.Add(registrationToken);
            await Repository.SaveChangesAsync();

            // создаем ссылку, где указываем токен и id организации
            var link = $"{_configuration.GetValue<string>("BaseLink")}Authorize/RegisterUser?token={registrationToken.Token}";

            if (organizationId.HasValue)
            {
                link += $"&id={organizationId}";
            }

            return link;
        }

        /// <summary>
        /// AddUser
        /// </summary>
        /// <param name="dto">dto</param>
        public async Task<long> AddUserAsync(RegistrationDto dto)
        {
            var user = new User { Email = dto.Email, UserName = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                return user.Id;
            }

            throw new Exception(string.Join(" ", result.Errors.Select(x => x.Description)));
        }

        /// <summary>
        /// AddUser
        /// </summary>
        /// <param name="dto">dto organization</param>
        /// <param name="organizationId">id organization</param>
        public async Task<long> AddUserAsync(RegistrationDto dto, long? organizationId = null)
        {
            var user = new User { Email = dto.Email, UserName = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                var organization = await _organizationRepo.Get(x => x.Id == organizationId).FirstOrDefaultAsync();
                if (organization != null)
                {
                    organization.UserId = user.Id;
                    await _organizationRepo.SaveChangesAsync();
                    return user.Id;
                }
            }

            return 0;
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
                var result1 = tokenHandler.WriteToken(token);
                return result1;
            }

            return "Bad Request";
        }
    }
}