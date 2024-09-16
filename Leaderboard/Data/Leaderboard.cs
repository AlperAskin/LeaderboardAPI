using System.ComponentModel.DataAnnotations;

namespace Leaderboard.Data
{
    public class Leaderboard
    {
        [Key]
        public string UserId { get; set; }
        [Required]
        public string Username { get; set; }
        public int Experience { get; set; } = 0;
        public int Score { get; set; } = 0;
        public int TrophyCount { get; set; } = 0;
        public int? Rank { get; set; }
    }
}
