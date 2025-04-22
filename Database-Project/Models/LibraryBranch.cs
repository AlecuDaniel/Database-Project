using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database_Project.Models
{
    public class LibraryBranch
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        public ICollection<BookStock> BookStocks { get; set; }
        public ICollection<BorrowRecord> BorrowRecords { get; set; }
    }
}