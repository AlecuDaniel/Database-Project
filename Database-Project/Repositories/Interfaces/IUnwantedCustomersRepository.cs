using Database_Project.Models;
using System.Collections.Generic;

namespace Database_Project.Repository
{
    public interface IUnwantedCustomersRepository
    {
        IEnumerable<UnwantedCustomer> GetAll();
        UnwantedCustomer GetById(int userId);
        void Add(UnwantedCustomer unwantedCustomer);
        void Update(UnwantedCustomer unwantedCustomer);
        void Delete(int userId);
        bool Exists(int userId);
        IEnumerable<User> GetPotentialUnwantedCustomers();
    }
}