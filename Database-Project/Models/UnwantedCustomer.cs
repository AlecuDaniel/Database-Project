using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database_Project.Models
{
    public class UnwantedCustomer
    {
        [Key]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        public DateTime DateAdded { get; set; } = DateTime.Now;

        [StringLength(255)]
        public string Reason { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public User? User { get; set; }
    }
}