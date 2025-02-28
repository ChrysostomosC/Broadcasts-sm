using Broadcast_sm.Models;

namespace Broadcast_sm.ViewModels
{
    public class UsersShowUserViewModel
    {
        public ApplicationUser User { get; set; }
        public List<Broadcast> Broadcasts { get; set; }
    }
}
