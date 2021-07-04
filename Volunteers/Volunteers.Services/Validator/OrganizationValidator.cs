namespace Volunteers.Services
{
    using System.Linq;
    using FluentValidation;
    using Volunteers.Services.Dto;

    /// <summary>
    /// Класс валидации организаций
    /// </summary>
    public class OrganizationValidator : AbstractValidator<OrganizationDto>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public OrganizationValidator()
        {
            string notEmptyMessage = "Поле не должно быть пустым";

            RuleFor(p => p.Name)
                .NotEmpty()
                    .WithMessage(notEmptyMessage)
                .Length(1, 200)
                    .WithMessage("Поле должно содержать от 1 до 200 символов");
            RuleFor(p => p.Manager)
                .NotEmpty()
                    .WithMessage(notEmptyMessage)
                .Must(NoSymbols)
                    .WithMessage("Поле должно содержать только буквы")
                .Length(1, 100)
                    .WithMessage("Поле должно содержать от 1 до 100 символов");
            RuleFor(p => p.Description)
                .NotEmpty()
                    .WithMessage(notEmptyMessage)
                .Length(1, 500)
                    .WithMessage("Поле должно содержать от 1 до 500 символов");
            RuleForEach(p => p.ActivityTypes)
                .NotEmpty()
                    .WithMessage(notEmptyMessage);
            RuleFor(p => p.Mail)
                .NotEmpty()
                    .WithMessage(notEmptyMessage)
                .EmailAddress()
                    .WithMessage("Неверный формат электронной почты");
            RuleForEach(p => p.PhoneNumbers)
                .NotEmpty()
                    .WithMessage(notEmptyMessage)
                .Length(10)
                    .WithMessage("Поле должно содержать 10 символов")
                .Must(IsPhoneValid)
                    .WithMessage("Поле может содержать только цифры");
            RuleFor(p => p.WorkingHours)
                .NotEmpty()
                    .WithMessage(notEmptyMessage)
                .Length(1, 100)
                    .WithMessage("Поле должно содержать от 1 до 100 символов");
            RuleFor(p => p.Address)
                .NotEmpty()
                    .WithMessage(notEmptyMessage)
                .Length(1, 100)
                    .WithMessage("Поле должно содержать от 1 до 100 символов");
            RuleFor(p => p.Location)
                .NotEmpty()
                    .WithMessage(notEmptyMessage);
        }

        /// <summary>
        /// Проверка валидации телефона
        /// </summary>
        /// <param name="phone">phone</param>
        /// <returns></returns>
        private bool IsPhoneValid(string phone)
        {
            return phone[1..].All(c => char.IsDigit(c));
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
