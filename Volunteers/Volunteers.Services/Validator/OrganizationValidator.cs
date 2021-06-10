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
            RuleFor(p => p.Name)
                .Must(p => p.All(char.IsLetter))
                .NotEmpty()
                .Length(1, 200);
            RuleFor(p => p.Logo)
                .NotEmpty();
            RuleFor(p => p.Manager)
                .Must(p => p.All(char.IsLetter))
                .NotEmpty()
                .Length(1, 100);
            RuleFor(p => p.Description)
                .Must(p => p.All(char.IsLetter))
                .NotEmpty()
                .Length(1, 500);
            RuleForEach(p => p.ActivityTypes)
                .NotEmpty();
            RuleFor(p => p.Mail)
                .NotEmpty()
                .EmailAddress();
            RuleForEach(p => p.PhoneNumbers)
                .NotEmpty()
                .Length(10)
                .Must(IsPhoneValid);
            RuleFor(p => p.WorkingHours)
                .NotEmpty()
                .Length(1, 100);
            RuleFor(p => p.Address)
                .NotEmpty()
                .Length(1, 100);
            RuleFor(p => p.Location)
                .NotEmpty();
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
    }
}
