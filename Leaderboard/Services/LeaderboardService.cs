using Leaderboard.Data;
using Leaderboard.Dtos;
using LeaderboardAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;


namespace Leaderboard.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _cache;

        private const string LeaderboardCacheKey = "LeaderboardCache";
        public LeaderboardService(
            ApplicationDbContext context,
            IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<List<LeaderboardDto>?> GetLeaderboardAsync()
        {
            
            var cachedLeaderboard = await _cache.GetStringAsync(LeaderboardCacheKey);
            if (!string.IsNullOrEmpty(cachedLeaderboard))
            {                
                return JsonConvert.DeserializeObject<List<LeaderboardDto>>(cachedLeaderboard);
            }            
            var leaderboard = await _context.Leaderboard
                .OrderByDescending(l => l.Score)
                .ThenByDescending(l => l.TrophyCount)
                .ThenByDescending(l => l.Experience)
                .ToListAsync();

            int rank = 1;
            var leaderboardDtos = leaderboard.Select(list => new LeaderboardDto
            {
                Username = list.Username,
                Score = list.Score,
                Experience = list.Experience,
                TrophyCount = list.TrophyCount,
                Rank = rank++
            }).ToList();
            
            var serializedLeaderboard = JsonConvert.SerializeObject(leaderboardDtos);
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
            };

            await _cache.SetStringAsync(LeaderboardCacheKey, serializedLeaderboard, cacheOptions);

            return leaderboardDtos;
        }

        public async Task UpdateAffectedRanksAsync(int oldScore, Data.Leaderboard result)
        {
            var affectedPlayers = await _context.Leaderboard!
            .Where(l => l.Score <= result.Score && l.Score > oldScore)
            .OrderByDescending(l => l.Score)
            .ToListAsync();

            if (affectedPlayers.Count > 0)
            {
                foreach (var affectedPlayer in affectedPlayers)
                {
                    affectedPlayer.Rank++;
                }
            }

            var playerEntry = await _context.Leaderboard!.FindAsync(result.UserId);
            if (playerEntry != null)
            {
                playerEntry.Rank = affectedPlayers.First().Rank - 1;
            }

            await _context.SaveChangesAsync();
            await _cache.RemoveAsync("LeaderboardCache");
        }


    }
}
