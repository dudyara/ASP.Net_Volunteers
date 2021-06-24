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
                .MinimumLength(6)
                .Must(IsPasswordValid)
                    .WithMessage("Пароль должен содержать символы верхнего и нижнего регистров, цифры и не алфавитно-цифровые символы");
        }

        private bool IsPasswordValid(string passwd)
        {
            var isDigit = passwd.Any(c => char.IsDigit(c));
            var isLower = passwd.Any(d => char.IsLower(d));
            var isUpper = passwd.Any(c => char.IsUpper(c));
            var isSymbol = passwd.Any(d => char.IsSymbol(d));
            var isPunctuation = passwd.Any(d => char.IsPunctuation(d));
            if (isDigit == true && isLower == true && isUpper == true && (isSymbol == true || isPunctuation == true))
                return true;
            else
                return false;
        }
    }
}