using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace TechnicalTest2023.Models
{
    public class AddressDTO
    {
        [IntegerValidator(MinValue = 0, MaxValue = 100000, ExcludeRange = true)]
        public required int StreetNumber { get; set; }
        [MaxLength(50, ErrorMessage = "Street number suffix must be at most 50 characters")]
        public string? StreetNumberSuffix { get; set; }
        [MinLength(1, ErrorMessage = "Street name is required")]
        [MaxLength(100, ErrorMessage = "Street name must be at most 100 characters")]
        public required string StreetName { get; set; }
        [MaxLength(100, ErrorMessage = "Suburb must be at most 100 characters")]
        public string? Suburb { get; set; }
        [MinLength(1, ErrorMessage = "City is required")]
        [MaxLength(100, ErrorMessage = "City must be at most 100 characters")]
        public required string City { get; set; }
        [MaxLength(10, ErrorMessage = "Postal code must be at most 10 characters")]
        public string? PostCode { get; set; }
    }
}
