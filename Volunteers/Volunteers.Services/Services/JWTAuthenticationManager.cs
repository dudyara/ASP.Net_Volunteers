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

    /// <summary>
    /// JWTAuthenticationManager
    /// </summary>
    public class JWTAuthenticationManager
    {
        private readonly string tokenKey;
        private readonly SignInManager<User> _signInManager;

        /// <summary>
        /// JWTAuthenticationManager
        /// </summary>
        /// <param name="signInManager">signInManager</param>
        /// <param name="repository">repository</param>
        /// <param name="userManager">userManager</param>
        public JWTAuthenticationManager(
            IDbRepository<User> repository,
            SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            /* this.tokenKey = "This is my test private key";
             *//*Repository = repository;*//*
             _signInManager = signInManager;
             *//* _appDbContext = appDbContext;*/
        }

        /* /// <summary>
         /// aervfmkmokv
         /// </summary>
         public DbRepository<User> UserRepository { get; set; }*/

        /// <summary>
        /// Authenticate
        /// </summary>
        /// <param name="email">username</param>
        /// <param name="password">password</param>
        /// <returns></returns>
        public async Task<string> AuthenticateAsync(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);

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
                return tokenHandler.WriteToken(token);
            }

            return "Bad Request";
        }
    }
}
