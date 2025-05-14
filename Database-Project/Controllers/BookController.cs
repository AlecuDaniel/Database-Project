using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Database_Project.Models;
using Database_Project.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Database_Project.Services.Interfaces;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Database_Project.Controllers
{
    [Authorize(Roles = "Librarian,Admin")]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IBookStockService _bookStockService;
        private readonly IImageService _imageService;
        private readonly ILogger<BookController> _logger;

        public BookController(
            IBookService bookService,
            IBookStockService bookStockService,
            IImageService imageService,
            ILogger<BookController> logger)
        {
            _bookService = bookService;
            _bookStockService = bookStockService;
            _imageService = imageService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetAllBooksAsync();
            return View(books);
        }

        public async Task<IActionResult> Details(int id)
        {
            var book = await _bookService.GetBookByIdForUpdateAsync(id);
            if (book == null)
                return NotFound();

            book = await _bookService.GetBookWithStocksAsync(id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        book.ImagePath = await _imageService.UploadImageAsync(imageFile);
                    }

                    await _bookService.AddBookAsync(book);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating book");
                    ModelState.AddModelError("", "An error occurred while creating the book.");
                }
            }
            return View(book);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var book = await _bookService.GetBookByIdForUpdateAsync(id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book, IFormFile imageFile)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingBook = await _bookService.GetBookByIdForUpdateAsync(id);
                    if (existingBook == null)
                    {
                        return NotFound();
                    }

                    
                    existingBook.Title = book.Title;
                    existingBook.ISBN = book.ISBN;
                    existingBook.Authors = book.Authors;
                    existingBook.Publisher = book.Publisher;
                    existingBook.Description = book.Description;

                    
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        
                        _imageService.DeleteImage(existingBook.ImagePath);
                        
                        existingBook.ImagePath = await _imageService.UploadImageAsync(imageFile);
                    }

                    await _bookService.UpdateBookAsync(existingBook);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating book with ID {BookId}", id);
                    ModelState.AddModelError("", "An error occurred while updating the book.");
                }
            }
            return View(book);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookService.GetBookByIdForUpdateAsync(id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIdForUpdateAsync(id);
                if (book != null)
                {
                    
                    _imageService.DeleteImage(book.ImagePath);
                    await _bookService.DeleteBookAsync(id);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book with ID {BookId}", id);
                return RedirectToAction(nameof(Delete), new { id, error = true });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Borrow(int id, int branchId)
        {
            try
            {
                var stock = await _bookStockService.GetByBookAndBranchAsync(id, branchId);
                if (stock == null || stock.Quantity <= 0)
                {
                    TempData["ErrorMessage"] = "This book is not available at the selected branch.";
                    return RedirectToAction(nameof(Details), new { id });
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "Unable to identify user.";
                    return RedirectToAction(nameof(Details), new { id });
                }

                var borrowRecord = new BorrowRecord
                {
                    BookId = id,
                    UserId = int.Parse(userId),
                    BranchId = branchId,
                    BorrowDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(14),
                    IsOverdue = false
                };

                stock.Quantity--;
                await _bookStockService.UpdateAsync(stock);
                await _bookService.AddBorrowRecordAsync(borrowRecord);

                TempData["SuccessMessage"] = $"Book borrowed successfully! Due date: {borrowRecord.DueDate:MMMM dd, yyyy}";
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error borrowing book with ID {BookId}", id);
                TempData["ErrorMessage"] = "An error occurred while borrowing the book.";
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int id)
        {
            try
            {
                
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var borrowRecord = await _bookService.GetActiveBorrowRecordAsync(id, userId);

                if (borrowRecord == null)
                {
                    TempData["ErrorMessage"] = "No active borrow record found for this book.";
                    return RedirectToAction(nameof(Details), new { id });
                }

                
                borrowRecord.ReturnDate = DateTime.Now;
                if (borrowRecord.ReturnDate > borrowRecord.DueDate)
                {
                    borrowRecord.IsOverdue = true;
                }

                
                var stocks = await _bookStockService.GetByBookIdAsync(id);
                var stock = stocks.FirstOrDefault(bs => bs.BranchId == borrowRecord.BranchId);
                if (stock != null)
                {
                    stock.Quantity++;
                    await _bookStockService.UpdateAsync(stock);
                }

                await _bookService.UpdateBorrowRecordAsync(borrowRecord);

                TempData["SuccessMessage"] = "Book returned successfully!";
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error returning book with ID {BookId}", id);
                TempData["ErrorMessage"] = "An error occurred while returning the book.";
                return RedirectToAction(nameof(Details), new { id });
            }
        }
    }
}