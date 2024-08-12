using Microsoft.EntityFrameworkCore;
using ToDoApp.Models;

namespace ToDoAppASP.Models
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options) { }

        public DbSet<ToDo> ToDos { get; set; } = null!;

        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<Status> Statuses { get; set; } = null!;

        //seed data

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = "work", Name = "Work" },
                new Category { CategoryId = "home", Name = "Home" },
                new Category { CategoryId = "hobby", Name = "Hobby" },
                new Category { CategoryId = "school", Name = "School" },
                new Category { CategoryId = "call", Name = "Contact" }
                );

            modelBuilder.Entity<Status>().HasData(
                new Status { StatusId = "open", Name = "Open" },
                new Status { StatusId = "done", Name = "Done" },
                new Status { StatusId = "inProgress", Name = "In Progress" }
                );
        }
    }
}
