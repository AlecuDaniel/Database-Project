using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database_Project.Models
{
    public class User : IdentityUser<int>
    {
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public override string Email { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(255)]
        public string? ProfilePicture { get; set; }

        [StringLength(50)]
        public string? UserRole { get; set; }

        public string? Bio { get; set; }

        public ICollection<BorrowRecord> BorrowRecords { get; set; }
        public UnwantedCustomer UnwantedCustomer { get; set; }

        public override string UserName
        {
            get => Email;
            set => base.UserName = value;
        }
    }
}