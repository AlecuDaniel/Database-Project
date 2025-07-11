﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Database_Project.Models;

namespace Database_Project.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book> GetByIdAsync(int id);
        Task<Book> GetByIdForUpdateAsync(int id);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(int id);
        Task AddBorrowRecordAsync(BorrowRecord record);
        Task UpdateBorrowRecordAsync(BorrowRecord record);
        Task<BorrowRecord> GetActiveBorrowRecordAsync(int bookId, int userId);
        Task<Book> GetBookWithStocksAsync(int id);
    }
}