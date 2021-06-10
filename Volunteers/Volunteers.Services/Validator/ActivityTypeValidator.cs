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
                .Length(1, 50);
        }
    }
}
