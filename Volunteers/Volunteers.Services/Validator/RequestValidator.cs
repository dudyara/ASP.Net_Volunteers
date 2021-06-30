namespace Volunteers.Services.Validator
{
    using System.Linq;
    using FluentValidation;
    using Volunteers.Services.Dto;

    /// <summary>
    /// RequestValidator
    /// </summary>
    public class RequestValidator : AbstractValidator<RequestCreateDto>
    {
        /// <summary>
        /// ctor.
        /// </summary>
        public RequestValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                    .WithMessage("Поле не должно быть пустым")
                .Must(NoSymbols)
                    .WithMessage("Поле должно содержать только буквы")
                .Length(1, 100)
                    .WithMessage("Количество символов должно быть от 1 до 100");
            RuleFor(p => p.PhoneNumber)
                .NotEmpty()
                    .WithMessage("Поле не должно быть пустым")
                .Length(10)
                    .WithMessage("Количество символов должно быть 10")
                .Must(IsPhoneValid)
                    .WithMessage("Номер должен содержать только цифры");
        }

        /// <summary>
        /// Проверка валидации телеона
        /// </summary>
        /// <param name="phone">phone</param>
        /// <returns></returns>
        private bool IsPhoneValid(string phone)
        {
            return phone[1..].All(c => char.IsDigit(c));
        }

        private bool NoSymbols(string name)
        {
            var isDigit = name.Any(c => char.IsDigit(c));
            var isSymbol = name.Any(d => char.IsSymbol(d));
            var isPunctuation = name.Any(d => char.IsPunctuation(d));
            if (isDigit == true || isSymbol == true || isPunctuation == true)
                return false;
            else
                return true;
        }
    }
}
