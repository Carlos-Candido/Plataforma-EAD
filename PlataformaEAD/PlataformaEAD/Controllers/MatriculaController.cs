using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlataformaEAD.Data;
using PlataformaEAD.Models;
using PlataformaEAD.Models.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaEAD.Controllers
{
    public class MatriculaController : Controller
    {
        private readonly AppDbContext _context;

        public MatriculaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Matricula
        public async Task<IActionResult> Index()
        {
            var matriculas = await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .ToListAsync();
            return View(matriculas);
        }

        // GET: Matricula/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Alunos = new SelectList(await _context.Alunos.ToListAsync(), "Id", "Nome");
            ViewBag.Cursos = new SelectList(await _context.Cursos.ToListAsync(), "Id", "Titulo");
            return View();
        }

        // POST: Matricula/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int alunoId, int[] cursosSelecionados, DateTime data, decimal precoPago, int progresso, Status status)
        {
            if (cursosSelecionados == null || cursosSelecionados.Length == 0)
            {
                ModelState.AddModelError("", "Selecione pelo menos um curso.");
            }

            if (ModelState.IsValid)
            {
                foreach (var cursoId in cursosSelecionados)
                {
                    if (_context.Matriculas.Any(m => m.AlunoId == alunoId && m.CursoId == cursoId))
                    {
                        var curso = await _context.Cursos.FindAsync(cursoId);
                        ModelState.AddModelError("", $"O aluno já está matriculado no curso {curso?.Titulo}.");
                        continue;
                    }

                    var matricula = new Matricula
                    {
                        AlunoId = alunoId,
                        CursoId = cursoId,
                        Data = data,
                        PrecoPago = precoPago,
                        Progresso = progresso,
                        Status = status
                    };
                    _context.Matriculas.Add(matricula);
                }

                if (_context.ChangeTracker.HasChanges())
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.Alunos = new SelectList(await _context.Alunos.ToListAsync(), "Id", "Nome");
            ViewBag.Cursos = new SelectList(await _context.Cursos.ToListAsync(), "Id", "Titulo");
            return View();
        }

        // GET: Matricula/Edit
        public async Task<IActionResult> Edit(int alunoId, int cursoId)
        {
            var matricula = await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);

            if (matricula == null) return NotFound();

            return View(matricula);
        }

        // POST: Matricula/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int alunoId, int cursoId, decimal precoPago, int progresso, decimal? notaFinal, Status status)
        {
            var matricula = await _context.Matriculas
                .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);

            if (matricula == null)
            {
                return NotFound();
            }

            if (status == Status.CONCLUIDO && progresso < 100)
            {
                ModelState.AddModelError("", "Para concluir a matrícula, o progresso deve ser 100%.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    matricula.PrecoPago = precoPago;
                    matricula.Progresso = progresso;
                    matricula.NotaFinal = notaFinal;
                    matricula.Status = status;

                    _context.Update(matricula);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatriculaExists(alunoId, cursoId))
                        return NotFound();
                    else
                        throw;
                }
            }

            // Recarrega os dados em caso de erro
            matricula = await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);

            return View(matricula);
        }

        // GET: Matricula/Delete
        public async Task<IActionResult> Delete(int alunoId, int cursoId)
        {
            var matricula = await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);

            if (matricula == null) return NotFound();

            return View(matricula);
        }

        // POST: Matricula/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int alunoId, int cursoId)
        {
            try
            {
                var matricula = await _context.Matriculas
                    .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);

                if (matricula != null)
                {
                    _context.Matriculas.Remove(matricula);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao excluir matrícula: {ex.Message}";
                return RedirectToAction(nameof(Delete), new { alunoId, cursoId });
            }
        }

        // GET: Matricula/Details
        public async Task<IActionResult> Details(int alunoId, int cursoId)
        {
            var matricula = await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);

            if (matricula == null)
                return NotFound();

            return View(matricula);
        }

        private bool MatriculaExists(int alunoId, int cursoId)
        {
            return _context.Matriculas.Any(e => e.AlunoId == alunoId && e.CursoId == cursoId);
        }
    }
}