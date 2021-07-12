﻿namespace Volunteers.Services.Validator
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
                    .WithMessage("Поле не должно быть пустым.")
                .EmailAddress()
                    .WithMessage("Неверный формат электронной почты.");
            RuleFor(p => p.Password)
                .NotEmpty()
                    .WithMessage("Поле не должно быть пустым.")
                .MinimumLength(6)
                    .WithMessage("Поле должно содержать как минимум 6 символов.")
                .Must(IsPasswordValid)
                    .WithMessage("Пароль должен содержать строчные и заглавные буквы, цифры и символы.");
        }

        private bool IsPasswordValid(string password)
        {
            var isDigit = password.Any(char.IsDigit);
            var isLower = password.Any(char.IsLower);
            var isUpper = password.Any(char.IsUpper);
            var isSymbol = password.Any(char.IsSymbol);
            var isPunctuation = password.Any(char.IsPunctuation);
            return isDigit && isLower && isUpper && (isSymbol || isPunctuation);
        }
    }
}