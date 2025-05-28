using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Linq;

using Database_Project.Controllers;
using Database_Project.Models;
using Database_Project.Services;
using Database_Project.Services.Interfaces;
using Database_Project.Repositories.Interfaces;
using Database_Project.Repositories;

namespace Database_Project.Tests
{
    [TestClass]
    public class BookServiceTests
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
        public async Task GetBookByIdAsync_ReturnsBook()
        {
            var book = new Book { Id = 1, Title = "Test" };
            await _bookRepo.AddAsync(book);

            var result = await _service.GetBookByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("Test", result.Title);
        }

        [TestMethod]
        public async Task AddBookAsync_AddsBook()
        {
            var book = new Book { Id = 10, Title = "New Book" };

            await _service.AddBookAsync(book);

            var fetched = await _bookRepo.GetByIdAsync(10);
            Assert.IsNotNull(fetched);
            Assert.AreEqual("New Book", fetched.Title);
        }

        [TestMethod]
        public async Task UpdateBookAsync_UpdatesBook()
        {
            var book = new Book { Id = 1, Title = "Old Title" };
            await _bookRepo.AddAsync(book);

            book.Title = "New Title";
            await _service.UpdateBookAsync(book);

            var updated = await _bookRepo.GetByIdAsync(1);
            Assert.AreEqual("New Title", updated.Title);
        }

        [TestMethod]
        public async Task DeleteBookAsync_RemovesBook()
        {
            var book = new Book { Id = 5, Title = "ToDelete" };
            await _bookRepo.AddAsync(book);

            await _service.DeleteBookAsync(5);

            var deleted = await _bookRepo.GetByIdAsync(5);
            Assert.IsNull(deleted);
        }

        [TestMethod]   // Interesanta
        public async Task GetBookStockAsync_ReturnsStock()
        {
            var stock = new BookStock { Id = 1, BookId = 3, BranchId = 4, Quantity = 10 };
            await _stockRepo.AddAsync(stock);

            var result = await _service.GetBookStockAsync(3, 4);

            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.Quantity);
        }

        [TestMethod]
        public async Task UpdateBookStockAsync_UpdatesStock()
        {
            var stock = new BookStock { Id = 1, BookId = 3, BranchId = 4, Quantity = 10 };
            await _stockRepo.AddAsync(stock);

            stock.Quantity = 20;
            await _service.UpdateBookStockAsync(stock);

            var updated = await _stockRepo.GetByBookAndBranchAsync(3, 4);
            Assert.AreEqual(20, updated.Quantity);
        }

        [TestMethod]
        public async Task AddBorrowRecordAsync_AddsRecord()
        {
            var record = new BorrowRecord { BookId = 1, UserId = 2 };

            await _service.AddBorrowRecordAsync(record);

            var fetched = await _bookRepo.GetActiveBorrowRecordAsync(1, 2);
            Assert.IsNotNull(fetched);
        }

        [TestMethod]
        public async Task UpdateBorrowRecordAsync_UpdatesRecord()
        {
            var record = new BorrowRecord { BookId = 1, UserId = 2, ReturnDate = null };
            await _service.AddBorrowRecordAsync(record);

            record.ReturnDate = DateTime.Now;
            await _service.UpdateBorrowRecordAsync(record);

            var updated = await _bookRepo.GetActiveBorrowRecordAsync(1, 2);
            Assert.IsNotNull(updated.ReturnDate);
        }

        [TestMethod]
        public async Task GetActiveBorrowRecordAsync_ReturnsRecord()
        {
            var record = new BorrowRecord { BookId = 5, UserId = 10 };
            await _service.AddBorrowRecordAsync(record);

            var result = await _service.GetActiveBorrowRecordAsync(5, 10);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetBookWithStocksAsync_ReturnsBook()
        {
            var book = new Book { Id = 7, Title = "With Stocks" };
            await _bookRepo.AddAsync(book);

            var result = await _service.GetBookWithStocksAsync(7);
            Assert.IsNotNull(result);
            Assert.AreEqual("With Stocks", result.Title);
        }
    }

    [TestClass]
    public class BookStockServiceTests
    {
        private BookStockService _service;
        private FakeBookStockRepository _repo;

        [TestInitialize]
        public void Setup()
        {
            _repo = new FakeBookStockRepository();
            _service = new BookStockService(_repo);
        }

        [TestMethod]
        public async Task AddAsync_AddsStock()
        {
            var stock = new BookStock { Id = 1, BookId = 1, BranchId = 1, Quantity = 5 };
            await _service.AddAsync(stock);

            var fetched = await _repo.GetByBookAndBranchAsync(1, 1);
            Assert.IsNotNull(fetched);
        }

        [TestMethod]
        public async Task UpdateAsync_UpdatesStock()
        {
            var stock = new BookStock { Id = 1, BookId = 1, BranchId = 1, Quantity = 10 };
            await _repo.AddAsync(stock);

            stock.Quantity = 20;
            await _service.UpdateAsync(stock);

            var updated = await _repo.GetByIdAsync(1);
            Assert.AreEqual(20, updated.Quantity);
        }

        [TestMethod]
        public async Task DeleteAsync_DeletesStock()
        {
            var stock = new BookStock { Id = 1, BookId = 1, BranchId = 1 };
            await _repo.AddAsync(stock);

            await _service.DeleteAsync(1);

            var deleted = await _repo.GetByIdAsync(1);
            Assert.IsNull(deleted);
        }
    }

    [TestClass]
    public class BranchServiceTests
    {
        private BranchService _service;
        private FakeGenericRepository<LibraryBranch> _repo;

        [TestInitialize]
        public void Setup()
        {
            _repo = new FakeGenericRepository<LibraryBranch>();
            _service = new BranchService(_repo);
        }

        [TestMethod]
        public async Task AddBranchAsync_AddsBranch()
        {
            var branch = new LibraryBranch { Id = 0, Name = "Central" };
            await _service.AddBranchAsync(branch);

            var fetched = await _repo.GetByIdAsync(branch.Id);
            Assert.IsNotNull(fetched);
            Assert.AreEqual("Central", fetched.Name);
        }

     
        [TestMethod]
        public async Task UpdateBranchAsync_UpdatesBranch()
        {
            var branch = new LibraryBranch { Name = "Old" };
            await _repo.AddAsync(branch);

            branch.Name = "New";
            await _service.UpdateBranchAsync(branch);

            var updated = await _repo.GetByIdAsync(branch.Id);
            Assert.AreEqual("New", updated.Name);
        }

        [TestMethod]
        public async Task DeleteBranchAsync_DeletesBranch()
        {
            var branch = new LibraryBranch { Name = "ToDelete" };
            await _repo.AddAsync(branch);

            await _service.DeleteBranchAsync(branch);

            var deleted = await _repo.GetByIdAsync(branch.Id);
            Assert.IsNull(deleted);
        }
    }

    [TestClass]
    public class UnwantedCustomersServiceTests
    {
        private UnwantedCustomersService _service;
        private FakeUnwantedCustomersRepository _repo;

        [TestInitialize]
        public void Setup()
        {
            _repo = new FakeUnwantedCustomersRepository();
            _service = new UnwantedCustomersService(_repo);
        }

        

        [TestMethod]
        public void GetUnwantedCustomer_ReturnsCustomer()
        {
            var cust = new UnwantedCustomer { UserId = 10, IsActive = true };
            _repo.Add(cust);

            var result = _service.GetUnwantedCustomer(10);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IsUserUnwanted_ReturnsTrueIfActive()
        {
            _repo.Add(new UnwantedCustomer { UserId = 1, IsActive = true });
            var result = _service.IsUserUnwanted(1);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsUserUnwanted_ReturnsFalseIfNotActive()
        {
            _repo.Add(new UnwantedCustomer { UserId = 1, IsActive = false });
            var result = _service.IsUserUnwanted(1);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddUnwantedCustomer_ThrowsIfNull()
        {
            _service.AddUnwantedCustomer(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddUnwantedCustomer_ThrowsIfExists()
        {
            var cust = new UnwantedCustomer { UserId = 1 };
            _repo.Add(cust);
            _service.AddUnwantedCustomer(cust);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateUnwantedCustomer_ThrowsIfNull()
        {
            _service.UpdateUnwantedCustomer(null);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void UpdateUnwantedCustomer_ThrowsIfNotExists()
        {
            _service.UpdateUnwantedCustomer(new UnwantedCustomer { UserId = 999 });
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void RemoveUnwantedCustomer_ThrowsIfNotExists()
        {
            _service.RemoveUnwantedCustomer(999);
        }

        [TestMethod]
        public void GetPotentialUnwantedCustomers_ReturnsUsers()
        {
            _repo.AddPotentialUser(new User { Id = 1, UserName = "User1" });
            var result = _service.GetPotentialUnwantedCustomers();

            Assert.AreEqual(1, ((List<User>)result).Count);
        }
    }
}