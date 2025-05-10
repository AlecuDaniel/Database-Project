using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Database_Project.Models;
using Database_Project.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Database_Project.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IImageService _imageService;

        public BookController(IBookService bookService, IImageService imageService)
        {
            _bookService = bookService;
            _imageService = imageService;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetAllBooksAsync();
            return View(books);
        }

        public async Task<IActionResult> Details(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
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
                if (imageFile != null && imageFile.Length > 0)
                {
                    book.ImagePath = await _imageService.UploadImageAsync(imageFile);
                }

                await _bookService.AddBookAsync(book);
                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Book book, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                var existingBook = await _bookService.GetBookByIdAsync(book.Id);

                if (imageFile != null && imageFile.Length > 0)
                {
                    // Delete old image if exists
                    _imageService.DeleteImage(existingBook.ImagePath);
                    // Upload new image
                    book.ImagePath = await _imageService.UploadImageAsync(imageFile);
                }
                else
                {
                    // Keep the old image if no new one was uploaded
                    book.ImagePath = existingBook.ImagePath;
                }

                await _bookService.UpdateBookAsync(book);
                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book != null)
            {
                // Delete associated image
                _imageService.DeleteImage(book.ImagePath);
                await _bookService.DeleteBookAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}