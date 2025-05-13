using System.Collections.Generic;
using System.Threading.Tasks;
using Database_Project.Models;

namespace Database_Project.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(int id);
        Task<Book> GetBookByIdForUpdateAsync(int id);
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(int id);
        Task<BookStock> GetBookStockAsync(int bookId, int branchId);
        Task UpdateBookStockAsync(BookStock stock);
        Task AddBorrowRecordAsync(BorrowRecord record);
        Task UpdateBorrowRecordAsync(BorrowRecord record);
        Task<BorrowRecord> GetActiveBorrowRecordAsync(int bookId, int userId);
        Task<Book> GetBookWithStocksAsync(int id);
    }
}