using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProgressusWebApi.DataContext;
using ProgressusWebApi.Dtos.NotificacionDtos;
using ProgressusWebApi.Models.NotificacionesModel;
using ProgressusWebApi.Repositories.NotificacionesRepositories.Interfaces;
using ProgressusWebApi.Request;
using ProgressusWebApi.Services.AuthServices.Interfaces;
using ProgressusWebApi.Services.NotificacionesServices.Interfaces;
using System.ComponentModel;

namespace ProgressusWebApi.Services.NotificacionesServices
{
	public class NotificacionesUsuariosService : INotificacionesUsuariosService
	{
		private readonly INotificacionRepository _notificacionRepository;
		private readonly IPlantillaRepository _plantillaRepository;
		private readonly IEstadoNotificacionRepository _estadoNotificacionRepository;
		private readonly ProgressusDataContext _context;

		public NotificacionesUsuariosService(INotificacionRepository notificacionRepository,IPlantillaRepository plantillaRepository,IEstadoNotificacionRepository estadoNotificacionRepository, ProgressusDataContext context)
		{
			_notificacionRepository = notificacionRepository;
			_plantillaRepository = plantillaRepository;
			_estadoNotificacionRepository = estadoNotificacionRepository;
			_context = context;
		}

		public async Task<List<NotificacionDto>> ObtenerNotificacionesPorUsuarioAsync(string usuarioId)
		{
			var notificaciones = await _notificacionRepository.ObtenerNotificacionesPorUsuarioAsync(usuarioId);
			return notificaciones.Select(n => new NotificacionDto
			{
				Id = n.Id,
				Titulo = n.PlantillaNotificacion.Titulo,
				Cuerpo = n.Cuerpo,
				Estado = n.EstadoNotificacion.Nombre,
				Usuario = n.UsuarioId,
				FechaCreacion = n.FechaCreacion,
				FechaEnvio = n.FechaEnvio
			}).ToList();
		}

		public async Task<bool> CrearNotificacionAsync(int plantillaId, string usuarioId)
		{
			var plantilla = await _plantillaRepository.ObtenerPlantillaPorIdAsync(plantillaId);
			if (plantilla == null)
				throw new Exception("Plantilla no encontrada");

            var usuario = await _context.Users.FirstOrDefaultAsync(u => u.Id == usuarioId);

			if (usuario == null)
				throw new Exception("Usuario no encontrado");

            string cuerpoPersonalizado = plantilla.Cuerpo
                .Replace("[Nombre]", usuario.UserName ?? "Usuario");

            return await GuardarNotificacion(usuarioId, plantilla, cuerpoPersonalizado);
		}

		private Task<bool> GuardarNotificacion(string usuario, PlantillaNotificacion plantilla, string cuerpo)
		{

            var estadoPendiente = _estadoNotificacionRepository
                .ObtenerEstadosNotificacionesAsync()
                .ContinueWith(t => t.Result.FirstOrDefault(e => e.Nombre == "Pendiente")).Result;

            if (estadoPendiente == null)
                throw new Exception("Estado 'Pendiente' no encontrado");


            var notificacion = new Notificacion
            {
                PlantillaNotificacionId = plantilla.Id,
                UsuarioId = usuario,
                EstadoNotificacionId = estadoPendiente.Id,
                FechaCreacion = DateTime.UtcNow,
                Titulo = plantilla.Titulo,
                Cuerpo = cuerpo
            };

            return _notificacionRepository.CrearNotificacionAsync(notificacion);
        }

		public async Task<bool> CambiarEstadoNotificacionAsync(int notificacionId, string nuevoEstado)
		{
			return await _notificacionRepository.CambiarEstadoNotificacionAsync(notificacionId, nuevoEstado);
		}

		public async Task<bool> EliminarNotificacionAsync(int notificacionId)
		{
			return await _notificacionRepository.EliminarNotificacionAsync(notificacionId);
		}

        public async Task<bool> EnviarNotificacionesPendientes()
        {
            var notificaciones = _notificacionRepository.ObtenerNotificacionesPendientesAsync().Result;

            var mapeoDias = new List<DiaModel>(){
                new (){
                    Id = (int)DayOfWeek.Monday,
                    Nombre = "lunes"
                },
                new (){
                    Id = (int)DayOfWeek.Tuesday,
                    Nombre = "martes"
                },
                new (){
                    Id = (int)DayOfWeek.Wednesday,
                    Nombre = "miércoles"
                },
                new (){
                    Id = (int)DayOfWeek.Thursday,
                    Nombre = "jueves"
                },
                new (){
                    Id = (int)DayOfWeek.Friday,
                    Nombre = "viernes"
                },
                new (){
                    Id = (int)DayOfWeek.Saturday,
                    Nombre = "sábado"
                },
                new (){
                    Id = (int)DayOfWeek.Sunday,
                    Nombre = "Domingo"
                }
            };
            var momentoDia = DateTime.Now.Hour <= 12 ? "Mañana" : "Tarde";
            var pendiente = _estadoNotificacionRepository.ObtenerEstadosNotificacionesAsync().ContinueWith(n => n.Result.FirstOrDefault(p => p.Nombre == "Pendiente"))?.Result;
            if (pendiente == null)
                return false;

            var diaActual = mapeoDias.FirstOrDefault(d => d.Id == (int)DateTime.Now.DayOfWeek)?.Nombre;
            var idNotificaciones = notificaciones.Where(n => n.PlantillaNotificacion.DiaSemana == diaActual)
                                    .Where(n => string.IsNullOrEmpty(n.PlantillaNotificacion.MomentoDia) || n.PlantillaNotificacion.MomentoDia == momentoDia)
                                    .Where(n => n.EstadoNotificacionId == pendiente.Id)
                                    .Select(n => n.Id)
                                    .ToList();

            var ok = await _notificacionRepository.CambiarEstadoNotifiacionesMasivo(idNotificaciones, pendiente.Id);

            return ok;
        }

        public async Task<bool> NotificarActualizacionPlan(List<string> usuariosId)
        {
			var plantilla = _plantillaRepository.ObtenerPlantillaPorIdAsync(5);
			if (plantilla == null)
				return false;
			
			foreach (var id in usuariosId)
			{
				await CrearNotificacionAsync(plantilla.Id, id);
            }


			return true;
        }

        public async Task<bool> NotificarMaquinaEnMantenimiento(List<string> usuariosId, string maquina, int dias, string motivo)
        {
            try
            {
                var plantilla = _plantillaRepository.ObtenerPlantillaPorIdAsync(6).Result;
                if (plantilla == null)
                    return false;

                string cuerpo = plantilla.Cuerpo.Replace("[Maquina]", maquina);
                cuerpo = cuerpo.Replace("[Dias]", dias.ToString());

                var usuarios = _context.Users.Where(u => usuariosId.Contains(u.Id)).ToList();

                foreach (var usuario in usuarios)
                {
                    await GuardarNotificacion(usuario.Id, plantilla, cuerpo.Replace("[Nombre]", usuario.UserName));
                }



                return true;
            }
            catch
            {
                return false;
            }            
        }

        public async Task<bool> NotificarMembresiaPorVencer(string usuarioId, string fechaVencimiento, List<string> planes)
        {
            try
            {
                var plantilla = _plantillaRepository.ObtenerPlantillaPorIdAsync(7).Result;
                if (plantilla == null)
                    return false;

                var usuario = _context.Users.FirstOrDefault(u => u.Id == usuarioId).UserName;

                string cuerpo = plantilla.Cuerpo.Replace("[Nombre]", usuario);
                cuerpo = cuerpo.Replace("[Vencimiento]", fechaVencimiento);

                var inicioFor = cuerpo.IndexOf("[*for]");
                var finFor = cuerpo.IndexOf("[*end]");

                // Si tenemos que mostrar los planes
                if(inicioFor != -1 || finFor != -1)
                {
                    var planCuerpo = " ";
                    var contador = 1;
                    foreach (var plan in planes)
                    {
                        planCuerpo = $"{contador.ToString()}. {plan}\n";
                    }
                    cuerpo = cuerpo.Substring(0, inicioFor) + planCuerpo + cuerpo.Substring(finFor);
                }
                cuerpo = cuerpo.Replace("[*for]", "");
                cuerpo = cuerpo.Replace("[*end]", "");

                await GuardarNotificacion(usuarioId, plantilla, cuerpo);



                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> CrearNotificacionMasivaAsync(int plantillaId)
        {
            var plantilla = await _plantillaRepository.ObtenerPlantillaPorIdAsync(plantillaId);
            if (plantilla == null)
                throw new Exception("Plantilla no encontrada");

            var usuarios = _context.Users.Where(u => u.EmailConfirmed).Select(u => new { u.UserName, u.Id });
            foreach (var usuario in usuarios)
            {
                string cuerpoPersonalizado = plantilla.Cuerpo
                        .Replace("[Nombre]", usuario.UserName ?? "Usuario");

                await GuardarNotificacion(usuario.Id, plantilla, cuerpoPersonalizado);
            }

            return true;
        }
    }


}
