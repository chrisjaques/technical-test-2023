using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnicalTest2023.Models
{
    [Index(nameof(FirstName), nameof(LastName))]
    public sealed class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        [MinLength(1, ErrorMessage = "First name is required")]
        [MaxLength(250, ErrorMessage = "First name must be at most 250 characters")]
        public required string FirstName { get; set; }
        [MinLength(1, ErrorMessage = "Last name is required")]
        [MaxLength(250, ErrorMessage = "Last name must be at most 250 characters")]
        public required string LastName { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")] // This doesn't actually format the date correctly
        [DataType(DataType.Date)]
        [Range(typeof(DateOnly), "", "")]
        public DateOnly DateOfBirth { get; set; }
        public required Address Address { get; set; }

        public static User Convert(UserDTO userDto)
        {
            return new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                DateOfBirth = userDto.DateOfBirth,
                Address = new Address
                {
                    City = userDto.Address.City,
                    PostCode = userDto.Address.PostCode,
                    StreetName = userDto.Address.StreetName,
                    StreetNumber = userDto.Address.StreetNumber,
                    StreetNumberSuffix = userDto.Address.StreetNumberSuffix,
                    Suburb = userDto.Address.Suburb
                }
            };
        }
    }
}
