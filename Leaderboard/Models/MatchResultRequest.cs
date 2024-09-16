using System.ComponentModel.DataAnnotations;

namespace Leaderboard.Models
{
    public class MatchResultRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public int Score { get; set; }
        [Required]
        public int Experience { get; set; }
        public int Trophy {  get; set; }
    }
}
