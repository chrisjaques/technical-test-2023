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
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }
        public required Address Address { get; set; }
    }
}
