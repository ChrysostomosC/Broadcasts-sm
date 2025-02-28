using Broadcast_sm.Models;
using Broadcast_sm.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Broadcast_sm.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileController(UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment; // 2. 
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var viewModel = new ProfileIndexViewModel()
            {
                Name = user.Name ?? "",
                ProfilePic = user.ProfilePic // 2. 
            };
            
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProfileIndexViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            // 3. 
            bool nameExists = _userManager.Users.Any(u => u.Name == viewModel.Name && u.Id != user.Id);

            if (nameExists)
            {
                // Show error message if the name is taken
                TempData["error"] = "This name is already taken. Please choose another one.";
                return RedirectToAction("Index");
            } // 3.

            user.Name = viewModel.Name;

            string filename = ""; // 2.

            if (viewModel.Photo != null)
            {
                // Determine the upload folder path within wwwroot
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");

                // Generate a unique filename to avoid collisions
                filename = Guid.NewGuid().ToString() + "_" + viewModel.Photo.FileName;
                
                string filePath = Path.Combine(uploadFolder, filename);
                // Save the file to disk
                
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    viewModel.Photo.CopyTo(fileStream);
                }

                // Store only the relative path in the database
                user.ProfilePic = "/Images/" + filename;
            } // 2.

            await _userManager.UpdateAsync(user);
            ViewBag.success = "Record Added"; // 2. 
            TempData["success"] = "Profile updated successfully."; // 3.

            return Redirect("/");
        }
    }
}
