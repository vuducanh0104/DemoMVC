using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models
{
    public class Student
    {
        [Key]
        public string StudentCode { get; set; } = default!;

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = default!;

        public string? Address { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(254)]
        public string Email { get; set; } = default!;

        [Range(2000, 2099)]
        public int AdmissionYear { get; set; }

        [Required]
        [RegularExpression(@"^\d{2}$", ErrorMessage = "Mã hệ đào tạo phải gồm 2 chữ số.")]
        public string TrainingSystemCode { get; set; } = "21";

        [Required]
        [RegularExpression(@"^\d{2}$", ErrorMessage = "Mã ngành phải gồm 2 chữ số.")]
        public string MajorCode { get; set; } = "05";

        [Required]
        public string DepartmentCode { get; set; } = string.Empty;

        public Department? Department { get; set; }
    }
}
