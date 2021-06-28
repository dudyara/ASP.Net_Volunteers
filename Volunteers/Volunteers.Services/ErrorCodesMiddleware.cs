namespace Volunteers.Services
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// ErrorCodes
    /// </summary>
    public class ErrorCodesMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="next">next</param>
        public ErrorCodesMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// InvokeAsync
        /// </summary>
        /// <param name="context">context</param>
        public async Task InvokeAsync(HttpContext context)
        {
            await _next.Invoke(context);
            switch (context.Response.StatusCode)
            {
                case 0:
                    await context.Response.WriteAsync("Код не задан");
                    break;
                case 400:
                    await context.Response.WriteAsync("Ошибка валидации запроса");
                    break;
                case 401:
                    await context.Response.WriteAsync("Пользователь не аутентифицирован");
                    break;
                case 403:
                    await context.Response.WriteAsync("У пользователя отсутствует доступ к объекту (на уровне бизнес-логики)");
                    break;
                case 404:
                    await context.Response.WriteAsync("Объект не найден");
                    break;
                case 409:
                    await context.Response.WriteAsync("Нарушение правил бизнес-логики.");
                    break;
            }
        }
    }
}
