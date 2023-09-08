using FluentValidation;
using TechnicalTest2023.Models;

namespace TechnicalTest2023.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.DateOfBirth)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now).AddYears(-150));
        }
    }
}
