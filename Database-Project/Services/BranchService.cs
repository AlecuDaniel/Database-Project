using System.Collections.Generic;
using System.Threading.Tasks;
using Database_Project.Models;
using Database_Project.Repositories.Interfaces;
using Database_Project.Services.Interfaces;

namespace Database_Project.Services
{
    public class BranchService : IBranchService
    {
        private readonly IGenericRepository<LibraryBranch> _repository;

        public BranchService(IGenericRepository<LibraryBranch> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<LibraryBranch>> GetAllBranchesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<LibraryBranch> GetBranchByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddBranchAsync(LibraryBranch branch)
        {
            await _repository.AddAsync(branch);
            await _repository.SaveAsync();
        }

        public async Task UpdateBranchAsync(LibraryBranch branch)
        {
            _repository.Update(branch);
            await _repository.SaveAsync();
        }

        public async Task DeleteBranchAsync(LibraryBranch branch)
        {
            _repository.Delete(branch);
            await _repository.SaveAsync();
        }
    }
}