using Microsoft.AspNetCore.Mvc;
using M5Storage.Data;

namespace M5Storage.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.TotalUsuarios = _context.Usuarios.Count();
            ViewBag.TotalRecursos = _context.Recursos.Count();
            ViewBag.TotalMovimentacoes = _context.Movimentacoes.Count();
            ViewBag.TotalAlertas = _context.Alertas.Count(a => a.Resolvido == 0);
            ViewBag.RecursosCriticos = _context.Recursos.Count(r => r.Critico == 1);
            ViewBag.AlertasAbertos = _context.Alertas.Count(a => a.Resolvido == 0);

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
