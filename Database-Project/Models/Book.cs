using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database_Project.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [StringLength(20)]
        public string ISBN { get; set; }

        [StringLength(255)]
        public string Authors { get; set; }

        [StringLength(100)]
        public string Publisher { get; set; }

        public string Description { get; set; }

        public ICollection<BookStock>? BookStocks { get; set; }
        public ICollection<BorrowRecord>? BorrowRecords { get; set; }
    }
}