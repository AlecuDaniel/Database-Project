using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database_Project.Models;
using Database_Project.Data;

namespace Database_Project.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books
                .Include(b => b.BookStocks)
                .ThenInclude(bs => bs.LibraryBranch)
                .Include(b => b.BorrowRecords)
                .ToListAsync();
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            return await _context.Books
                .Include(b => b.BookStocks)
                .ThenInclude(bs => bs.LibraryBranch)
                .Include(b => b.BorrowRecords)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book> GetByIdForUpdateAsync(int id)
        {
            return await _context.Books
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task AddAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddBorrowRecordAsync(BorrowRecord record)
        {
            _context.BorrowRecords.Add(record);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBorrowRecordAsync(BorrowRecord record)
        {
            _context.BorrowRecords.Update(record);
            await _context.SaveChangesAsync();
        }

        public async Task<BorrowRecord> GetActiveBorrowRecordAsync(int bookId, int userId)
        {
            return await _context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.LibraryBranch)
                .Where(br => br.BookId == bookId &&
                             br.UserId == userId &&
                             br.ReturnDate == null)
                .FirstOrDefaultAsync();
        }

        public async Task<Book> GetBookWithStocksAsync(int id)
        {
            return await _context.Books
                .Include(b => b.BookStocks)
                .ThenInclude(bs => bs.LibraryBranch)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}