using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database_Project.Models
{
    public class BookStock
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Book")]
        public int BookId { get; set; }

        [Required]
        [ForeignKey("LibraryBranch")]
        public int BranchId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public Book Book { get; set; }
        public LibraryBranch LibraryBranch { get; set; }
    }

}