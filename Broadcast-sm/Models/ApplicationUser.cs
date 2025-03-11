using Microsoft.AspNetCore.Identity;

namespace Broadcast_sm.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string ProfilePic { get; set; } = "/Images/default-profile.png"; // 2. 
        public ICollection<Broadcast> Broadcasts { get; set; }
        public ICollection<ApplicationUser> ListeningTo { get; set; } = new List<ApplicationUser>();
    }
}
