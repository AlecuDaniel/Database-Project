using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Database_Project.Models;
using Database_Project.Repositories.Interfaces;
using Database_Project.Services;


using Database_Project.Repositories.Interfaces;

public class FakeGenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly Dictionary<int, T> _storage = new();
    private int _nextId = 1;
    public Task AddAsync(T entity)
    {
        var idProp = typeof(T).GetProperty("Id");
        if (idProp != null && idProp.GetValue(entity) is int id && id == 0)
            idProp.SetValue(entity, _nextId++);
        var idValue = (int)idProp.GetValue(entity);
        _storage[idValue] = entity;
        return Task.CompletedTask;
    }
    public void Delete(T entity)
    {
        var idProp = typeof(T).GetProperty("Id");
        if (idProp == null) return;
        var idValue = (int)idProp.GetValue(entity);
        _storage.Remove(idValue);
    }
    public Task<IEnumerable<T>> GetAllAsync() => Task.FromResult<IEnumerable<T>>(_storage.Values);
    public Task<T> GetByIdAsync(int id) => Task.FromResult(_storage.ContainsKey(id) ? _storage[id] : null);
    public void Update(T entity)
    {
        var idProp = typeof(T).GetProperty("Id");
        if (idProp == null) return;
        var idValue = (int)idProp.GetValue(entity);
        _storage[idValue] = entity;
    }
    public Task SaveAsync() => Task.CompletedTask;
}
