using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using M5Storage.Data;
using M5Storage.Models;

namespace M5Storage.Controllers
{
    public class MovimentacoesController : Controller
    {
        private readonly AppDbContext _context;
        public MovimentacoesController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index()
            => View(await _context.Movimentacoes
                .Include(m => m.Usuario)
                .Include(m => m.Recurso)
                .OrderByDescending(m => m.DataMovimentacao)
                .ToListAsync());

        public async Task<IActionResult> Create()
        {
            ViewBag.UsuarioId = new SelectList(await _context.Usuarios.ToListAsync(), "Id", "Nome");
            ViewBag.RecursoId = new SelectList(await _context.Recursos.ToListAsync(), "Id", "Nome");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Movimentacao m)
        {
            ViewBag.UsuarioId = new SelectList(await _context.Usuarios.ToListAsync(), "Id", "Nome", m.UsuarioId);
            ViewBag.RecursoId = new SelectList(await _context.Recursos.ToListAsync(), "Id", "Nome", m.RecursoId);

            if (!ModelState.IsValid) return View(m);

            m.TipoMovimentacao = m.TipoMovimentacao.ToUpper();
            m.DataMovimentacao = DateTime.Now;

            var recurso = await _context.Recursos.FindAsync(m.RecursoId);
            if (recurso != null)
            {
                if (m.TipoMovimentacao == "CONSUMO")
                {
                    if (recurso.Quantidade < m.Quantidade)
                    {
                        ModelState.AddModelError("Quantidade", "Quantidade insuficiente em estoque!");
                        return View(m);
                    }
                    recurso.Quantidade -= m.Quantidade;
                }
                else if (m.TipoMovimentacao == "REABASTECIMENTO")
                {
                    recurso.Quantidade += m.Quantidade;
                }

                recurso.Critico = recurso.Quantidade <= recurso.Minimo ? 1 : 0;
                recurso.Nivel = CalcularNivel(recurso.Quantidade, recurso.Minimo);
                recurso.Status = recurso.Critico == 1 ? "CRITICO" : "NORMAL";
                recurso.UltimaAtualizacao = DateTime.Now;

                _context.Recursos.Entry(recurso).State = EntityState.Modified;

                _context.Movimentacoes.Add(m);
                await _context.SaveChangesAsync();

                if (recurso.Critico == 1)
                {
                    var recursoCritico = _context.Recursos.Where(a => a.Id == recurso.Id).Where(a => a.Critico != 0).ToList();

                    if (recursoCritico.Count > 0)
                    {
                        _context.Alertas.Add(new Alerta
                        {
                            RecursoId = recurso.Id,
                            Mensagem = $"ALERTA: '{recurso.Nome}' em nível crítico após movimentação. Qtd: {recurso.Quantidade}",
                            Nivel = recurso.Nivel,
                            Resolvido = 0,
                            DataAlerta = DateTime.Now
                        });

                        await _context.SaveChangesAsync();
                    }
                }
            }

            TempData["Sucesso"] = "Movimentação registrada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var m = await _context.Movimentacoes
                .Include(x => x.Usuario)
                .Include(x => x.Recurso)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (m == null) return NotFound();
            return View(m);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var m = await _context.Movimentacoes.FindAsync(id);
            if (m != null)
            {
                _context.Movimentacoes.Remove(m);
                await _context.SaveChangesAsync();
            }
            TempData["Sucesso"] = "Movimentação removida com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private string CalcularNivel(decimal quantidade, decimal minimo)
        {
            if (minimo == 0) return "NORMAL";
            double pct = (double)(quantidade / minimo);
            if (pct <= 1.0) return "CRITICO";
            if (pct <= 1.5) return "BAIXO";
            if (pct <= 3.0) return "NORMAL";
            return "ALTO";
        }
    }
}
