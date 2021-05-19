namespace Volunteers.Services.Services
{
    using Mapper;

    /// <summary>
    /// Тестовый сервис
    /// </summary>
    public class TestService : BaseService
    {
        /// <inheritdoc />
        public TestService(IVolunteerMapper mapper)
            : base(mapper)
        {
        }

        /// <summary>
        /// Hello world
        /// </summary>
        /// <returns></returns>
        public string GetHelloWorld()
        {
            return "Hello world";
        }
    }
}
