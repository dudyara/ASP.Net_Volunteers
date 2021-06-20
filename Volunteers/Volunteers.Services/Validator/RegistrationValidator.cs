namespace Volunteers.Services.Validator
{
    using System.Linq;
    using FluentValidation;
    using Volunteers.Services.Dto;

    /// <summary>
    /// RegistrationDtoValidator
    /// </summary>
    public class RegistrationValidator : AbstractValidator<RegistrationDto>
    {
        /// <summary>
        /// RegistrationDtoValidator
        /// </summary>
        public RegistrationValidator()
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