using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database_Project.Models;
using Database_Project.Repositories.Interfaces;

public class FakeBookStockRepository : IBookStockRepository
{
    public List<BookStock> Stocks { get; set; } = new List<BookStock>();

    public Task<IEnumerable<BookStock>> GetAllAsync()
    {
        return Task.FromResult(Stocks.AsEnumerable());
    }

    public Task<BookStock> GetByIdAsync(int id)
    {
        return Task.FromResult(Stocks.FirstOrDefault(s => s.Id == id));
    }

    public Task AddAsync(BookStock bookStock)
    {
        Stocks.Add(bookStock);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(BookStock bookStock)
    {
        var index = Stocks.FindIndex(s => s.Id == bookStock.Id);
        if (index >= 0)
        {
            Stocks[index] = bookStock;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Stocks.RemoveAll(s => s.Id == id);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<BookStock>> GetByBookIdAsync(int bookId)
    {
        return Task.FromResult(Stocks.Where(s => s.BookId == bookId).AsEnumerable());
    }

    public Task<BookStock> GetByBookAndBranchAsync(int bookId, int branchId)
    {
        return Task.FromResult(Stocks.FirstOrDefault(s => s.BookId == bookId && s.BranchId == branchId));
    }

    public Task<Book> GetBookWithStocksAsync(int id)
    {
        // Minimal fake logic — assuming each BookStock includes a Book object
        var stock = Stocks.FirstOrDefault(s => s.BookId == id);
        return Task.FromResult(stock?.Book);
    }
}
