using FluentValidation;
using TechnicalTest2023.Models;

namespace Tests.Unit_Tests
{
    public class ConverterTests
    {
        [Fact]
        public void Convert_to_userDTO_should_throw()
        {
            var invalidUser = GetUser(false);

            Assert.Throws<ValidationException>(() => UserDTO.Convert(invalidUser));
        }

        [Fact]
        public void Convert_to_userDTO_should_match()
        {
            var validUser = GetUser(true);

            var validUserDto = UserDTO.Convert(validUser);

            Assert.Equal(validUser.DateOfBirth, validUserDto.DateOfBirth);
            Assert.Equal(validUser.FirstName, validUserDto.FirstName);
            Assert.Equal(validUser.LastName, validUserDto.LastName);

            Assert.Equal(validUser.Address.StreetNumber, validUserDto.Address.StreetNumber);
            Assert.Equal(validUser.Address.StreetNumberSuffix, validUserDto.Address.StreetNumberSuffix);
            Assert.Equal(validUser.Address.StreetName, validUserDto.Address.StreetName);
            Assert.Equal(validUser.Address.Suburb, validUserDto.Address.Suburb);
            Assert.Equal(validUser.Address.City, validUserDto.Address.City);
            Assert.Equal(validUser.Address.PostCode, validUserDto.Address.PostCode);
        }

        [Fact]
        public void Convert_to_user_should_throw()
        {
            var invalidUserDto = GetUserDto(false);

            Assert.Throws<ValidationException>(() => User.Convert(invalidUserDto));
        }

        [Fact]
        public void Convert_to_user_should_match()
        {
            var validUserDto = GetUserDto(true);
            
            var validUser = User.Convert(validUserDto);

            Assert.Equal(validUserDto.DateOfBirth, validUser.DateOfBirth);
            Assert.Equal(validUserDto.FirstName, validUser.FirstName);
            Assert.Equal(validUserDto.LastName, validUser.LastName);

            Assert.Equal(validUserDto.Address.StreetNumber, validUser.Address.StreetNumber);
            Assert.Equal(validUserDto.Address.StreetNumberSuffix, validUser.Address.StreetNumberSuffix);
            Assert.Equal(validUserDto.Address.StreetName, validUser.Address.StreetName);
            Assert.Equal(validUserDto.Address.Suburb, validUser.Address.Suburb);
            Assert.Equal(validUserDto.Address.City, validUser.Address.City);
            Assert.Equal(validUserDto.Address.PostCode, validUser.Address.PostCode);
        }

        private static User GetUser(bool valid) => new()
        {
            DateOfBirth = valid ? DateOnly.FromDateTime(DateTime.Now) : DateOnly.FromDateTime(DateTime.Now).AddYears(1),
            FirstName = "Chris",
            LastName = "J",
            Address = new Address
            {
                City = "Wellington",
                StreetName = "The Terrace",
                StreetNumber = 1,
                StreetNumberSuffix = "A",
                Suburb = "Wellington Central",
                PostCode = "6011"
            }
        };

        private static UserDTO GetUserDto(bool valid) => new()
        {
            DateOfBirth = valid ? DateOnly.FromDateTime(DateTime.Now) : DateOnly.FromDateTime(DateTime.Now).AddYears(1),
            FirstName = "Chris",
            LastName = "J",
            Address = new AddressDTO
            {
                City = "Wellington",
                StreetName = "The Terrace",
                StreetNumber = 1,
                StreetNumberSuffix = "A",
                Suburb = "Wellington Central",
                PostCode = "6011"
            }
        };
    }
}
