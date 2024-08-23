using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ToDoApp.Models;

namespace ToDoAppASP.Models
{
    public class ToDo
    {

        public enum Priority 
        { 
            Low,
            Medium, 
            High 
        }

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

        [Required(ErrorMessage = "Please select a priority.")]
        public Priority TaskPriority { get; set; } 

        [ValidateNever]

        public Status Status { get; set; } = null!;

        public bool Overdue => StatusId == "open" && DueDate < DateTime.Today;

        public string? RecurrenceFrequency { get; set; } 
        public int? RecurrenceInterval { get; set; } 
        public DateTime? RecurrenceEndDate { get; set; } 
    }
}
