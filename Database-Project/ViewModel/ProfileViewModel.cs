using System.ComponentModel.DataAnnotations;

namespace Database_Project.ViewModels
{
    public class ProfileViewModel
    {
        public string? SelectedProfilePicture { get; set; }

        [StringLength(500)]
        public string? Bio { get; set; }

        public List<string> AvailablePictures { get; set; } = new();
    }
}