using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Database_Project.Models;
using Database_Project.Repositories.Interfaces;
using Database_Project.Services;
using Database_Project.Repository;

public class FakeUnwantedCustomersRepository : IUnwantedCustomersRepository
{
    private readonly Dictionary<int, UnwantedCustomer> _storage = new();
    private readonly List<User> _potentialUsers = new();

    public IEnumerable<UnwantedCustomer> GetAll() => _storage.Values;
    public UnwantedCustomer GetById(int userId) => _storage.TryGetValue(userId, out var val) ? val : null;
    public void Add(UnwantedCustomer unwantedCustomer) => _storage[unwantedCustomer.UserId] = unwantedCustomer;
    public void Update(UnwantedCustomer unwantedCustomer) => _storage[unwantedCustomer.UserId] = unwantedCustomer;
    public void Delete(int userId) => _storage.Remove(userId);
    public bool Exists(int userId) => _storage.ContainsKey(userId);
    public IEnumerable<User> GetPotentialUnwantedCustomers() => _potentialUsers;
    public void AddPotentialUser(User user) => _potentialUsers.Add(user);
}