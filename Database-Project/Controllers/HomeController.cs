using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Database_Project.Models;
using Database_Project.Services;
using System.Threading.Tasks;

namespace Database_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookService _bookService;

        public HomeController(
            ILogger<HomeController> logger,
            IBookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }

        public async Task<IActionResult> Index()
        {
            // Creează un model special care combină ambele tipuri de cărți
            var model = new HomeViewModel
            {
                FeaturedBooks = GetHardcodedBooks(),
                AllBooks = await _bookService.GetAllBooksAsync()
            };

            return View(model);
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

        private List<FeaturedBook> GetHardcodedBooks()
        {
            return new List<FeaturedBook>
            {
 /*               new FeaturedBook
                {
                    Title = "Harry Potter and the Order of the Phoenix",
                    ImagePath = "/img/harry-potter-and-the-order-of-the-phoenix-cover-image-692x1024.jpeg",
                    Author = "J.K. Rowling",
                    Details = "800+ pages"
                },
                new FeaturedBook
                {
                    Title = "Maitreyi",
                    ImagePath = "/img/maitreyi.jpg",
                    Author = "Mircea Eliade",
                    Details = "256 pages"
                },*/
                // Adaugă celelalte cărți hardcodate aici
            };
        }
    }

    // Modele suplimentare
    public class HomeViewModel
    {
        public List<FeaturedBook> FeaturedBooks { get; set; }
        public IEnumerable<Book> AllBooks { get; set; }
    }

    public class FeaturedBook
    {
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string Author { get; set; }
        public string Details { get; set; }
    }
}