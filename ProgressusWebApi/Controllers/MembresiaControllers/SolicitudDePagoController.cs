using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgressusWebApi.DataContext;
using ProgressusWebApi.Dtos.MembresiaDtos;
using ProgressusWebApi.Models.MembresiaModels;
using ProgressusWebApi.Services.MembresiaServices;
using ProgressusWebApi.Services.MembresiaServices.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class SolicitudDePagoController : ControllerBase
{
    private readonly ISolicitudDePagoService _solicitudDePagoService;
    private readonly ProgressusDataContext _context;

    public SolicitudDePagoController(ISolicitudDePagoService solicitudDePagoService, ProgressusDataContext context)
    {
        _solicitudDePagoService = solicitudDePagoService;
        _context = context;
    }

    [HttpPost("CrearSolicitudDePago")]
    public async Task<IActionResult> CrearSolicitudDePago(CrearSolicitudDePagoDto dto)
    {
        SolicitudDePago solicitud = await _solicitudDePagoService.CrearSolicitudDePago(dto);
        return Ok(solicitud);
    }

    [HttpPut("RegistrarPagoEnEfectivo")]
    public async Task<IActionResult> RegistrarPagoEnEfectivo(int idSolicitudDePago)
    {
        var solicitudExistente = await _solicitudDePagoService.RegistrarPagoEnEfectivo(idSolicitudDePago);
        if (solicitudExistente == null)
            return NotFound(); // Retornar 404 si no se encuentra la solicitud
        return Ok(solicitudExistente); // Retornar la solicitud actualizada
    }

    [HttpPut("RegistrarPagoConMercadoPago")]
    public async Task<IActionResult> RegistrarPagoConMercadoPago(int idSolicitudDePago)
    {
        var solicitud = await _solicitudDePagoService.RegistrarPagoConMercadoPago(idSolicitudDePago);
        if (solicitud == null)
            return NotFound(); // Retornar 404 si no se encuentra la solicitud
        return Ok(solicitud); // Retornar la solicitud actualizada
    }

    [HttpPut("CancelarSolicitudDePago")]
    public async Task<IActionResult> CancelarSolicitudDePago(int idSolicitudDePago)
    {
        var solicitudExistente = await _solicitudDePagoService.CancelarSolicitudDePago(idSolicitudDePago);
        if (solicitudExistente == null)
            return NotFound(); // Retornar 404 si no se encuentra la solicitud
        return Ok(solicitudExistente); // Retornar la solicitud cancelada
    }

    [HttpGet("ObtenerEstadoActualDeSolicitud")]
    public async Task<IActionResult> ObtenerEstadoActualDeSolicitud(int idSolicitudDePago)
    {
        return new OkObjectResult(_solicitudDePagoService.ObtenerEstadoActualDeSolicitud(idSolicitudDePago).Result);
    }

    [HttpGet("ObtenerSolicitudDePagoDeSocio")]
    public async Task<IActionResult> ObtenerSolicitudDePagoDeSocio(string IdentityUserId)
    {
        return new OkObjectResult(_solicitudDePagoService.ObtenerSolicitudDePagoDeSocio(IdentityUserId).Result);
    }

    [HttpGet("ObtenerTiposDePagos")]
    public async Task<IActionResult> ObtenerTiposDePagos()
    {
        return new OkObjectResult(await _solicitudDePagoService.ObtenerTiposDePago());
    }

    [HttpGet("ConsultarVigenciaDeMembresia")]
    public async Task<IActionResult> ConsultarVigenciaDeMembresia(string identityUserId)
    {
        return new OkObjectResult(await _solicitudDePagoService.ConsultarVigenciaDeMembresia(identityUserId));
    }

    [HttpGet("ObtenerTodasLasSolicitudesDeUnSocio")]
    public async Task<IActionResult> ObtenerTodasLasSolicitudesDeUnSocio(string identityUserId)
    {
        return new OkObjectResult(await _solicitudDePagoService.ObtenerTodasLasSolicitudesDeUnSocio(identityUserId));
    }

    [HttpGet("pagos-efectivo-confirmados")]
    public async Task<IActionResult> ObtenerPagosEfectivoConfirmadosAsync()
    {
        var pagosEfectivo = await _context.SolicitudDePagos
            .Where(s => s.TipoDePagoId == 1) // Filtra por pagos en efectivo
            .Join(
                _context.HistorialSolicitudDePagos,
                solicitud => solicitud.Id,
                historial => historial.SolicitudDePagoId,
                (solicitud, historial) => new { solicitud, historial }
            )
            .Where(joined => joined.historial.EstadoSolicitudId == 2) // Filtra por estado confirmado
            .Select(joined => new PagoEfectivoConfirmadoDto
            {
                TipoMembresiaId = joined.solicitud.MembresiaId,
                FechaPago = joined.historial.FechaCambioEstado
            })
            .ToListAsync();

        if (!pagosEfectivo.Any())
        {
            return NotFound("No se encontraron pagos en efectivo confirmados.");
        }

        return Ok(pagosEfectivo);
    }


    [HttpGet("pagos-efectivo-Usuario")]
    public async Task<IActionResult> ObtenerPagosEfectivoConfirmadosUsuario()
    {
        var pagosEfectivo = await _context.SolicitudDePagos
              .Where(s => s.TipoDePagoId == 1) // Filtra por pagos en efectivo
              .Join(
                  _context.HistorialSolicitudDePagos,
                  solicitud => solicitud.Id,
                  historial => historial.SolicitudDePagoId,
                  (solicitud, historial) => new { solicitud, historial }
              )
              .Join(
                  _context.Socios, // Tabla de Socios
                  joined => joined.solicitud.IdentityUserId,
                  socio => socio.UserId, // Relación entre UserId de SolicitudDePagos y Socio
                  (joined, socio) => new
                  {
                      joined.solicitud,
                      joined.historial,
                      socio
                  }
              )
              .Where(joined => joined.historial.EstadoSolicitudId == 2) // Filtra por estado confirmado
              .Select(joined => new PagoEfectivoUserConfirmadoDto
              {
                  TipoMembresiaId = joined.solicitud.MembresiaId,
                  FechaPago = joined.historial.FechaCambioEstado,
                  IdentityUserId = joined.socio.UserId,
                  Nombre = joined.socio.Nombre,
                  Apellido = joined.socio.Apellido
              })
              .ToListAsync();

        if (!pagosEfectivo.Any())
        {
            return NotFound("No se encontraron pagos en efectivo confirmados.");
        }

        return Ok(pagosEfectivo);
    }

}
