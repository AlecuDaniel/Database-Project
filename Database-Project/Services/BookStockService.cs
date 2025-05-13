using Database_Project.Models;
using Database_Project.Repositories.Interfaces;
using Database_Project.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Database_Project.Services
{
    public class BookStockService : IBookStockService
    {
        private readonly IBookStockRepository _repository;

        public BookStockService(IBookStockRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<BookStock>> GetAllAsync() => _repository.GetAllAsync();
        public Task<BookStock> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
        public Task<IEnumerable<BookStock>> GetByBookIdAsync(int bookId) => _repository.GetByBookIdAsync(bookId);
        public Task<BookStock> GetByBookAndBranchAsync(int bookId, int branchId) => _repository.GetByBookAndBranchAsync(bookId, branchId);
        public Task AddAsync(BookStock bookStock) => _repository.AddAsync(bookStock);
        public Task UpdateAsync(BookStock bookStock) => _repository.UpdateAsync(bookStock);
        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}