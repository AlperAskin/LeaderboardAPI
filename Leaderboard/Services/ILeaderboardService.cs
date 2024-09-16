using Leaderboard.Dtos;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LeaderboardAPI.Services
{
    public interface ILeaderboardService
    {
        Task<List<LeaderboardDto>?> GetLeaderboardAsync();
        Task UpdateAffectedRanksAsync(int oldScore, Leaderboard.Data.Leaderboard result);
    }
}
