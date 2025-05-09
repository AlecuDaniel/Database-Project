using Database_Project.Data;
using Database_Project.Models;
using Database_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Database_Project.Repositories
{
    public class BookStockRepository : IBookStockRepository
    {
        private readonly ApplicationDbContext _context;

        public BookStockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookStock>> GetAllAsync()
        {
            return await _context.BookStocks
                .Include(bs => bs.Book)
                .Include(bs => bs.LibraryBranch)
                .ToListAsync();
        }

        public async Task<BookStock> GetByIdAsync(int id)
        {
            return await _context.BookStocks
                .Include(bs => bs.Book)
                .Include(bs => bs.LibraryBranch)
                .FirstOrDefaultAsync(bs => bs.Id == id);
        }

        public async Task<IEnumerable<BookStock>> GetByBookIdAsync(int bookId)
        {
            return await _context.BookStocks
                .Where(bs => bs.BookId == bookId)
                .Include(bs => bs.LibraryBranch)
                .ToListAsync();
        }

        public async Task AddAsync(BookStock bookStock)
        {
            _context.BookStocks.Add(bookStock);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BookStock updated)
        {
            var existing = await _context.BookStocks.FindAsync(updated.Id);
            if (existing != null)
            {
                existing.BookId = updated.BookId;
                existing.BranchId = updated.BranchId;
                existing.Quantity = updated.Quantity;

                await _context.SaveChangesAsync();
            }
        }


        public async Task DeleteAsync(int id)
        {
            var stock = await _context.BookStocks.FindAsync(id);
            if (stock != null)
            {
                _context.BookStocks.Remove(stock);
                await _context.SaveChangesAsync();
            }
        }
    }
}
