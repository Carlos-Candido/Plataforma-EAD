using Microsoft.EntityFrameworkCore;
using PlataformaEAD.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace PlataformaEAD.Models
{
    public class Matricula
    {
        public int AlunoId { get; set; }
        public Aluno? Aluno { get; set; }

        public int CursoId { get; set; }
        public Curso? Curso { get; set; }

        [Required]
        public DateTime Data { get; set; } = DateTime.UtcNow;

        [Range(0, double.MaxValue)]
        [Precision(18, 2)]
        public decimal PrecoPago { get; set; }

        public Status Status { get; set; }

        [Range(0, 100)]
        public int Progresso { get; set; } = 0;

        [Range(0, 10)]
        public decimal? NotaFinal { get; set; }
    }
}
