// File: ViewModels/BookStockViewModel.cs
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace Database_Project.ViewModels
{
    public class BookStockViewModel
    {

        [Required]
        [Display(Name = "Book")]
        public int BookId { get; set; }

        [Required]
        [Display(Name = "Library Branch")]
        public int BranchId { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative number.")]
        public int Quantity { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Books { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Branches { get; set; }
    }
}
