using Database_Project.Models;
using System.Threading.Tasks;

namespace Database_Project.Services.Interfaces
{
    public interface IRoleManagerService
    {
        Task EnsureRolesExist(params string[] roles);
        Task UpdateUserRoleAsync(User user, string newRole);
        Task<string> GetUserRoleAsync(User user);
    }
}