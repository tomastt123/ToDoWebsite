using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ToDoApp.Models;

namespace ToDoAppASP.Models
{
    public class ToDo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a title.")]

        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a due date.")]

        public DateTime? DueDate { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        public string CategoryId { get; set; } = string.Empty;

        [ValidateNever]

        public Category Category { get; set; } = null!;

        [Required(ErrorMessage = "Please select a status.")]

        public string StatusId { get; set; } = string.Empty;
        [ValidateNever]

        public Status Status { get; set; } = null!;

        public bool Overdue => StatusId == "open" && DueDate < DateTime.Today;
    }
}
