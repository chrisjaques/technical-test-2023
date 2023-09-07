using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnicalTest2023.Models
{
    [Index(nameof(StreetNumber), nameof(StreetName), nameof(City))]
    public sealed class Address
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public required int StreetNumber { get; set; }
        public string? StreetNumberSuffix { get; set; }
        public required string StreetName { get; set; }
        public string? Suburb { get; set; }
        public required string City { get; set; }
        public string? PostCode { get; set; }
    }
}
