using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using M5Storage.Data;
using M5Storage.Models;

namespace M5Storage.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;
        public UsuariosController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index()
            => View(await _context.Usuarios.ToListAsync());

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Usuario u)
        {
            if (!ModelState.IsValid) return View(u);

            var existe = _context.Usuarios.Where(x => x.Email == u.Email).ToList();
            if (existe.Count > 0)
            {
                ModelState.AddModelError("Email", "Email já cadastrado.");
                return View(u);
            }

            _context.Usuarios.Add(u);
            _context.SaveChanges();

            TempData["Sucesso"] = "Usuário cadastrado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var u = await _context.Usuarios.FindAsync(id);
            if (u == null) return NotFound();
            return View(u);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Usuario atualizado)
        {
            if (!ModelState.IsValid) return View(atualizado);

            _context.Usuarios.Entry(atualizado).State = EntityState.Modified;
            _context.SaveChanges();

            TempData["Sucesso"] = "Usuário atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var u = await _context.Usuarios.FindAsync(id);
            if (u == null) return NotFound();
            return View(u);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var u = await _context.Usuarios.FindAsync(id);
            if (u != null)
            {
                _context.Usuarios.Remove(u);
                await _context.SaveChangesAsync();
            }
            TempData["Sucesso"] = "Usuário removido com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}
