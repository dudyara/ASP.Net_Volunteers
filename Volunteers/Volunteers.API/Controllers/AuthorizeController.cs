namespace Volunteers.API.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Authorization controller
    /// </summary>
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class AuthorizeController : ControllerBase
    {
        private readonly ILogger<AuthorizeController> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeController"/> class.
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="signInManager">signInManager</param>
        /// <param name="userManager">userManager</param>
        public AuthorizeController(
            ILogger<AuthorizeController> logger,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        /// <summary>
        /// Test method get
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<string>> Get()
        {
            return await _userManager.Users.Select(x => x.UserName).ToListAsync();
        }

        /// <summary>
        /// Test add new user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IdentityResult> AddUser()
        {
            var user = new IdentityUser { Email = "helltrial@gmail.com", UserName = "Testlogin" };
            return await _userManager.CreateAsync(user, "aA123123123!");
        }
    }
}
