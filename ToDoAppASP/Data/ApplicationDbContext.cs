using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ToDoApp.Models;

namespace ToDoApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> ToDos { get; set; }
    }
}
