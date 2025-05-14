using Database_Project.Models;
using Database_Project.Repository;
using System;
using System.Collections.Generic;

namespace Database_Project.Services
{
    public class UnwantedCustomersService : IUnwantedCustomersService
    {
        private readonly IUnwantedCustomersRepository _repository;

        public UnwantedCustomersService(IUnwantedCustomersRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<UnwantedCustomer> GetAllUnwantedCustomers()
        {
            return _repository.GetAll();
        }

        public UnwantedCustomer GetUnwantedCustomer(int userId)
        {
            return _repository.GetById(userId);
        }
        public bool IsUserUnwanted(int userId)
        {
            var unwanted = _repository.GetById(userId);
            return unwanted != null && unwanted.IsActive;
        }

        public void AddUnwantedCustomer(UnwantedCustomer unwantedCustomer)
        {
            if (unwantedCustomer == null)
                throw new ArgumentNullException(nameof(unwantedCustomer));

            if (_repository.Exists(unwantedCustomer.UserId))
                throw new InvalidOperationException("This user is already in the unwanted list.");

            
            _repository.Add(unwantedCustomer);
        }

        public void UpdateUnwantedCustomer(UnwantedCustomer unwantedCustomer)
        {
            if (unwantedCustomer == null)
                throw new ArgumentNullException(nameof(unwantedCustomer));

            if (!_repository.Exists(unwantedCustomer.UserId))
                throw new KeyNotFoundException("Unwanted customer record not found.");

            _repository.Update(unwantedCustomer);
        }

        public void RemoveUnwantedCustomer(int userId)
        {
            if (!_repository.Exists(userId))
                throw new KeyNotFoundException("Unwanted customer record not found.");

            _repository.Delete(userId);
        }

        public IEnumerable<User> GetPotentialUnwantedCustomers()
        {
            return _repository.GetPotentialUnwantedCustomers();
        }
    }
}