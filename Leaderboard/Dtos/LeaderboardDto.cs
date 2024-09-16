namespace Leaderboard.Dtos
{
    public class LeaderboardDto
    {
        public string Username { get; set; }
        public int Experience { get; set; }
        public int Score { get; set; }
        public int TrophyCount { get; set; }
        public int? Rank { get; set; }
    }
}
