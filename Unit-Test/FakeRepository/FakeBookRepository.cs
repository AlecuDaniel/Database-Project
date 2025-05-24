using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Database_Project.Models;
using Database_Project.Repositories.Interfaces;
using Database_Project.Services;
using Database_Project.Repositories;

// --- Fake Repositories ---

public class FakeBookRepository : IBookRepository
{
    private readonly Dictionary<int, Book> _storage = new();
    private readonly Dictionary<(int bookId, int userId), BorrowRecord> _borrowRecords = new();

    public Task AddAsync(Book book) { _storage[book.Id] = book; return Task.CompletedTask; }
    public Task AddBorrowRecordAsync(BorrowRecord record) { _borrowRecords[(record.BookId, record.UserId)] = record; return Task.CompletedTask; }
    public Task DeleteAsync(int id) { _storage.Remove(id); return Task.CompletedTask; }
    public Task<Book> GetByIdAsync(int id) => Task.FromResult(_storage.ContainsKey(id) ? _storage[id] : null);
    public Task<Book> GetByIdForUpdateAsync(int id) => GetByIdAsync(id);
    public Task<IEnumerable<Book>> GetAllAsync() => Task.FromResult<IEnumerable<Book>>(_storage.Values);
    public Task UpdateAsync(Book book) { _storage[book.Id] = book; return Task.CompletedTask; }
    public Task UpdateBorrowRecordAsync(BorrowRecord record) { _borrowRecords[(record.BookId, record.UserId)] = record; return Task.CompletedTask; }
    public Task<BorrowRecord> GetActiveBorrowRecordAsync(int bookId, int userId)
    {
        _borrowRecords.TryGetValue((bookId, userId), out var record);
        return Task.FromResult(record);
    }
    public Task<Book> GetBookWithStocksAsync(int id) => GetByIdAsync(id);
}