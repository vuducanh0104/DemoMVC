using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models
{
    public class Person
    {
        [Key]
        [Required]
        public string PersonId { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        public string? Address { get; set; }
    }
}
