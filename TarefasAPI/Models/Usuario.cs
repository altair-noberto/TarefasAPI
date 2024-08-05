using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TarefasAPI.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class Usuario
    {
        public int Id { get; set; }
        [StringLength(30)]
        public string Nome { get; set; } = string.Empty;
        [EmailAddress]
        [Required]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        public ICollection<Tarefa>? Tarefas { get; set; }
    }
}
