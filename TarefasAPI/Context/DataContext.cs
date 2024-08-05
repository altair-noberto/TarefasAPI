using Microsoft.EntityFrameworkCore;
using TarefasAPI.Models;

namespace TarefasAPI.Context
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }
    }
}
