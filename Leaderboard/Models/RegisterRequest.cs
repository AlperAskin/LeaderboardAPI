using System.ComponentModel.DataAnnotations;

namespace Leaderboard.Models
{
    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string DeviceId { get; set; }
    }
}
