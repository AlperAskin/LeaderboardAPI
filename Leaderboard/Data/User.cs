using Microsoft.AspNetCore.Identity;

namespace Leaderboard.Data
{
    public class User : IdentityUser
    {
        public string? DeviceId { get; set; }

    }
}
