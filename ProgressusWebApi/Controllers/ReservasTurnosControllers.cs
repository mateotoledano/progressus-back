using Microsoft.AspNetCore.Mvc;
using ProgressusWebApi.Services.ReservaService;
using ProgressusWebApi.Dtos.RerservaDto;  // Corrección del nombre
using ProgressusWebApi.Services.ReservaService.cs.interfaces;
using ProgressusWebApi.Dtos.RerservaDto;

namespace ProgressusWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasTurnosController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservasTurnosController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearReserva([FromBody] RerservaDto reservaDto)
        {
            var disponibilidad = await _reservaService.VerificarDisponibilidadAsync(reservaDto.Fecha, reservaDto.HoraInicio, reservaDto.HoraFin);
            if (disponibilidad == null)
            {
                return BadRequest("El horario seleccionado no está disponible.");
            }

            var resultado = await _reservaService.CrearReservaAsync(reservaDto);

            if (resultado == null) // Si no hay resultado o falla
            {
                return BadRequest("Hubo un error al crear la reserva.");
            }

            return Ok("Reserva creada con éxito.");
        }


        // Método para verificar disponibilidad
        [HttpGet("verificar")]
        public async Task<IActionResult> VerificarDisponibilidad(DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin)
        {
            var disponibilidad = await _reservaService.VerificarDisponibilidadAsync(fecha, horaInicio, horaFin);
            if (disponibilidad != null)
            {
                return Ok("Horario disponible.");
            }
            else
            {
                return BadRequest("El horario no está disponible.");
            }
        }

        // Obtener reservas por usuario
        [HttpGet("usuario/{userId}")]
        public async Task<IActionResult> ObtenerReservasPorUsuario(string userId)
        {
            var reservas = await _reservaService.ObtenerReservasPorUsuarioAsync(userId);
            if (reservas == null)
            {
                return NotFound("No se encontraron reservas para el usuario especificado.");
            }

            return Ok(reservas);
        }
        // Endpoint para obtener reservas por fecha y hora de inicio
        [HttpGet("ObtenerReservasPorHorario")]
        public async Task<IActionResult> ObtenerReservasPorHorarioAsync(DateTime fecha, TimeSpan horaInicio)
        {
            var resultado = await _reservaService.ObtenerReservasPorHorarioAsync(fecha, horaInicio);
            return resultado;
        }


        // Endpoint para registrar asistencia
        [HttpPost("registrarAsistencia/{userId}")]
        public async Task<IActionResult> RegistrarAsistencia(string userId)
        {
            var resultado = await _reservaService.RegistrarAsistenciaAsync(userId);

            if (!resultado)
            {
                return BadRequest("No se encontró ninguna reserva válida para este usuario en el horario actual.");
            }

            return Ok("Asistencia registrada con éxito.");
        }
        [HttpGet("EstadisticasPorMes")]
        public async Task<IActionResult> ObtenerEstadisticasReservas()
        {
            return await _reservaService.ObtenerEstadisticasReservasAsync();
        }


        [HttpGet("EstadisticasPorDiaSemana")]
        public async Task<IActionResult> ObtenerEstadisticasPorDiaSemana()
        {
            return await _reservaService.ObtenerEstadisticasPorDiaSemanaAsync();
        }


        // Método para eliminar una reserva
        [HttpDelete("eliminar/{userId}")]
        public async Task<IActionResult> EliminarReserva(string userId)
        {
            var resultado = await _reservaService.EliminarReservaAsync(userId);
            if (resultado == null)
            {
                return NotFound("No se encontró la reserva a eliminar.");
            }

            return Ok("Reserva eliminada exitosamente.");
        }


    }
}
