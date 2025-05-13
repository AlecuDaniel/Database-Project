using System.Collections.Generic;
using System.Threading.Tasks;
using Database_Project.Models;
using Database_Project.Repositories;
using Database_Project.Repositories.Interfaces;

namespace Database_Project.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookStockRepository _bookStockRepository;

        public BookService(IBookRepository bookRepository, IBookStockRepository bookStockRepository)
        {
            _bookRepository = bookRepository;
            _bookStockRepository = bookStockRepository;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task<Book> GetBookByIdForUpdateAsync(int id)
        {
            return await _bookRepository.GetByIdForUpdateAsync(id);
        }

        public async Task AddBookAsync(Book book)
        {
            await _bookRepository.AddAsync(book);
        }

        public async Task UpdateBookAsync(Book book)
        {
            await _bookRepository.UpdateAsync(book);
        }

        public async Task DeleteBookAsync(int id)
        {
            await _bookRepository.DeleteAsync(id);
        }

        public async Task<BookStock> GetBookStockAsync(int bookId, int branchId)
        {
            return await _bookStockRepository.GetByBookAndBranchAsync(bookId, branchId);
        }

        public async Task UpdateBookStockAsync(BookStock stock)
        {
            await _bookStockRepository.UpdateAsync(stock);
        }

        public async Task AddBorrowRecordAsync(BorrowRecord record)
        {
            await _bookRepository.AddBorrowRecordAsync(record);
        }

        public async Task UpdateBorrowRecordAsync(BorrowRecord record)
        {
            await _bookRepository.UpdateBorrowRecordAsync(record);
        }

        public async Task<BorrowRecord> GetActiveBorrowRecordAsync(int bookId, int userId)
        {
            return await _bookRepository.GetActiveBorrowRecordAsync(bookId, userId);
        }

        public async Task<Book> GetBookWithStocksAsync(int id)
        {
            return await _bookRepository.GetBookWithStocksAsync(id);
        }
    }
}