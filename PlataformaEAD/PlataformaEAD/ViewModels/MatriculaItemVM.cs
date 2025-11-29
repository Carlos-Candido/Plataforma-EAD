using PlataformaEAD.Models.Enums;

namespace PlataformaEAD.ViewModels
{
    public class MatriculaItemVM
    {
        public int CursoId { get; set; }
        public string CursoTitulo { get; set; } = string.Empty;
        public decimal PrecoPago { get; set; }
        public int Progresso { get; set; }
        public decimal? NotaFinal { get; set; }
        public Status Status { get; set; }

        public bool Selecionado { get; set; }
    }
}
