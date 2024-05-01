using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TodoApp.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class TodoDb : DbContext
    {
        public TodoDb(DbContextOptions<TodoDb> options) : base(options) { }
        public DbSet<Todo> Todos { get; set; } = null!;
    }
}