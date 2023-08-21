using AgendaCitaOdontologia.Models;
using AgendaCitaOdontologia.Servicios.ProyectoU3.Servicios;
using AgendaCitaOdontologia.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;

namespace AgendaCitaOdontologia.Controllers
{
    public class AgendaController : Controller
    {
        private readonly string connectionString;
        //private readonly IConfiguration configuration;
        private readonly ILogger<AgendaController> logger;
        private readonly IRepositorioAgenda repositorioAgenda;
        private readonly IServicioUsuarios servicioUsuarios;
        public AgendaController(ILogger<AgendaController> logger, 
            IRepositorioAgenda repositorioAgenda, 
            IServicioUsuarios servicioUsuarios)
        {
            this.logger = logger;
            this.repositorioAgenda = repositorioAgenda;
            this.servicioUsuarios = servicioUsuarios;
        }

        public IActionResult Gracias()
        {
            return View();
        }

        public IActionResult Conocenos()
        {
            return View();
        }

        public IActionResult Contactenos()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login(string email, string password)
        {
            string usuarioValidoEmail = "usuario@example.com";
            string usuarioValidoPassword = "123456";

            if (email == usuarioValidoEmail && password == usuarioValidoPassword)
            {
                return RedirectToAction("AgendaCita");
            }

            ViewBag.ErrorMessage = "Credenciales inválidas";
            return View();
        }

        public async Task<IActionResult> AgendaCita()
        {
            var usaurioId = servicioUsuarios.ObtenerUsuarioId();
            var agenda = await repositorioAgenda.ObtenerOrdenadoPorFecha(usaurioId);
            return View(agenda);
        }

        [HttpGet]
        public IActionResult CrearCita()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CrearCita(Agenda modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }
            modelo.UsuarioId = servicioUsuarios.ObtenerUsuarioId();
            await repositorioAgenda.CrearCita(modelo);
            return RedirectToAction("Gracias");
        }
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var agenda = await repositorioAgenda.ObtenerPorId(id, usuarioId);
            if (agenda is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(agenda);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(Agenda modelo)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tipoCuentaExiste = await repositorioAgenda.ObtenerPorId(modelo.Id, usuarioId);
            if (tipoCuentaExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await repositorioAgenda.Editar(modelo);

            return RedirectToAction("AgendaCita");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var agenda = await repositorioAgenda.ObtenerPorId(id, usuarioId);
            if (agenda == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(agenda);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarCita(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var agenda = await repositorioAgenda.ObtenerPorId(id, usuarioId);
            if (agenda is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await repositorioAgenda.Borrar(id);
            return RedirectToAction("AgendaCita");
        }


        public IEnumerable<Agenda> ObtenerCitasProgramadas()
        {
            // Lógica para obtener las citas programadas desde el repositorio/servicio
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            return repositorioAgenda.Obtener(usuarioId).Result;
        }

        public async Task<IActionResult> Calendario()
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var citas = await repositorioAgenda.ObtenerCitasProgramadas(usuarioId);

            var eventos = citas.Select(c => new
            {
                title = c.Nombre,
                start = c.FechaHota.ToString("yyyy-MM-ddTHH:mm:ss")
            }).ToList();

            ViewBag.Eventos = JsonConvert.SerializeObject(eventos);
            return View();
        }
    }
    
}
