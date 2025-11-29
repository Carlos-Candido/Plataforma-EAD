using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PlataformaEAD.Models
{
    public class Curso
    {
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string Titulo { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        [Range(0, double.MaxValue)]
        [Precision(18, 2)]
        public decimal PrecoBase { get; set; }

        [Range(1, int.MaxValue)]
        public int CargaHoraria { get; set; }

        public ICollection<Matricula>? Matriculas { get; set; }
    }
}
