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
                .Length(1, 100);
            RuleFor(p => p.PhoneNumber)
                .NotEmpty()
                .Length(10)
                .Must(IsPhoneValid);
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
    }
}
