namespace Volunteers.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation.Internal;

    /// <summary>
    /// Обобщенный валидатор DTO.
    /// </summary>
    public interface IDtoValidator
    {
        /// <summary>
        /// Performs validation asynchronously and then throws an exception if validation fails.
        /// This method is a shortcut for: ValidateAsync(instance, options => options.ThrowOnFailures());
        /// </summary>
        /// <param name="instance">The instance of the type we are validating.</param>
        /// <param name="options">Настройки валидатора.</param>
        /// <param name="cancellationToken">cancellation Token</param>
        /// <typeparam name="T">Instance type.</typeparam>
        Task ValidateAndThrowAsync<T>(
            T instance,
            Action<ValidationStrategy<T>> options = null,
            CancellationToken cancellationToken = default);
    }
}