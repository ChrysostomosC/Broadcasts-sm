using Broadcast_sm.Data;
using Broadcast_sm.Models;
using Broadcast_sm.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Broadcast_sm.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(UsersIndexViewModel viewModel)
        {
            if (viewModel.Search != null)
            {
                var users = await _dbContext.Users.Where(u => u.Name.Contains(viewModel.Search))
                .ToListAsync();

                viewModel.Result = users;
            }

            return View(viewModel);
        }

        [Route("/Users/{id}")] // {id} is a placeholder for a dynamic value
        public async Task<IActionResult> ShowUser(string id) // id in the method signature is used to capture the dynamic value from the URL.
        {
            var broadcasts = await _dbContext.Broadcasts.Where(b => b.User.Id == id)
                .OrderByDescending(b => b.Published)
                .ToListAsync();

            var user = await _dbContext.Users.Where(u => u.Id == id).FirstOrDefaultAsync();

            var viewModel = new UsersShowUserViewModel()
            {
                Broadcasts = broadcasts,
                User = user
            };

            return View(viewModel);
        }

        [HttpPost, Route("/Users/Listen")]
        public async Task<IActionResult> ListenToUser(UsersListenToUserViewModel viewModel)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var userToListenTo = await _dbContext.Users.Where(u => u.Id == viewModel.UserId)
                .FirstOrDefaultAsync();

            loggedInUser.ListeningTo.Add(userToListenTo);

            await _userManager.UpdateAsync(loggedInUser);
            await _dbContext.SaveChangesAsync();

            return Redirect("/");
        }

        [HttpPost, Route("/Users/Ignore")]
        public async Task<IActionResult> IgnoreUser(UsersListenToUserViewModel viewModel)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var userToIgnoreTo = await _dbContext.Users.Where(u => u.Id == viewModel.UserId)
                .FirstOrDefaultAsync();

            loggedInUser.ListeningTo.Remove(userToIgnoreTo);

            await _userManager.UpdateAsync(loggedInUser);
            await _dbContext.SaveChangesAsync();

            return Redirect("/");
        }

        [Authorize]
        [Route("/Users/UsersRecommended")] /*7.*/
        public async Task<IActionResult> UsersRecommended()
        {
            // Get the currently logged-in user
            var loggedInUser = await _userManager.GetUserAsync(User);

            // Ensure the ListeningTo collection is loaded
            await _dbContext.Entry(loggedInUser).Collection(u => u.ListeningTo).LoadAsync();

            // Get the IDs of users that the logged-in user is following
            var followingIds = loggedInUser.ListeningTo.Select(u => u.Id).ToList();

            // Query for users who are NOT the logged-in user and not in their following list
            var recommendedUsers = await _dbContext.Users
                .Where(u => u.Id != loggedInUser.Id && !followingIds.Contains(u.Id))
                .Take(10) // Limit the number of recommendations
                .ToListAsync();

            // Create the view model
            var viewModel = new UsersRecommendedViewModel
            {
                UsersRecommended = recommendedUsers
            };

            return View(viewModel);
        }

    }
}
