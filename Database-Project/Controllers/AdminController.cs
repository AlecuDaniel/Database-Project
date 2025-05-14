using System.Threading.Tasks;
using Database_Project.Models;
using Database_Project.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Database_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IBranchService _branchService;

        public AdminController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        
        public async Task<IActionResult> Index()
        {
            var branches = await _branchService.GetAllBranchesAsync();
            return View(branches);
        }

        
        public IActionResult AddBranch()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBranch(LibraryBranch branch)
        {
            if (ModelState.IsValid)
            {
                await _branchService.AddBranchAsync(branch);
                return RedirectToAction(nameof(Index));
            }
            return View(branch);
        }

        
        public async Task<IActionResult> Edit(int id)
        {
            var branch = await _branchService.GetBranchByIdAsync(id);
            if (branch == null)
            {
                return NotFound();
            }
            return View(branch);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LibraryBranch branch)
        {
            if (id != branch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _branchService.UpdateBranchAsync(branch);
                return RedirectToAction(nameof(Index));
            }
            return View(branch);
        }

        
        public async Task<IActionResult> Delete(int id)
        {
            var branch = await _branchService.GetBranchByIdAsync(id);
            if (branch == null)
            {
                return NotFound();
            }
            return View(branch);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var branch = await _branchService.GetBranchByIdAsync(id);
            if (branch != null)
            {
                await _branchService.DeleteBranchAsync(branch);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}