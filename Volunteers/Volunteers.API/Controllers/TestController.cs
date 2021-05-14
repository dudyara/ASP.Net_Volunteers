namespace Volunteers.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;

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
        [HttpGet]
        public string Index()
        {
            return "Hello, World";
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
