using System;
using System.Threading.Tasks;
using Database_Project.Models;
using Database_Project.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Database_Project.Services
{
    public class RoleManagerService : IRoleManagerService
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RoleManagerService> _logger;

        public RoleManagerService(
            RoleManager<IdentityRole<int>> roleManager,
            UserManager<User> userManager,
            ILogger<RoleManagerService> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task EnsureRolesExist(params string[] roles)
        {
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole<int>(role));
                    _logger.LogInformation("Created role {Role}", role);
                }
            }
        }

        public async Task UpdateUserRoleAsync(User user, string newRole)
        {
            
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            
            await _userManager.AddToRoleAsync(user, newRole);

            
            user.UserRole = newRole;
            await _userManager.UpdateAsync(user);
        }

        public async Task<string> GetUserRoleAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.Count > 0 ? roles[0] : null;
        }
    }
}