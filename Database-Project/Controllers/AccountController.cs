using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;
using Database_Project.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Database_Project.ViewModels;

namespace Database_Project.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IWebHostEnvironment _environment;

        public AccountController(UserManager<User> userManager,
                                 SignInManager<User> signInManager,
                                 IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            var model = new ProfileViewModel
            {
                Bio = user.Bio,
                SelectedProfilePicture = user.ProfilePicture,
                AvailablePictures = Directory.GetFiles(Path.Combine(_environment.WebRootPath, "images/profile-pictures"))
                                             .Select(Path.GetFileName)
                                             .ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            if (!ModelState.IsValid)
            {
                model.AvailablePictures = Directory.GetFiles(Path.Combine(_environment.WebRootPath, "images/profile-pictures"))
                                                   .Select(Path.GetFileName)
                                                   .ToList();
                return View(model);
            }

            user.ProfilePicture = model.SelectedProfilePicture;
            user.Bio = model.Bio;
            await _userManager.UpdateAsync(user);

            ViewBag.Message = "Profile updated successfully.";
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            await _signInManager.SignOutAsync();
            await _userManager.DeleteAsync(user);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
