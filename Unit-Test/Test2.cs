using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Database_Project.Models;
using Database_Project.Services;
using Database_Project.Repositories.Interfaces;
using Database_Project.Repository;

namespace Database_Project.Tests
{
    [TestClass]
    public class AdditionalBookServiceTests
    {
        private BookService _service;
        private FakeBookRepository _bookRepo;
        private FakeBookStockRepository _stockRepo;
        private FakeUnwantedCustomersRepository _unwantedCustomersRepository;

        [TestInitialize]
        public void Setup()
        {
            _bookRepo = new FakeBookRepository();
            _stockRepo = new FakeBookStockRepository();
            _unwantedCustomersRepository = new FakeUnwantedCustomersRepository();
            _service = new BookService(_bookRepo, _stockRepo, _unwantedCustomersRepository);
            
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task BorrowBookWhileHavingActiveRecord_ThrowsException()
        {
            var book = new Book { Id = 1, Title = "Test Book" };
            await _bookRepo.AddAsync(book);
            var stock = new BookStock { Id = 1, BookId = 1, BranchId = 1, Quantity = 5 };
            await _stockRepo.AddAsync(stock);
            var existingRecord = new BorrowRecord { BookId = 1, UserId = 1, ReturnDate = null };
            await _bookRepo.AddBorrowRecordAsync(existingRecord);

            var newRecord = new BorrowRecord { BookId = 1, UserId = 1 };
            await _service.AddBorrowRecordAsync(newRecord);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task BorrowAsUnwantedCustomer_ThrowsException()
        {
          
            _unwantedCustomersRepository.Add(new UnwantedCustomer { UserId = 1, IsActive = true });

            var book = new Book { Id = 1, Title = "Test Book" };
            await _bookRepo.AddAsync(book);
            var stock = new BookStock { Id = 1, BookId = 1, BranchId = 1, Quantity = 5 };
            await _stockRepo.AddAsync(stock);

            var record = new BorrowRecord { BookId = 1, UserId = 1 };
            await _service.AddBorrowRecordAsync(record);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task BorrowFromEmptyBranch_ThrowsException()
        {
            var book = new Book { Id = 1, Title = "Test Book" };
            await _bookRepo.AddAsync(book);
            var stock = new BookStock { Id = 1, BookId = 1, BranchId = 1, Quantity = 0 };
            await _stockRepo.AddAsync(stock);

            var record = new BorrowRecord { BookId = 1, UserId = 1 };
            await _service.AddBorrowRecordAsync(record);
        }

        [TestMethod]
        public async Task BorrowSameBookFromDifferentBranches_ThrowsException()
        {
            var book = new Book { Id = 1, Title = "Test Book" };
            await _bookRepo.AddAsync(book);

            var stock1 = new BookStock { Id = 1, BookId = 1, BranchId = 1, Quantity = 5 };
            var stock2 = new BookStock { Id = 2, BookId = 1, BranchId = 2, Quantity = 3 };
            await _stockRepo.AddAsync(stock1);
            await _stockRepo.AddAsync(stock2);

            var record1 = new BorrowRecord { BookId = 1, UserId = 1, BranchId = 1 };
            await _service.AddBorrowRecordAsync(record1);

            var record2 = new BorrowRecord { BookId = 1, UserId = 1, BranchId = 2 };
            await _service.AddBorrowRecordAsync(record2);

            Assert.AreEqual(1, _bookRepo.GetActiveBorrowRecordsCount(1, 1));
        }

        [TestMethod]
        public async Task ReturnBookAsUnwantedCustomer_Succeeds()
        {
            var unwantedRepo = new FakeUnwantedCustomersRepository();
            var unwantedService = new UnwantedCustomersService(unwantedRepo);
            unwantedRepo.Add(new UnwantedCustomer { UserId = 1, IsActive = true });

            var book = new Book { Id = 1, Title = "Test Book" };
            await _bookRepo.AddAsync(book);
            var stock = new BookStock { Id = 1, BookId = 1, BranchId = 1, Quantity = 5 };
            await _stockRepo.AddAsync(stock);

            var record = new BorrowRecord { BookId = 1, UserId = 1, ReturnDate = null };
            await _bookRepo.AddBorrowRecordAsync(record);

            record.ReturnDate = DateTime.Now;
            await _service.UpdateBorrowRecordAsync(record);

            var updatedRecord = await _service.GetActiveBorrowRecordAsync(1, 1);
            Assert.IsNotNull(updatedRecord.ReturnDate);
        }
    }
}