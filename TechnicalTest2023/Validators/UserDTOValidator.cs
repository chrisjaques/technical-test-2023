using FluentValidation;
using TechnicalTest2023.Models;

namespace TechnicalTest2023.Validators
{
    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        public UserDTOValidator()
        {
            RuleFor(userDto => userDto.DateOfBirth)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now).AddYears(-150));
        }
    }
}
