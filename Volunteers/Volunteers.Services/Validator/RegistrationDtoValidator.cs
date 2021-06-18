
namespace Volunteers.Services.Validator
{
    using System.Linq;
    using FluentValidation;
    using Volunteers.Services.Dto;

    /// <summary>
    /// RegistrationDtoValidator
    /// </summary>
    public class RegistrationDtoValidator : AbstractValidator<RegistrationDto>
    {
        /// <summary>
        /// RegistrationDtoValidator
        /// </summary>
        public RegistrationDtoValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(p => p.Password)
                .NotEmpty()
                .MinimumLength(6);
        }
    }
}