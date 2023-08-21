using AgendaCitaOdontologia.Models;

namespace AgendaCitaOdontologia.Servicios.ProyectoU3.Servicios
{
    public interface IRepositorioAgenda
    {
        Task Editar(Agenda modelo);
        Task<bool> Existe(string nombre, int usuarioId);
        Task<IEnumerable<Agenda>> Obtener(int usuarioId);
        Task<Agenda> ObtenerPorId(int id, int usuarioId);
        Task Borrar(int id);
        Task<IEnumerable<Agenda>> ObtenerOrdenadoPorFecha(int usuarioId);
        Task<IEnumerable<Agenda>> ObtenerCitasProgramadas(int usuarioId);
        Task CrearCita(Agenda modelo);
    }
}