using System.ComponentModel.DataAnnotations.Schema;

namespace BankSystemAssessment.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
    }
}
