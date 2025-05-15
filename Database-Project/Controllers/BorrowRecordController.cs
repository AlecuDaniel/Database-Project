using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database_Project.Data;
using Database_Project.Models;
using Microsoft.AspNetCore.Authorization;

namespace Database_Project.Controllers
{
    [Authorize(Roles = "Librarian,Admin")]
    public class BorrowRecordController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BorrowRecordController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Index()
        {
            var borrowRecords = await _context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.User)
                .Include(br => br.LibraryBranch)
                .OrderByDescending(br => br.BorrowDate)
                .ToListAsync();

            return View(borrowRecords);
        }
    }
}
