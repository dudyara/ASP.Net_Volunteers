namespace Volunteers.Services
{
    using FluentValidation;
    using Volunteers.Entities;
    using Volunteers.Services.Dto;

    /// <summary>
    /// Класс валидации организаций
    /// </summary>
    public class OrganizationValidator : AbstractValidator<Organization>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public OrganizationValidator()
        {
            RuleFor(p => p.Mail)
                .NotEmpty()
                    .WithMessage("Необходимо ввести адрес электронной почты.")
                .EmailAddress()
                    .WithMessage("Неверный формат адреса электронной почты.");

            RuleFor(p => p.PhoneNumbers)
                .NotEmpty()
                    .WithMessage("Необходимо ввести номер телефона.")
                .Length(10)
                    .WithMessage("Неверный формат номера телефона.");
        }
    }
}
