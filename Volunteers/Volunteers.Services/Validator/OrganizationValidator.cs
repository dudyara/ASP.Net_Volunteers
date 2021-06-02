namespace Volunteers.Services
{
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
            RuleFor(p => p.Mail)
                .NotEmpty()
                    .WithMessage("Необходимо ввести адрес электронной почты.")
                .EmailAddress()
                    .WithMessage("Неверный формат адреса электронной почты.");

            RuleForEach(p => p.Phones)
                .NotEmpty()
                    .WithMessage("Необходимо ввести номер телефона.");
        }
    }
}
