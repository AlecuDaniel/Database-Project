using Database_Project.Models;
using Database_Project.Repositories.Interfaces;
using Database_Project.Services;
using Database_Project.Services.Interfaces;
using Database_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Database_Project.Controllers
{
    [Authorize(Roles = "Librarian,Admin")]
    public class BookStockController : Controller
    {
        private readonly IBookStockService _service;
        private readonly IBookService _bookService;
        private readonly IGenericRepository<LibraryBranch> _branchService;

        public BookStockController(IBookStockService service, IBookService bookService, IGenericRepository<LibraryBranch> branchService)
        {
            _service = service;
            _bookService = bookService;
            _branchService = branchService;
        }

        public async Task<IActionResult> Index()
        {
            var stocks = await _service.GetAllAsync();
            return View(stocks);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new BookStockViewModel
            {
                Books = (await _bookService.GetAllBooksAsync())
                    .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Title }),
                Branches = (await _branchService.GetAllAsync())
                    .Select(br => new SelectListItem { Value = br.Id.ToString(), Text = br.Name })
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookStockViewModel model)
        {
            if (ModelState.IsValid)
            {
                var stock = new BookStock
                {
                    BookId = model.BookId,
                    BranchId = model.BranchId,
                    Quantity = model.Quantity
                };

                await _service.AddAsync(stock);
                return RedirectToAction(nameof(Index));
            }

            // Log the issue
            Console.WriteLine("Model is invalid");
            foreach (var key in ModelState.Keys)
            {
                var errors = ModelState[key].Errors;
                foreach (var error in errors)
                {
                    Console.WriteLine($"{key}: {error.ErrorMessage}");
                }
            }

            // Repopulate dropdowns if validation fails
            model.Books = (await _bookService.GetAllBooksAsync())
                .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Title });

            model.Branches = (await _branchService.GetAllAsync())
                .Select(br => new SelectListItem { Value = br.Id.ToString(), Text = br.Name });

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var stock = await _service.GetByIdAsync(id);
            if (stock == null)
                return NotFound();

            // Populate the dropdown lists with books and branches
            ViewBag.Books = new SelectList(await _bookService.GetAllBooksAsync(), "Id", "Title", stock.BookId);
            ViewBag.Branches = new SelectList(await _branchService.GetAllAsync(), "Id", "Name", stock.BranchId);

            // Return the stock object to be edited
            var viewModel = new BookStockViewModel
            {
                BookId = stock.BookId,
                BranchId = stock.BranchId,
                Quantity = stock.Quantity,
                Books = (await _bookService.GetAllBooksAsync())
         .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Title }),
                Branches = (await _branchService.GetAllAsync())
         .Select(br => new SelectListItem { Value = br.Id.ToString(), Text = br.Name })
            };

            return View(viewModel);

        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, BookStockViewModel model)

        {
            if (!ModelState.IsValid)
            {
                // Repopulate dropdowns in case of failure
                model.Books = (await _bookService.GetAllBooksAsync())
                    .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Title });

                model.Branches = (await _branchService.GetAllAsync())
                    .Select(br => new SelectListItem { Value = br.Id.ToString(), Text = br.Name });

                return View(model);
            }

            var bookStock = new BookStock
            {
                Id = id,
                BookId = model.BookId,
                BranchId = model.BranchId,
                Quantity = model.Quantity
            };

            await _service.UpdateAsync(bookStock);
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Delete(int id)
        {
            var stock = await _service.GetByIdAsync(id);
            if (stock == null) return NotFound();
            return View(stock);
        }

        
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stock = await _service.GetByIdAsync(id);
            if (stock == null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }



    }
}
