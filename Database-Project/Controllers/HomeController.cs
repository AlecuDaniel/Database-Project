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



        public async Task<IActionResult> Index(string searchString)
        {
            var allBooks = await _bookService.GetAllBooksAsync();
                       
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower(); 

                allBooks = allBooks.Where(b =>
                 
                    (b.Title != null && b.Title.ToLower().Contains(searchString)) ||

                    (b.Authors != null && b.Authors.Any(a => a != null && a.ToString().ToLower().Contains(searchString))) ||

                    
                    (b.Publisher != null && b.Publisher.ToLower().Contains(searchString))
                ).ToList();
            }
            var model = new HomeViewModel
            {
                AllBooks = allBooks
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

    }

    
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