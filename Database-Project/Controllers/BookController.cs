using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Database_Project.Models;
using Database_Project.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace Database_Project.Controllers
{
    [Authorize(Roles = "Librarian,Admin")]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IImageService _imageService;
        private readonly ILogger<BookController> _logger;

        public BookController(
            IBookService bookService,
            IImageService imageService,
            ILogger<BookController> logger)
        {
            _bookService = bookService;
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

                    // Update properties
                    existingBook.Title = book.Title;
                    existingBook.ISBN = book.ISBN;
                    existingBook.Authors = book.Authors;
                    existingBook.Publisher = book.Publisher;
                    existingBook.Description = book.Description;

                    // Handle image upload
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Delete old image if exists
                        _imageService.DeleteImage(existingBook.ImagePath);
                        // Upload new image
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
                    // Delete associated image
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
    }
}