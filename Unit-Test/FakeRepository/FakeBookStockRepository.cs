using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database_Project.Models;
using Database_Project.Repositories.Interfaces;

public class FakeBookStockRepository : IBookStockRepository
{
    private readonly Dictionary<(int bookId, int branchId), BookStock> _stocks = new();

    public Task AddAsync(BookStock bookStock)
    {
        _stocks[(bookStock.BookId, bookStock.BranchId)] = bookStock;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var toRemove = new List<(int, int)>();
        foreach (var kvp in _stocks)
            if (kvp.Value.Id == id) toRemove.Add(kvp.Key);

        foreach (var key in toRemove)
            _stocks.Remove(key);

        return Task.CompletedTask;
    }

    public Task<IEnumerable<BookStock>> GetAllAsync() => Task.FromResult<IEnumerable<BookStock>>(_stocks.Values);

    public Task<BookStock> GetByBookAndBranchAsync(int bookId, int branchId) =>
        Task.FromResult(_stocks.TryGetValue((bookId, branchId), out var stock) ? stock : null);

    public Task<BookStock> GetByIdAsync(int id)
    {
        foreach (var stock in _stocks.Values)
            if (stock.Id == id) return Task.FromResult(stock);
        return Task.FromResult<BookStock>(null);
    }

    public Task<IEnumerable<BookStock>> GetByBookIdAsync(int bookId)
    {
        var list = _stocks.Values.Where(stock => stock.BookId == bookId).ToList();
        return Task.FromResult<IEnumerable<BookStock>>(list);
    }

    public Task UpdateAsync(BookStock bookStock)
    {
        _stocks[(bookStock.BookId, bookStock.BranchId)] = bookStock;
        return Task.CompletedTask;
    }

    public Task<Book> GetBookWithStocksAsync(int bookId)
    {
        // Create a dummy Book with the matching bookId and add all related stocks
        var bookStocks = _stocks.Values.Where(s => s.BookId == bookId).ToList();

        if (!bookStocks.Any())
        {
            return Task.FromResult<Book>(null);
        }

        var book = new Book
        {
            Id = bookId,
            Title = $"Book {bookId} (Fake)",
            BookStocks = bookStocks
        };

        return Task.FromResult(book);
    }
}
