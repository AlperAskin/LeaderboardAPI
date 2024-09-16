using Leaderboard.Data;
using Leaderboard.Models;
using LeaderboardAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Security.Claims;


namespace Leaderboard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILeaderboardService _leaderboardService;

        public LeaderboardController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            ILeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
            _userManager = userManager;
            _context = context;             
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitMatchResult([FromBody] MatchResultRequest matchresult)
        {            
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {               
                var player = await _context.Users.FirstOrDefaultAsync(p => p.UserName == matchresult.Username);
                if (player is null)
                {
                    return NotFound("Player not found");
                }                
                var leaderboardEntry = await _context.Leaderboard.FindAsync(player.Id);

                if (leaderboardEntry is null)
                {                   
                    var newEntry = new Data.Leaderboard
                    {
                        UserId = player.Id,
                        Username = matchresult.Username,
                        Experience = matchresult.Experience,
                        Score = matchresult.Score
                    };

                    await _context.Leaderboard.AddAsync(newEntry);
                    await _context.SaveChangesAsync();                    
                    await _leaderboardService.UpdateAffectedRanksAsync(0, newEntry);                     
                    await transaction.CommitAsync();

                    return Ok(newEntry);
                }
                
                var oldScore = leaderboardEntry.Score;
                leaderboardEntry.Score += matchresult.Score;
                leaderboardEntry.Experience += matchresult.Experience;                
                await _leaderboardService.UpdateAffectedRanksAsync(oldScore, leaderboardEntry);
                
                await _context.SaveChangesAsync();                
                await transaction.CommitAsync();

                return Ok(leaderboardEntry);
            }
            catch (Exception ex)
            {
                // If anything fails, roll back the transaction
                await transaction.RollbackAsync();
                return StatusCode(500, "An error occurred while processing the match result: " + ex.Message);
            }
        }

        [HttpGet("GetLeaderBoard")]
        public async Task<IActionResult> GetLeaderboard()
        {   
            var leaderboard = await _leaderboardService.GetLeaderboardAsync();
            if (leaderboard is null)
            {
                return NotFound("No leaderboard entries found.");
            }
            return Ok(leaderboard);
        }       

    }
}
