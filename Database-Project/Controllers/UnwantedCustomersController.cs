using Database_Project.Models;
using Database_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Database_Project.Controllers
{
    [Authorize(Roles = "Librarian,Admin")]
    public class UnwantedCustomersController : Controller
    {
        private readonly IUnwantedCustomersService _service;

        public UnwantedCustomersController(IUnwantedCustomersService service)
        {
            _service = service;
        }

        
        public IActionResult Index()
        {
            return View(_service.GetAllUnwantedCustomers());
        }

        
        public IActionResult Details(int id)
        {
            var unwantedCustomer = _service.GetUnwantedCustomer(id);
            if (unwantedCustomer == null)
            {
                return NotFound();
            }
            return View(unwantedCustomer);
        }

        
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_service.GetPotentialUnwantedCustomers(), "Id", "UserName");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("UserId,Reason,IsActive")] UnwantedCustomer unwantedCustomer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _service.AddUnwantedCustomer(unwantedCustomer);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            ViewData["UserId"] = new SelectList(_service.GetPotentialUnwantedCustomers(), "Id", "UserName", unwantedCustomer.UserId);
            return View(unwantedCustomer);
        }

        
        public IActionResult Edit(int id)
        {
            var unwantedCustomer = _service.GetUnwantedCustomer(id);
            if (unwantedCustomer == null)
            {
                return NotFound();
            }
            return View(unwantedCustomer);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("UserId,Reason,IsActive,DateAdded")] UnwantedCustomer unwantedCustomer)
        {
            if (id != unwantedCustomer.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _service.UpdateUnwantedCustomer(unwantedCustomer);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(unwantedCustomer);
        }

        
        public IActionResult Delete(int id)
        {
            var unwantedCustomer = _service.GetUnwantedCustomer(id);
            if (unwantedCustomer == null)
            {
                return NotFound();
            }
            return View(unwantedCustomer);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _service.RemoveUnwantedCustomer(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(_service.GetUnwantedCustomer(id));
            }
        }
    }
}