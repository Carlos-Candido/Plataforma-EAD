using System.ComponentModel.DataAnnotations;

namespace PlataformaEAD.Models
{
    public class Aluno
    {
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string Nome { get; set; } = string.Empty;

        [EmailAddress, StringLength(120)]
        public string? Email { get; set; }

        [Phone, StringLength(20)]
        public string? Telefone { get; set; }

        public ICollection<Matricula>? Matriculas { get; set; }
    }
}
