using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using M5Storage.Data;
using M5Storage.Models;

namespace M5Storage.Controllers
{
    public class RecursosController : Controller
    {
        private readonly AppDbContext _context;
        public RecursosController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index()
            => View(await _context.Recursos.ToListAsync());

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Recurso r, string? TipoEnergia, DateTime? Validade)
        {
            if (!ModelState.IsValid) return View(r);

            r.Critico = r.Quantidade <= r.Minimo ? 1 : 0;
            r.Nivel = CalcularNivel(r.Quantidade, r.Minimo);
            r.Status = r.Critico == 1 ? "CRITICO" : "NORMAL";
            r.UltimaAtualizacao = DateTime.Now;

            _context.Recursos.Add(r);
            await _context.SaveChangesAsync();

            if (r.Categoria.ToUpper() == "ENERGIA" && !string.IsNullOrEmpty(TipoEnergia))
            {
                _context.RecursosEnergia.Add(new RecursoEnergia { Id = r.Id, TipoEnergia = TipoEnergia });
                await _context.SaveChangesAsync();
            }
            else if (r.Categoria.ToUpper() == "MEDICAMENTO" && Validade.HasValue)
            {
                _context.RecursosMedicamento.Add(new RecursoMedicamento { Id = r.Id, Validade = Validade });
                await _context.SaveChangesAsync();
            }

            if (r.Critico == 1)
            {
                GerarAlerta(r);
                await _context.SaveChangesAsync();
            }

            TempData["Sucesso"] = "Recurso cadastrado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var r = await _context.Recursos
                .Include(x => x.RecursoEnergia)
                .Include(x => x.RecursoMedicamento)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (r == null) return NotFound();
            return View(r);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Recurso atualizado, string? TipoEnergia, DateTime? Validade)
        {
            if (!ModelState.IsValid) return View(atualizado);

            atualizado.Critico = atualizado.Quantidade <= atualizado.Minimo ? 1 : 0;
            atualizado.Nivel = CalcularNivel(atualizado.Quantidade, atualizado.Minimo);
            atualizado.Status = atualizado.Critico == 1 ? "CRITICO" : "NORMAL";
            atualizado.UltimaAtualizacao = DateTime.Now;

            _context.Recursos.Entry(atualizado).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            if (atualizado.Categoria.ToUpper() == "ENERGIA")
            {
                var energia = await _context.RecursosEnergia.FindAsync(atualizado.Id);
                if (energia != null)
                {
                    energia.TipoEnergia = TipoEnergia;
                    _context.RecursosEnergia.Entry(energia).State = EntityState.Modified;
                }
                else if (!string.IsNullOrEmpty(TipoEnergia))
                {
                    _context.RecursosEnergia.Add(new RecursoEnergia { Id = atualizado.Id, TipoEnergia = TipoEnergia });
                }
                await _context.SaveChangesAsync();
            }

            if (atualizado.Categoria.ToUpper() == "MEDICAMENTO")
            {
                var med = await _context.RecursosMedicamento.FindAsync(atualizado.Id);
                if (med != null)
                {
                    med.Validade = Validade;
                    _context.RecursosMedicamento.Entry(med).State = EntityState.Modified;
                }
                else if (Validade.HasValue)
                {
                    _context.RecursosMedicamento.Add(new RecursoMedicamento { Id = atualizado.Id, Validade = Validade });
                }
                await _context.SaveChangesAsync();
            }

            if (atualizado.Critico == 1)
            {
                var recursoCritico = _context.Recursos.Where(a => a.Id == atualizado.Id).Where(a => a.Critico != 0).ToList();

                if (recursoCritico.Count > 0)
                {
                    GerarAlerta(atualizado);
                    await _context.SaveChangesAsync();
                }
            }

            TempData["Sucesso"] = "Recurso atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var r = await _context.Recursos.FirstOrDefaultAsync(x => x.Id == id);
            if (r == null) return NotFound();
            return View(r);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var r = await _context.Recursos.FindAsync(id);
            if (r != null)
            {
                _context.Recursos.Remove(r);
                await _context.SaveChangesAsync();
            }
            TempData["Sucesso"] = "Recurso removido com sucesso!";
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

        private void GerarAlerta(Recurso r)
        {
            _context.Alertas.Add(new Alerta
            {
                RecursoId = r.Id,
                Mensagem = $"ALERTA: Recurso '{r.Nome}' atingiu nível crítico. Quantidade: {r.Quantidade} / Mínimo: {r.Minimo}",
                Nivel = r.Nivel,
                Resolvido = 0,
                DataAlerta = DateTime.Now
            });
        }
    }
}
