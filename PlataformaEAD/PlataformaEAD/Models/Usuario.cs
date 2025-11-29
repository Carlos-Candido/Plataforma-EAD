using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PlataformaEAD.Models
{
    public class Usuario : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string NomeCompleto { get; set; } = string.Empty;

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    }
}