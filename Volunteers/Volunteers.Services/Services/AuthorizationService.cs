namespace Volunteers.Services.Services
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
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
        public const string BaseLink = "https://rubius.ru";
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDbContext _appDbContext;

        /// <summary>
        /// AuthenticationService
        /// </summary>
        /// <param name="signInManager">signInManager</param>
        /// <param name="userManager">userManager</param>
        /// <param name="repository">repository</param>
        /// <param name="appDbContext">appDbContext</param>
        public AuthorizationService(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IDbRepository<RegistrationToken> repository,
            AppDbContext appDbContext)
            : base(repository, signInManager, userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appDbContext = appDbContext;
        }

        /// <summary>
        /// CheckRegistrationToken
        /// </summary>
        /// <param name="token">token</param>
        /// <returns></returns>
        public bool CheckRegistrationToken(string token)
        {
            if (Repository.Get(s => s.Token == token).FirstOrDefault() != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// GenerateLink
        /// </summary>
        /// <returns></returns>
        public string GenerateLink()
        {
            RegistrationToken registrationToken = RegistrationToken.GenerateToken(new RegistrationToken());
            Repository.Add(registrationToken);

            var link = BaseLink + "?token=" + registrationToken.Token;
            Repository.SaveChangesAsync();
            return link;
        }

        /// <summary>
        /// GetToken
        /// </summary>
        /// <param name="organizationId">id организации</param>
        public string GenerateLink(long organizationId)
        {
            // Создаем токен регистрации
            RegistrationToken registrationToken = RegistrationToken.GenerateToken(new RegistrationToken());
            Repository.Add(registrationToken);

            // создаем ссылку, где указываем токен и id организации
            var link = BaseLink + "Authorize/RegisterUser" + "?token=" + registrationToken.Token + "&id=" + organizationId;
            Repository.SaveChangesAsync();
            return link;
        }

        /// <summary>
        /// AddUser
        /// </summary>
        /// <param name="dto">dto</param>
        public async Task<long> AddUser(RegistrationDto dto)
        {
            var user = new User { Email = dto.Email, UserName = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                return user.Id;
            }

            return 0;
        }

        /// <summary>
        /// AddUser
        /// </summary>
        /// <param name="dto">dto organization</param>
        /// <param name="id">id orgnization</param>
        /// <param name="dbRepository">dbRepository</param>
        /// <returns></returns>
        public async Task<long> AddUser(RegistrationDto dto, long id)
        {
            var user = new User { Email = dto.Email, UserName = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                var organization = _appDbContext.Organizations.Where(x => x.Id == id).FirstOrDefault();
                if (organization != null)
                {
                    organization.UserId = id;
                    await _appDbContext.SaveChangesAsync();
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