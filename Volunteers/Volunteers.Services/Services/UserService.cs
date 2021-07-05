namespace Volunteers.Services.Services
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using Volunteers.DB;
    using Volunteers.Entities;
    using Volunteers.Services.Dto;
    using Volunteers.Services.Mapper;

    /// <summary>
    /// AuthenticationService
    /// </summary>
    public class UserService : BaseManagerService<RegistrationToken>
    {
        /// <summary>
        /// BaseLink
        /// </summary>
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IDbRepository<Organization> _organizationRepo;
        private readonly IDbRepository<Role> _roleRepo;
        private readonly IDbRepository<User> _userRepo;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// AuthenticationService
        /// </summary>
        /// <param name="signInManager">signInManager</param>
        /// <param name="userManager">userManager</param>
        /// <param name="repository">repository</param>
        /// <param name="organizationRepo">organizationRepo</param>
        /// <param name="configuration">configuration</param>
        /// <param name="roleRepo">roleRepo</param>
        /// <param name="userRepo">userRepo</param>
        /// <param name="mapper">mapper</param>
        /// <param name="validator">validator</param>
        public UserService(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IDbRepository<RegistrationToken> repository,
            IDbRepository<Organization> organizationRepo,
            IDbRepository<Role> roleRepo,
            IDbRepository<User> userRepo,
            IConfiguration configuration,
            IVolunteerMapper mapper,
            IDtoValidator validator)
            : base(repository, validator)
        {
            _userManager = userManager;
            _organizationRepo = organizationRepo;
            _signInManager = signInManager;
            _configuration = configuration;
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            Mapper = mapper;
        }

        /// <summary>
        /// Mapper
        /// </summary>
        public IVolunteerMapper Mapper { get; }

        /// <summary>
        /// CheckRegistrationToken
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="organizationId">organizationId</param>
        public async Task<bool> CheckRegistrationToken(string token, long? organizationId)
        {
            var result = await Repository.GetAll(s => s.Token == token).FirstOrDefaultAsync();
            if (result != null)
            {
                if (organizationId != null)
                {
                    var organization = await _organizationRepo.Get(x => x.Id == organizationId).FirstOrDefaultAsync();
                    organization.RegistrationTokenId = null;
                    await _organizationRepo.SaveChangesAsync();
                }

                await Repository.DeleteAsync(result);
                return true;
            }

            return false;
        }

        /// <summary>
        /// GetToken
        /// </summary>
        /// <param name="organizationId">id организации</param>
        public async Task<string> GenerateLink(long? organizationId)
        {
            // Создаем токен регистрации
            var registrationToken = new RegistrationToken();

            // создаем ссылку, где указываем токен и id организации, если он есть
            var link = $"{_configuration.GetValue<string>("BaseLink")}?";

            if (organizationId.HasValue)
            {
                // Проверяем, если у организации есть уже ссылка, то возващаем существующую
                var organization = await _organizationRepo.Get(x => x.Id == organizationId).FirstOrDefaultAsync();
                if (organization == null)
                {
                    throw new Exception("Неправильная ссылка, организации с таким id не существует");
                }

                if (organization.RegistrationTokenId != null)
                {
                    var token = await Repository.Get(x => x.Id == organization.RegistrationTokenId).Select(t => t.Token).FirstOrDefaultAsync();

                    link += $"token={token}&id={organizationId}";
                    return link;
                }

                registrationToken = RegistrationToken.GenerateToken();
                await Repository.AddAsync(registrationToken);
                await Repository.SaveChangesAsync();
                link += $"token={registrationToken.Token}&id={organizationId}";
                organization.RegistrationTokenId = registrationToken.Id;
                await Repository.SaveChangesAsync();
                return link;
            }

            registrationToken = RegistrationToken.GenerateToken();
            await Repository.AddAsync(registrationToken);
            await Repository.SaveChangesAsync();
            return link += $"token={registrationToken.Token}";
        }

        /// <summary>
        /// ChangePassword
        /// </summary>
        /// <param name="dto">dto</param>
        public async Task<IdentityResult> ChangePassword(PasswordDto dto)
        {
            var user = await _userRepo.Get(x => x.Id == dto.UserId).FirstOrDefaultAsync();
            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            await Repository.SaveChangesAsync();
            return result;
        }

        /// <summary>
        /// AddUser
        /// </summary>
        /// <param name="dto">dto organization</param>
        /// <param name="organizationId">id organization</param>
        public async Task<RegistrationDto> AddUserAsync(RegistrationDto dto, long? organizationId = null)
        {
            var checkEmail = await _organizationRepo.Get(org => org.User.Email == dto.Email).FirstOrDefaultAsync();
            if (!(checkEmail == null))
            {
                throw new Exception("Пользователь с такой почтой уже существует");
            }

            var user = new User { Email = dto.Email, UserName = dto.Email };
            user.RoleId = 2;
            var result = await _userManager.CreateAsync(user, dto.Password);

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

        /// <summary>
        /// AuthenticateAsync
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        public async Task<AuthenticateDto> AuthenticateAsync(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
            var tokenKey = "This is my test private key";
            if (result.Succeeded)
            {
                // находим пользователя по email
                var user = await _userManager.FindByEmailAsync(email);

                // определяем роль
                var roleUser = await _roleRepo.Get(x => x.Id == user.RoleId).FirstOrDefaultAsync();
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(tokenKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Email, email),
                        new Claim(ClaimTypes.Role, roleUser.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddHours(24),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha512Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                var authenticateDto = new AuthenticateDto();
                var total_token = tokenHandler.WriteToken(token);
                authenticateDto.Token = total_token;
                authenticateDto.Role = roleUser.ToString();
                var organizationResult = await _organizationRepo
                    .Get(t => t.UserId == user.Id)
                    .ProjectTo<OrganizationDto>(Mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                if (organizationResult != null)
                {
                    var organizationDto = Mapper.Map<OrganizationDto>(organizationResult);
                    authenticateDto.Data = organizationDto;
                    authenticateDto.UserId = user.Id;
                }
                else
                {
                    authenticateDto.Data = null;
                    authenticateDto.UserId = user.Id;
                }

                return authenticateDto;
            }

            return null;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">id пользователей</param>
        public void Delete(long id)
        {
            Repository.DeleteAsync(id);
        }
    }
}