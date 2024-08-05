using System.ComponentModel.DataAnnotations;

namespace TarefasAPI.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        [Required]
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public Usuario? Usuario { get; set; }
    }
}
