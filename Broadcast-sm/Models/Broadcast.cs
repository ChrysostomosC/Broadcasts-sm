namespace Broadcast_sm.Models
{
    public class Broadcast
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public ApplicationUser User { get; set; }

        // Added property for clarity:
        public string UserId { get; set; } /*5.*/


        public string Image { get; set; } // 3. 
        public DateTime Published { get; set; } = DateTime.Now;
        public ICollection<ApplicationUser> Likes { get; set; } = new List<ApplicationUser>(); /*5.*/
    }
}
