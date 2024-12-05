using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using ProgressusWebApi.DataContext;
using ProgressusWebApi.Dtos.AuthDtos;
using ProgressusWebApi.Models.EjercicioModels;
using ProgressusWebApi.Models.MembresiaModels;
using ProgressusWebApi.Services.AuthServices.Interfaces;
using ProgressusWebApi.Services.EmailServices.Interfaces;

namespace ProgressusWebApi.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        readonly IMemoryCache _memoryCache;
        readonly IEmailSenderService _emailSenderService;
        readonly UserManager<IdentityUser> _userManager;
        readonly ProgressusDataContext _progressusDataContext;
        readonly RoleManager<IdentityRole> _roleManager;


        public AuthService(IMemoryCache memoryCache, IEmailSenderService emailSenderService, UserManager<IdentityUser> userManager, ProgressusDataContext progressusDataContext, RoleManager<IdentityRole> roleManager)
        {
            _memoryCache = memoryCache;
            _emailSenderService = emailSenderService;
            _userManager = userManager;
            _progressusDataContext = progressusDataContext;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> EnviarCodigoDeVerificacion(string correo)
        {
            if (_memoryCache.TryGetValue(correo, out string codigoVerificacionExistente))
            {
                return new BadRequestObjectResult("El código para ese email ya se generó y se debe esperar 2 minutos.");
            }

            var codigoVerificacion = new Random().Next(1000, 9999).ToString();
            await _emailSenderService.SendEmail("Código de confirmación", codigoVerificacion, correo);
            _memoryCache.Set(correo, codigoVerificacion, TimeSpan.FromMinutes(2));

            return new OkObjectResult("El código de verificación se generó correctamente.");
        }

        public async Task<IActionResult> CambiarContraseña(string correo, string contraseñaNueva, string contraseñaActual)
        {
            try
            {
                IdentityUser? usuarioCambioContraseña = await _userManager.FindByEmailAsync(correo);
                if (!_userManager.CheckPasswordAsync(usuarioCambioContraseña, contraseñaActual).Result)
                {
                    return new BadRequestObjectResult("La contraseña es incorrecta");
                }
                var tokenCambioContraseña = await _userManager.GeneratePasswordResetTokenAsync(usuarioCambioContraseña);
                CambioDeContraseñaDto cambioDeContraseñaDto = new CambioDeContraseñaDto()
                {
                    Token = tokenCambioContraseña,
                    Email = correo,
                    nuevaContraseña = contraseñaNueva,
                };
                await this.RecuperarContraseña(cambioDeContraseñaDto);
                return new OkObjectResult(usuarioCambioContraseña);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }

        }

        public async Task<IActionResult?> ConfirmarCorreo(CodigoDeVerificacionDto codigoDeVerificacion)
        {
            try
            {
                if (ComprobarCodigoDeVerificacion(codigoDeVerificacion).Result == false)
                {
                    return null;
                }

                IdentityUser? usuarioAConfirmar = await _userManager.FindByEmailAsync(codigoDeVerificacion.Email);
                if (usuarioAConfirmar == null)
                {
                    return null;
                }

                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(usuarioAConfirmar);
                var result = await _userManager.ConfirmEmailAsync(usuarioAConfirmar, confirmationToken);
                if (!result.Succeeded)
                {
                    return null; // Fallo en la confirmación del correo
                }

                return new OkObjectResult("Email de usuario confirmado");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }

        public async Task<IActionResult?> ObtenerTokenCambioDeContraseña(CodigoDeVerificacionDto codigoDeVerificacion)
        {
            try
            {
                if (ComprobarCodigoDeVerificacion(codigoDeVerificacion).Result == false)
                {
                    return null;
                }

                IdentityUser? usuarioCambioContraseña = await _userManager.FindByEmailAsync(codigoDeVerificacion.Email);
                if (usuarioCambioContraseña == null)
                {
                    return null;
                }
                CambioDeContraseñaDto cambioDeContraseña = new CambioDeContraseñaDto()
                {
                    Token = await _userManager.GeneratePasswordResetTokenAsync(usuarioCambioContraseña),
                    Email = codigoDeVerificacion.Email,
                };


                return new OkObjectResult(cambioDeContraseña);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }

        public async Task<IActionResult> RecuperarContraseña(CambioDeContraseñaDto cambioDeContraseñaDto)
        {
            try
            {
                IdentityUser? usuarioCambioContraseña = await _userManager.FindByEmailAsync(cambioDeContraseñaDto.Email);
                await _userManager.ResetPasswordAsync(usuarioCambioContraseña, cambioDeContraseñaDto.Token, cambioDeContraseñaDto.nuevaContraseña);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }
        public async Task<bool> ComprobarCodigoDeVerificacion(CodigoDeVerificacionDto codigoDeVerificacion)
        {
            try
            {
                if (!_memoryCache.TryGetValue(codigoDeVerificacion.Email, out string codigoDeVerificacionEnCache))
                {
                    return false;
                }

                if (codigoDeVerificacionEnCache != codigoDeVerificacion.Codigo)
                {
                    return false;
                }

                _memoryCache.Remove(codigoDeVerificacion.Email); // Eliminar el código de la caché
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IActionResult> RegistrarEntrenador(string email, string nombre, string apellido)
        {
            IdentityUser? usuarioARegistrar = await _userManager.FindByEmailAsync(email);
            Entrenador entrenador = new Entrenador()
            {
                User = usuarioARegistrar,
                UserId = usuarioARegistrar.Id,
                Nombre = nombre,
                Apellido = apellido
            };
            try
            {
                _progressusDataContext.Entrenadores.Add(entrenador);
                await _progressusDataContext.SaveChangesAsync();
                await _userManager.AddToRoleAsync(usuarioARegistrar, "ENTRENADOR");
                return new OkObjectResult(entrenador);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }

        public async Task<IActionResult> RegistrarSocio(string email, string nombre, string apellido)
        {
            IdentityUser? usuarioARegistrar = await _userManager.FindByEmailAsync(email);
            Socio socio = new Socio()
            {
                User = usuarioARegistrar,
                UserId = usuarioARegistrar.Id,
                Nombre = nombre,
                Apellido = apellido
            };
            try
            {
                _progressusDataContext.Socios.Add(socio);
                await _progressusDataContext.SaveChangesAsync();
                await _userManager.AddToRoleAsync(usuarioARegistrar, "SOCIO");
                return new OkObjectResult(socio);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }

        public async Task<IActionResult> ObtenerDatosDelUsuario(string email)
        {
            IdentityUser usuario = await _userManager.FindByEmailAsync(email);
            var socio = await _progressusDataContext.Socios.FirstOrDefaultAsync(e => e.UserId == usuario.Id);
            if (socio == null)
            {
                var entrenador = await _progressusDataContext.Entrenadores.FirstOrDefaultAsync(e => e.UserId == usuario.Id);
                DatosUsuarioDto datosDelEntrenador = new DatosUsuarioDto()
                {
                    IdentityUserId = usuario.Id,
                    Nombre = entrenador.Nombre,
                    Apellido = entrenador.Apellido,
                    Telefono = entrenador.Telefono,
                    Roles = _userManager.GetRolesAsync(usuario).Result.ToList(),
                    Email = email,

                };
                return new OkObjectResult(datosDelEntrenador);
            }
            DatosUsuarioDto datosDelSocio = new DatosUsuarioDto()
            {
                IdentityUserId = usuario.Id,
                Nombre = socio.Nombre,
                Apellido = socio.Apellido,
                Telefono = socio.Telefono,
                Roles = _userManager.GetRolesAsync(usuario).Result.ToList(),
                Email = email,
            };

            return new OkObjectResult(datosDelSocio);
        }

        public async Task<IActionResult> ObtenerTodosLosUsuarios()
        {
            try
            {
                // Materializar usuarios y socios desde la base de datos
                var usuarios = await _progressusDataContext.Users.ToListAsync();
                var socios = await _progressusDataContext.Socios.ToListAsync();

                // Crear la lista de usuarios con detalles
                var usuariosConDetalles = new List<DatosUsuarioDto>();

                foreach (var user in usuarios)
                {
                    // Buscar el socio correspondiente (si existe)
                    var socio = socios.FirstOrDefault(s => s.UserId == user.Id);

                    // Obtener roles del usuario
                    var roles = (await _userManager.GetRolesAsync(user)).ToList();

                    // Crear el DTO del usuario
                    usuariosConDetalles.Add(new DatosUsuarioDto
                    {
                        IdentityUserId = user.Id,
                        Email = user.Email,
                        Roles = roles,
                        Nombre = socio?.Nombre,
                        Apellido = socio?.Apellido,
                        Telefono = socio?.Telefono
                    });
                }

                return new OkObjectResult(usuariosConDetalles);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        public async Task<List<DatosUsuarioDto>> ObtenerUsuariosEntrenadoresAsync()
        {
            // Materializar usuarios y socios desde la base de datos
            var usuarios = await _progressusDataContext.Users.ToListAsync();
            var socios = await _progressusDataContext.Socios.ToListAsync();

            // Crear la lista de usuarios con detalles
            var usuariosConDetalles = new List<DatosUsuarioDto>();

            foreach (var user in usuarios)
            {
                // Buscar el socio correspondiente (si existe)
                var socio = socios.FirstOrDefault(s => s.UserId == user.Id);

                // Obtener roles del usuario
                var roles = (await _userManager.GetRolesAsync(user)).ToList();

                // Filtrar por rol de "Entrenador"
                if (roles.Contains("Entrenador"))
                {
                    usuariosConDetalles.Add(new DatosUsuarioDto
                    {
                        IdentityUserId = user.Id,
                        Email = user.Email,
                        Roles = roles,
                        Nombre = socio?.Nombre,
                        Apellido = socio?.Apellido,
                        Telefono = socio?.Telefono
                    });
                }
            }

            return usuariosConDetalles;
        }


        public async Task<IActionResult> ActualizarUsuario(string userId, string nuevoRol)
        {
            try
            {
                // Buscar el usuario por su ID
                IdentityUser usuario = await _userManager.FindByIdAsync(userId);
                if (usuario == null)
                {
                    return new NotFoundObjectResult("Usuario no encontrado.");
                }

                // Actualizar el rol si se proporciona un nuevo rol
                if (!string.IsNullOrEmpty(nuevoRol))
                {
                    // Buscar el RoleId basado en el nombre del rol
                    var rol = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == nuevoRol);
                    if (rol == null)
                    {
                        return new NotFoundObjectResult("El rol especificado no existe.");
                    }

                    var roleId = rol.Id;

                    // Actualizar la relación en la tabla AspNetUserRoles
                    using (var transaction = await _progressusDataContext.Database.BeginTransactionAsync())
                    {
                        // Eliminar los roles actuales del usuario
                        var rolesActuales = _progressusDataContext.UserRoles.Where(ur => ur.UserId == userId);
                        _progressusDataContext.UserRoles.RemoveRange(rolesActuales);
                        await _progressusDataContext.SaveChangesAsync();

                        // Agregar el nuevo rol
                        _progressusDataContext.UserRoles.Add(new IdentityUserRole<string>
                        {
                            UserId = userId,
                            RoleId = roleId
                        });
                        await _progressusDataContext.SaveChangesAsync();

                        await transaction.CommitAsync();
                    }
                }

                return new OkObjectResult("Usuario actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }


        public async Task<IActionResult> EliminarUsuario(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return new NotFoundObjectResult("Usuario no encontrado.");
                }

                // Eliminar todos los logs de asistencia asociados al usuario
                var asistenciaLogs = _progressusDataContext.AsistenciaLogs.Where(a => a.UserId == userId);
                if (asistenciaLogs.Any())
                {
                    _progressusDataContext.AsistenciaLogs.RemoveRange(asistenciaLogs);
                    await _progressusDataContext.SaveChangesAsync();
                }

                // Eliminar todas las reservas asociadas al usuario
                var reservas = _progressusDataContext.Reservas.Where(r => r.UserId == userId);
                if (reservas.Any())
                {
                    _progressusDataContext.Reservas.RemoveRange(reservas);
                    await _progressusDataContext.SaveChangesAsync();
                }

                // Verificar y eliminar referencias relacionadas en la tabla socios
                var socio = await _progressusDataContext.Socios.FirstOrDefaultAsync(s => s.UserId == userId);
                if (socio != null)
                {
                    _progressusDataContext.Socios.Remove(socio);
                    await _progressusDataContext.SaveChangesAsync();
                }

                // Eliminar el usuario en AspNetUsers
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return new BadRequestObjectResult($"No se pudo eliminar el usuario. " +
                                                      $"{string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                return new OkObjectResult("Usuario eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}