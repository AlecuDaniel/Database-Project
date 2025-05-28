using System.Collections.Generic;
using System.Threading.Tasks;
using Database_Project.Models;
using Database_Project.Repositories;
using Database_Project.Repositories.Interfaces;
using Database_Project.Repository;


namespace Database_Project.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookStockRepository _bookStockRepository;
        private readonly IUnwantedCustomersRepository _unwantedCustomersRepository;

        public BookService(IBookRepository bookRepository, IBookStockRepository bookStockRepository, IUnwantedCustomersRepository unwantedCustomersRepository )
        {
            _bookRepository = bookRepository;
            _bookStockRepository = bookStockRepository;
            _unwantedCustomersRepository = unwantedCustomersRepository;
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
            
            if (_unwantedCustomersRepository.Exists(record.UserId))
            {
                var unwanted = _unwantedCustomersRepository.GetById(record.UserId);
                if (unwanted?.IsActive == true)
                {
                    throw new InvalidOperationException("User is unwanted and cannot borrow books.");
                }
            }

            var stock = await _bookStockRepository.GetByBookAndBranchAsync(record.BookId, record.BranchId);
            if (stock == null || stock.Quantity <= 0)
            {
                throw new InvalidOperationException("Book is not available in this branch.");
            }

            
            var activeRecord = await GetActiveBorrowRecordAsync(record.BookId, record.UserId);
            if (activeRecord != null)
            {
                throw new InvalidOperationException("User already has an active borrow record for this book.");
            }

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