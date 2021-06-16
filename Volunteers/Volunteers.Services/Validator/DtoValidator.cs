namespace Volunteers.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using FluentValidation.Internal;
    using Microsoft.Extensions.DependencyInjection;

    /// <inheritdoc />
    public class DtoValidator : IDtoValidator
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">service Provider</param>
        public DtoValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public Task ValidateAndThrowAsync<T>(
            T instance,
            Action<ValidationStrategy<T>> options = null,
            CancellationToken cancellationToken = default)
        {
            var validator = _serviceProvider.GetRequiredService<IValidator<T>>();
            return validator.ValidateAsync(
                instance,
                o =>
                {
                    options?.Invoke(o);
                    o.ThrowOnFailures();
                },
                cancellationToken);
        }
    }
}