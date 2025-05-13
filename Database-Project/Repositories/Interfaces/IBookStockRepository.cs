using Database_Project.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Database_Project.Repositories.Interfaces
{
    public interface IBookStockRepository
    {
        Task<IEnumerable<BookStock>> GetAllAsync();
        Task<BookStock> GetByIdAsync(int id);
        Task AddAsync(BookStock bookStock);
        Task UpdateAsync(BookStock bookStock);
        Task DeleteAsync(int id);
        Task<IEnumerable<BookStock>> GetByBookIdAsync(int bookId);
        Task<BookStock> GetByBookAndBranchAsync(int bookId, int branchId);
        Task<Book> GetBookWithStocksAsync(int id);
    }
}