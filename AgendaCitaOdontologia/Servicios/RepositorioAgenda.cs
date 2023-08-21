using AgendaCitaOdontologia.Models;
using AgendaCitaOdontologia.Servicios.ProyectoU3.Servicios;
using Dapper;
using Microsoft.AspNetCore.Connections;
using Microsoft.Data.SqlClient;
//using ProyectoU3.Models;
using System.Data.SqlClient;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;

namespace ProyectoU3.Servicios
{
    public class RepositorioAgenda : IRepositorioAgenda
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<RepositorioAgenda> logger;
        private readonly string connectionString;



        public RepositorioAgenda(IConfiguration configuration, ILogger<RepositorioAgenda> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task CrearCita(Agenda modelo)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                var insertQuery = @"
            INSERT INTO Agenda2
            (Nombre, Edad, Telefono, Email, TipoCita, FechaHota, Observacion, UsuarioId)
            VALUES
            (@Nombre, @Edad, @Telefono, @Email, @TipoCita, @FechaHota, @Observacion, @UsuarioId);
            SELECT SCOPE_IDENTITY();";

                var id = await connection.QuerySingleAsync<int>(insertQuery, new
                {
                    modelo.Nombre,
                    modelo.Edad,
                    modelo.Telefono,
                    modelo.Email,
                    modelo.TipoCita,
                    modelo.FechaHota,
                    modelo.Observacion,
                    modelo.UsuarioId
                });

                modelo.Id = id;

                logger.LogInformation($"Cita creada. ID: {modelo.Id}, Nombre: {modelo.Nombre}");
            }
            catch (Exception ex)
            {
                // Manejar la excepción adecuadamente, ya sea registrándola o propagándola
                logger.LogError($"Error al crear la cita: {ex.Message}");
                throw;
            }
        }


        public async Task<bool> Existe(string nombre, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(
                $@"SELECT 1 FROM Agenda2
                WHERE Nombre = @Nombre AND UsuarioId = @UsuarioId",
                new { nombre, usuarioId });

            return existe == 1;
        }

        public async Task<IEnumerable<Agenda>> Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Agenda>(
                $@"SELECT Id, Nombre, Edad, 
                          Telefono, Email,
                          TipoCita, FechaHota, 
                          Observacion
                FROM Agenda2
                WHERE UsuarioId = @usuarioId",
                new { usuarioId });
        }
        public async Task<Agenda> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Agenda>
                (@"
                SELECT
	                Id,
	                Nombre,
                    Edad,
	                Telefono,
	                Email,
	                TipoCita,
	                FechaHota,
	                Observacion
                FROM Agenda2
                WHERE Id = @id AND UsuarioId = @usuarioId
                ", new { id, usuarioId });
        }

        public async Task Editar(Agenda modelo)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync
                (@"
                UPDATE Agenda2
                SET Nombre = @nombre,
                    Edad = @edad,
                    Telefono = @telefono,
                	Email = @email,
                	TipoCita = @tipoCita,
                	FechaHota = @fechahota,
                	Observacion = @observacion
                WHERE Id = @id;
                ", modelo);
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                @"DELETE FROM Agenda2 WHERE Id = @id",
                new { id });
        }

        public async Task<IEnumerable<Agenda>> ObtenerOrdenadoPorFecha(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Agenda>(
                $@"SELECT Id, Nombre,Edad, 
                  Telefono, Email,
                  TipoCita, FechaHota, 
                  Observacion
        FROM Agenda2
        WHERE UsuarioId = @usuarioId
        ORDER BY FechaHota",
                new { usuarioId });
        }

        //public  async Task<IEnumerable<Agenda>> ObtenerCitasProgramadas(int usuarioId)
        //{
        //    using var connection = new SqlConnection(connectionString);
        //    return await connection.QueryAsync<Agenda>(
        //        $@"SELECT Id, Nombre, Edad, 
        //          Telefono, Email,
        //          TipoCita, FechaHota, 
        //          Observacion
        //          FROM Agenda2
        //          WHERE UsuarioId = @usuarioId
        //        ORDER BY FechaHota",
        //        new { usuarioId });
        //}

        public async Task<IEnumerable<Agenda>> ObtenerCitasProgramadas(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Agenda>(
                @"SELECT Id, Nombre, Edad, 
                  Telefono, Email,
                  TipoCita, FechaHota, 
                  Observacion
                  FROM Agenda2
                  WHERE UsuarioId = @usuarioId
                ORDER BY FechaHota",
                new { usuarioId });
        }
    }
}

