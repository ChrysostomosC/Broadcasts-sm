using Broadcast_sm.Models;

namespace Broadcast_sm.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<Broadcast> Broadcasts { get; set; } = new List<Broadcast>(); /* 5. new List<Broadcast>(); */ 
    }
}
