namespace PlataformaEAD.ViewModels
{
    public class MatriculaFormVM
    {
        public int AlunoId { get; set; }
        public List<MatriculaItemVM> Itens { get; set; } = new();
    }
}
