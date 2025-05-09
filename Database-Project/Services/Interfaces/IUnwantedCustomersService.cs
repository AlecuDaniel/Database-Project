using Database_Project.Models;
using System.Collections.Generic;

namespace Database_Project.Services
{
    public interface IUnwantedCustomersService
    {
        IEnumerable<UnwantedCustomer> GetAllUnwantedCustomers();
        UnwantedCustomer GetUnwantedCustomer(int userId);
        void AddUnwantedCustomer(UnwantedCustomer unwantedCustomer);
        void UpdateUnwantedCustomer(UnwantedCustomer unwantedCustomer);
        void RemoveUnwantedCustomer(int userId);
        IEnumerable<User> GetPotentialUnwantedCustomers();
    }
}