namespace Volunteers.Services.Validator
{
    using System.Linq;
    using FluentValidation;
    using Volunteers.Services.Dto;

    /// <summary>
    /// ActivityTypeValidator
    /// </summary>
    public class ActivityTypeValidator : AbstractValidator<ActivityTypeDto>
    {
        /// <summary>
        /// ctor.
        /// </summary>
        public ActivityTypeValidator()
        {
            RuleFor(p => p.TypeName)
                .NotEmpty()
                    .WithMessage("Поле не должно быть пустым")
                .Must(NoSymbols)
                    .WithMessage("Поле должно содержать только буквы")
                .Length(1, 50)
                    .WithMessage("Количество символов должно быть от 1 до 50");
        }

        /// <summary>
        /// Проверка отсутствия символов
        /// </summary>
        /// <param name="name">name</param>
        /// <returns></returns>
        private bool NoSymbols(string name)
        {
            var isDigit = name.Any(char.IsDigit);
            var isSymbol = name.Any(char.IsSymbol);
            var isPunctuation = name.Any(char.IsPunctuation);
            return !isDigit && !isSymbol && !isPunctuation;
        }
    }
}
