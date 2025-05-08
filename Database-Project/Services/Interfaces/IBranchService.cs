using System.Collections.Generic;
using System.Threading.Tasks;
using Database_Project.Models;

namespace Database_Project.Services.Interfaces
{
    public interface IBranchService
    {
        Task<IEnumerable<LibraryBranch>> GetAllBranchesAsync();
        Task<LibraryBranch> GetBranchByIdAsync(int id);
        Task AddBranchAsync(LibraryBranch branch);
        Task UpdateBranchAsync(LibraryBranch branch);
        Task DeleteBranchAsync(LibraryBranch branch);
    }
}