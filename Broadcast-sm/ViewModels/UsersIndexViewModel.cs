using Broadcast_sm.Models;

namespace Broadcast_sm.ViewModels
{
    public class UsersIndexViewModel
    {
        public string Search { get; set; }
        public List<ApplicationUser> Result { get; set; } = new List<ApplicationUser>();
    }
}
