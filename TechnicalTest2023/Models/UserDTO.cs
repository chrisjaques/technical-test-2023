﻿using System.ComponentModel.DataAnnotations;
using FluentValidation;
using TechnicalTest2023.Validators;

namespace TechnicalTest2023.Models
{
    public class UserDTO
    {
        [MinLength(1, ErrorMessage = "First name is required")]
        [MaxLength(250, ErrorMessage = "First name must be at most 250 characters")]
        public required string FirstName { get; set; }
        [MinLength(1, ErrorMessage = "Last name is required")]
        [MaxLength(250, ErrorMessage = "Last name must be at most 250 characters")]
        public required string LastName { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")] // This doesn't actually format the date correctly
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }
        public required AddressDTO Address { get; set; }

        public static UserDTO Convert(User user)
        {
            var userDto = new UserDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Address = new AddressDTO
                {
                    City = user.Address.City,
                    PostCode = user.Address.PostCode,
                    StreetName = user.Address.StreetName,
                    StreetNumber = user.Address.StreetNumber,
                    StreetNumberSuffix = user.Address.StreetNumberSuffix,
                    Suburb = user.Address.Suburb
                }
            };

            var validator = new UserDTOValidator();
            validator.ValidateAndThrow(userDto);

            return userDto;
        }
    }
}
