namespace Volunteers.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Volunteers.Services.Services;

    /// <summary>
    /// TestController.
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : Controller
    {
        /// <summary>
        /// It's a test method Index.
        /// </summary>
        /// <param name="service">Сервис.</param>
        [HttpGet]
        public string GetHelloWorld(
            [FromServices] TestService service)
        {
            return service.GetHelloWorld();
        }

        /// <summary>
        /// This is a sum method
        /// </summary>
        /// <param name="a">first number is a! </param>
        /// <param name="b">first number is b! </param>
        [HttpGet("param")]
        public int SumNumbers(int a, int b)
        {
            return a + b;
        }
    }
}
