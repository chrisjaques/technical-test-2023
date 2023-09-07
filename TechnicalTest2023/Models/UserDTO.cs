using System.ComponentModel.DataAnnotations;

namespace TechnicalTest2023.Models
{
    public class UserDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }
        public required string Address { get; set; }

        public static UserDTO Convert(User user)
        {
            return new UserDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Address =
                    $"{user.Address.StreetNumber}{user.Address.StreetNumberSuffix ?? string.Empty} {user.Address.StreetName}, {user.Address.City}"
            };
        }
    }
}
