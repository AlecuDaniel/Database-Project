using Database_Project.Data;
using Database_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Database_Project.Repository
{
    public class UnwantedCustomersRepository : IUnwantedCustomersRepository
    {
        private readonly ApplicationDbContext _context;

        public UnwantedCustomersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UnwantedCustomer> GetAll()
        {
            return _context.UnwantedCustomers
                .Include(uc => uc.User)
                .OrderByDescending(uc => uc.DateAdded)
                .ToList();
        }

        public UnwantedCustomer GetById(int userId)
        {
            return _context.UnwantedCustomers
                .Include(uc => uc.User)
                .FirstOrDefault(uc => uc.UserId == userId);
        }

        public void Add(UnwantedCustomer unwantedCustomer)
        {
            
            _context.UnwantedCustomers.Add(unwantedCustomer);
            _context.SaveChanges();
        }

        public void Update(UnwantedCustomer unwantedCustomer)
        {
            
            var existing = _context.UnwantedCustomers.Find(unwantedCustomer.UserId);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(unwantedCustomer);
                _context.SaveChanges();
            }
        }

        public void Delete(int userId)
        {
            var unwantedCustomer = _context.UnwantedCustomers.Find(userId);
            if (unwantedCustomer != null)
            {
                _context.UnwantedCustomers.Remove(unwantedCustomer);
                _context.SaveChanges();
            }
        }

        public bool Exists(int userId)
        {
            return _context.UnwantedCustomers.Any(uc => uc.UserId == userId);
        }

        public IEnumerable<User> GetPotentialUnwantedCustomers()
        {
            var existingIds = _context.UnwantedCustomers.Select(uc => uc.UserId).ToList();
            return _context.Users
                .Where(u => !existingIds.Contains(u.Id))
                .OrderBy(u => u.UserName)
                .ToList();
        }
    }
}