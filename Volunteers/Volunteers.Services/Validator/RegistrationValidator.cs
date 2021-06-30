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
                    .WithMessage("Поле не должно быть пустым")
                .EmailAddress()
                    .WithMessage("Неверный формат электронной почты");
            RuleFor(p => p.Password)
                .NotEmpty()
                    .WithMessage("Поле не должно быть пустым")
                .MinimumLength(6)
                    .WithMessage("Поле должно содержать как минимум 6 символов")
                .Must(IsPasswordValid)
                    .WithMessage("Пароль должен содержать строчные и заглавные буквы, цифры и символы");
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