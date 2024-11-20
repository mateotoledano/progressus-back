using ProgressusWebApi.Dtos.RerservaDto;
//using ProgressusWebApi.Services.ReservaService.Interfaces;
using ProgressusWebApi.DataContext;
using Microsoft.AspNetCore.Mvc;
using ProgressusWebApi.DataContext;
using ProgressusWebApi.Dtos.RerservaDto;
using ProgressusWebApi.Models.RerservasModels;
using ProgressusWebApi.Services.ReservaService.cs.interfaces;
using Microsoft.EntityFrameworkCore;
using MercadoPago.Resource.User;
using ProgressusWebApi.Models.AsistenciaModels;

namespace ProgressusWebApi.Services.ReservaServices
{
    public class ReservaService : IReservaService
    {
        private readonly ProgressusDataContext _context;

        public ReservaService(ProgressusDataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> CrearReservaAsync(RerservaDto reservaDto)
        {
            // Lógica para crear una reserva
            var existeConflicto = _context.Reservas
                .Any(r => r.FechaReserva == reservaDto.Fecha &&
                          r.HoraInicio < reservaDto.HoraFin &&
                          r.HoraFin > reservaDto.HoraInicio);

            if (existeConflicto)
            {
                return new BadRequestObjectResult("El horario seleccionado ya está reservado.");
            }

            var reserva = new ReservaTurno
            {
                UserId = reservaDto.UserId,
                FechaReserva = reservaDto.Fecha,
                HoraInicio = reservaDto.HoraInicio,
                HoraFin = reservaDto.HoraFin,
                Confirmada = true // Confirmada automáticamente
            };

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return new OkObjectResult("Reserva creada exitosamente.");
        }

        public async Task<IActionResult> VerificarDisponibilidadAsync(DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin)
        {
            // Lógica para verificar disponibilidad
            var existeConflicto = _context.Reservas
                .Any(r => r.FechaReserva == fecha && r.HoraInicio < horaFin && r.HoraFin > horaInicio);

            return new OkObjectResult(!existeConflicto);
        }

        public async Task<IActionResult> ObtenerReservasPorUsuarioAsync(string userId)
        {
            var reservas = _context.Reservas
                .Where(r => r.UserId == userId)
                .ToList();

            return new OkObjectResult(reservas);
        }
        public async Task<IActionResult> ObtenerReservasPorHorarioAsync(DateTime fecha, TimeSpan horaInicio)
        {
            var reservas = await _context.Reservas
                .Where(r => r.FechaReserva == fecha && r.HoraInicio == horaInicio)
                .ToListAsync();

            if (!reservas.Any())
            {
                return new NotFoundObjectResult("No se encontraron reservas para la fecha y hora especificadas.");
            }

            return new OkObjectResult(reservas);
        }
        public async Task<bool> RegistrarAsistenciaAsync(string userId)
        {
            var ahora = DateTime.Now;

            // Buscar una reserva válida para el usuario en la fecha y hora actual
            var reserva = await _context.Reservas
                .Where(r => r.UserId == userId
                            && r.FechaReserva.Date == ahora.Date // La fecha debe coincidir
                            && r.HoraInicio <= ahora.TimeOfDay   // La hora actual está dentro del rango
                            && r.HoraFin >= ahora.TimeOfDay)
                .FirstOrDefaultAsync();

            if (reserva == null)
            {
                // No se encontró ninguna reserva válida
                return false;
            }

            // Registrar el log de asistencia
            await RegistrarLogDeAsistenciaAsync(userId, reserva.Id);

            return true;
        }


        public async Task RegistrarLogDeAsistenciaAsync(string userId, int reservaId)
        {
            // Crear un nuevo registro en la tabla AsistenciaLogs
            var log = new AsistenciaLog
            {
                UserId = userId,                 // ID del usuario
                ReservaId = reservaId,           // ID de la reserva correspondiente
                FechaAsistencia = DateTime.Now   // Fecha y hora actuales
            };

            _context.AsistenciaLogs.Add(log); // Agregar el registro al contexto
            await _context.SaveChangesAsync(); // Guardar los cambios en la base de datos
        }
        public async Task<IActionResult> ObtenerEstadisticasReservasAsync()
        {
            var estadisticas = await _context.Reservas
                .GroupBy(r => new { r.FechaReserva.Year, r.FechaReserva.Month })
                .Select(g => new
                {
                    Anio = g.Key.Year,
                    Mes = g.Key.Month,
                    TotalReservas = g.Count()
                })
                .OrderBy(e => e.Anio)
                .ThenBy(e => e.Mes)
                .ToListAsync();

            return new OkObjectResult(estadisticas);
        }

        public async Task<IActionResult> ObtenerEstadisticasPorDiaSemanaAsync()
        {
            // Reduce la cantidad de datos primero (consulta en la base de datos)
            var reservas = await _context.Reservas
                .Select(r => new { r.FechaReserva, r.Id }) // Solo trae los campos necesarios
                .ToListAsync(); // Ejecuta la consulta en la base de datos

            // Realiza la agrupación y conteo en memoria
            var estadisticas = reservas
                .AsEnumerable() // Cambia a LINQ en memoria
                .GroupBy(r => new { r.FechaReserva.Year, DiaSemana = r.FechaReserva.DayOfWeek })
                .Select(g => new
                {
                    Anio = g.Key.Year,
                    DiaSemana = g.Key.DiaSemana.ToString(),
                    TotalReservas = g.Count()
                })
                .OrderBy(e => e.Anio)
                .ThenBy(e => e.DiaSemana)
                .ToList();

            return new OkObjectResult(estadisticas);
        }


        // Método para eliminar una reserva
        public async Task<IActionResult> EliminarReservaAsync(string UserId)
        {
            // Buscar la reserva por ID
            var reserva = await _context.Reservas.FirstOrDefaultAsync(r => r.UserId == UserId);
            if (reserva == null)
            {
                return new NotFoundObjectResult("No se encontró la reserva especificada.");
            }

            // Eliminar la reserva
            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            return new OkObjectResult("Reserva eliminada exitosamente.");
        }
    }
}