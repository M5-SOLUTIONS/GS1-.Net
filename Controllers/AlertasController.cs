using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using M5Storage.Data;
using M5Storage.Models;

namespace M5Storage.Controllers
{
    public class AlertasController : Controller
    {
        private readonly AppDbContext _context;
        public AlertasController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index()
            => View(await _context.Alertas
                .Include(a => a.Recurso)
                .OrderByDescending(a => a.DataAlerta)
                .ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Resolver(int id)
        {
            var a = await _context.Alertas.FindAsync(id);
            if (a != null)
            {
                a.Resolvido = 1;
                _context.Alertas.Entry(a).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                TempData["Sucesso"] = "Alerta marcado como resolvido!";
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var a = await _context.Alertas
                .Include(x => x.Recurso)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (a == null) return NotFound();
            return View(a);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var a = await _context.Alertas.FindAsync(id);
            if (a != null)
            {
                _context.Alertas.Remove(a);
                await _context.SaveChangesAsync();
            }
            TempData["Sucesso"] = "Alerta removido com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}
