using System.Diagnostics;
using Broadcast_sm.Data;
using Broadcast_sm.Models;
using Broadcast_sm.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Broadcast_sm.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _userManager = userManager;
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var dbUser = await _dbContext.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();

            var broadcasts = await _dbContext.Users.Where(u => u.Id == user.Id)
                .SelectMany(u => u.ListeningTo)
                .SelectMany(u => u.Broadcasts)
                .Include(b => b.User)
                .Include(b => b.Likes)
                .OrderByDescending(b => b.Published)
                .ToListAsync();

            var viewModel = new HomeIndexViewModel()
            {
                Broadcasts = broadcasts
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Broadcast(HomeBroadcastViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            // Check if the post is empty (no message and no image) 8.
            if (string.IsNullOrWhiteSpace(viewModel.Message) && viewModel.Image == null)
            {
                ModelState.AddModelError(string.Empty, "You cannot post an empty broadcast.");

                // Retrieve broadcasts for the current user to re-display on the Index page
                var broadcasts = await _dbContext.Users.Where(u => u.Id == user.Id)
                    .SelectMany(u => u.ListeningTo)
                    .SelectMany(u => u.Broadcasts)
                    .Include(b => b.User)
                    .Include(b => b.Likes)
                    .OrderByDescending(b => b.Published)
                    .ToListAsync();

                var indexViewModel = new HomeIndexViewModel
                {
                    Broadcasts = broadcasts
                };

                return View("Index", indexViewModel);
            } /*8.*/

            string filename = null; // 2.

            if (viewModel.Image != null)
            {
                // Determine the upload folder path within wwwroot
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "PostImages");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                // Generate a unique filename to avoid collisions
                filename = Guid.NewGuid().ToString() + "_" + viewModel.Image.FileName;

                string filePath = Path.Combine(uploadFolder, filename);
                // Save the file to disk

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await viewModel.Image.CopyToAsync(fileStream);
                }
            }

            var broadcast = new Broadcast()
            {
                Message = viewModel.Message,
                User = user,
                Image = filename, // Store filename in Broadcast model
                Published = DateTime.Now
            }; // 2.

            _dbContext.Broadcasts.Add(broadcast);
            await _dbContext.SaveChangesAsync();

            return Redirect("/");
        }

        [HttpPost] /*5.*/
        public async Task<IActionResult> LikeBroadcast(int broadcastId)
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            // Find the broadcast by Id, including its Likes collection
            var broadcast = await _dbContext.Broadcasts
                .Include(b => b.User)
                .Include(b => b.Likes)
                .FirstOrDefaultAsync(b => b.Id == broadcastId);

            if (broadcast == null)
            {
                return NotFound();
            }

            // Add the user to the broadcast's Likes list if not already liked
            if (!broadcast.Likes.Any(u => u.Id == user.Id))
            {
                broadcast.Likes.Add(user);
                await _dbContext.SaveChangesAsync();
            }

            return Redirect("/");
        }

        [Authorize] /*6.*/
        public async Task<IActionResult> PopularPosts()
        {
            // Retrieve broadcasts along with their Likes collection
            var popularBroadcasts = await _dbContext.Broadcasts.Where(b => b.Likes.Count > 0)
                .Include(b => b.User)
                .Include(b => b.Likes) // Ensure Likes is loaded to count the likes
                .OrderByDescending(b => b.Likes.Count) // Order by like count descending
                .Take(10) // Take the top 10 posts (adjust as needed)
                .ToListAsync();

            var viewModel = new HomeIndexViewModel
            {
                Broadcasts = popularBroadcasts
            };

            return View(viewModel);
        }


    }
}
