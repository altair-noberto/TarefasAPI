using System.ComponentModel.DataAnnotations;

namespace TarefasAPI.Models
{
    public class UsuarioRequest
    {
        public string Nome { get; set; } = string.Empty;
        [EmailAddress]
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Senha { get; set; } = string.Empty;
    }
}
