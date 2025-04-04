using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgressusWebApi.DataContext;
using ProgressusWebApi.Dtos.AuthDtos;
using ProgressusWebApi.Models.RolesUsuarioModels;
using ProgressusWebApi.Services.AuthServices;
using ProgressusWebApi.Services.AuthServices.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProgressusWebApi.Controllers.AuthControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _usuarioService;
        private readonly ProgressusDataContext _context;

        public AuthController(IAuthService usuarioService, ProgressusDataContext context)
        {
            _usuarioService = usuarioService;
            _context = context;
        }

        [HttpPut("CambiarContraseña")]
        public async Task<IActionResult> CambiarContraseña([FromBody] string correo, string nuevaContraseña, string contraseñaActual)
        {
            return await _usuarioService.CambiarContraseña(correo, nuevaContraseña, contraseñaActual);
        }

        [HttpPost("EnviarCodigoDeVerificacionRecuperarContraseña")]
        public async Task<IActionResult> EnviarCodigoDeVerificacionCambioContraseña([FromBody] CorreoRequestDto correo)
        {
            return await _usuarioService.EnviarCodigoDeVerificacion(correo.Email);
        }

        [HttpPost("EnviarCodigoDeVerificacionConfirmarEmail")]
        public async Task<IActionResult> EnviarCodigoDeVerificacionConfirmarEmail([FromBody] CorreoRequestDto correo)
        {
            return await _usuarioService.EnviarCodigoDeVerificacion(correo.Email);
        }

        [HttpPost("ComprobarCodigoDeRecuperarContraseña")]
        public async Task<IActionResult> ComprobarCodigoDeRecuperarContraseña([FromBody] CodigoDeVerificacionDto codigoDeVerificacion)
        {
            return await _usuarioService.ObtenerTokenCambioDeContraseña(codigoDeVerificacion);
        }

        [HttpPut("RecuperarContraseña")]
        public async Task<IActionResult> RecuperarContraseña([FromBody] CambioDeContraseñaDto cambioDeContraseñaDto)
        {
            return await _usuarioService.RecuperarContraseña(cambioDeContraseñaDto);
        }

        [HttpPost("ComprobarCodigoDeConfirmacionCorreo")]
        public async Task<IActionResult> ConfirmarCorreo([FromBody] CodigoDeVerificacionDto codigoDeVerificacion)
        {
            return await _usuarioService.ConfirmarCorreo(codigoDeVerificacion);
        }

        [HttpGet("ObtenerUsuariosEntrenadores")]
        public async Task<ActionResult<List<DatosUsuarioDto>>> ObtenerUsuariosEntrenadores()
        {
            try
            {
                var entrenadores = await _usuarioService.ObtenerUsuariosEntrenadoresAsync();
                return Ok(entrenadores);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener los usuarios entrenadores: {ex.Message}");
            }
        }


        [HttpGet("ObtenerDatosDelUsuario")]
        public async Task<IActionResult> ObtenerDatosDelUsuario(string email)
        {
            return await _usuarioService.ObtenerDatosDelUsuario(email);
        }

        [HttpPost("RegistrarComoSocio")]
        public async Task<IActionResult> RegistrarComoSocio(string email, string nombre, string apellido, string telefono)
        {
            return await _usuarioService.RegistrarSocio(email,nombre, apellido);
        }

        [HttpPost("RegistrarComoEntrenador")]
        public async Task<IActionResult> RegistrarComoEntrenador(string email, string nombre, string apellido, string telefono)
        {
            return await _usuarioService.RegistrarEntrenador(email, nombre, apellido);
        }

        [HttpGet("usuarios")]
        public async Task<IActionResult> ObtenerTodosLosUsuarios()
        {
            return await _usuarioService.ObtenerTodosLosUsuarios();
        }


        [HttpPut("usuario/{userId}")]
        public async Task<IActionResult> ActualizarUsuario(string userId, [FromBody] ActualizarUsuarioDto actualizarUsuarioDto)
        {
            return await _usuarioService.ActualizarUsuario(userId, actualizarUsuarioDto.NuevoRol);
        }

        [HttpDelete("usuario/{userId}")]
        public async Task<IActionResult> EliminarUsuario(string userId)
        {
            return await _usuarioService.EliminarUsuario(userId);
        }


        [HttpPost("RegistrarRutinaFinalizada")]
        public async Task<IActionResult> RegistrarRutinaFinalizada([FromBody] RutinasFinalizadasXUsuario rutina)
        {
            if (rutina == null || string.IsNullOrWhiteSpace(rutina.IdUser))
            {
                return BadRequest("Los datos enviados son inválidos o incompletos.");
            }

            // Verifica si el usuario existe en la tabla Socios
            var userExists = await _context.Socios.AnyAsync(u => u.UserId == rutina.IdUser);
            if (!userExists)
            {
                return NotFound($"No se encontró un usuario con el Id {rutina.IdUser}.");
            }

            try
            {
                // Agrega la rutina a la tabla
                _context.RutinasFinalizadasXUsuarios.Add(rutina);

                // Guarda los cambios en la base de datos
                await _context.SaveChangesAsync();

                return Ok("La rutina finalizada se registró exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al registrar la rutina: {ex.Message}");
            }
        }

        [HttpPost("RegistrarMedicionUsuario")]
        public async Task<IActionResult> RegistrarMendicionUsuario([FromBody] MedicionesUsuario medicion)
        {
            try
            {
                _context.MedicionesUsuario.Add(medicion);
                await _context.SaveChangesAsync();

                return Ok("Medicion agregado con exito");



            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpGet("TraerMedicionUsuarios")]
        public async Task<ActionResult<IEnumerable<MedicionesUsuario>>> ObtenerTodasLasMediciones()
        {
            try
            {
                var mediciones = await _context.MedicionesUsuario.ToListAsync();
                if (mediciones == null || !mediciones.Any())
                {
                    return NotFound("No se encontraron mediciones.");
                }

                return Ok(mediciones);
            }
            catch (Exception e)
            {
                return BadRequest($"Error: {e.Message}");
            }
        }
        // GET: api/Mediciones/{idUser}
        [HttpGet("{idUser}")]
        public async Task<ActionResult<IEnumerable<MedicionesUsuario>>> ObtenerMedicionPorIdUsuario(string idUser)
        {
            var mediciones = await _context.MedicionesUsuario
                .Where(m => m.IdUser == idUser)
                .ToListAsync();

            if (mediciones == null || !mediciones.Any())
            {
                return NotFound($"No se encontraron mediciones para el usuario con ID: {idUser}");
            }

            return Ok(mediciones);
        }

    }

}
