using ProgressusWebApi.Dtos.NotificacionDtos;
using ProgressusWebApi.Models.NotificacionesModel;
using ProgressusWebApi.Repositories.NotificacionesRepositories.Interfaces;
using ProgressusWebApi.Services.NotificacionesServices.Interfaces;

namespace ProgressusWebApi.Services.NotificacionesServices
{
	public class PlantillasService : IPlantillasService
	{
		private readonly IPlantillaRepository _plantillaRepository;

		public PlantillasService(IPlantillaRepository plantillaRepository)
		{
			_plantillaRepository = plantillaRepository;
		}

		public async Task<bool> CrearPlantillaAsync(InsertPlantillaNotificacionDto plantillaDto)
		{
			var plantilla = new PlantillaNotificacion
			{
				TipoNotificacionId = plantillaDto.TipoNotificacionId,
				RolId = plantillaDto.RolId,
				Titulo = plantillaDto.Titulo,
				Cuerpo = plantillaDto.Cuerpo,
				DiaSemana = plantillaDto.DiaSemana,
				MomentoDia = plantillaDto.MomentoDia
			};

			return await _plantillaRepository.CrearPlantillaAsync(plantilla);
		}

		public async Task<bool> ActualizarPlantillaAsync(int plantillaId, UpdatePlantillaNotificacionDto plantillaDto)
		{
			var plantillaExistente = await _plantillaRepository.ObtenerPlantillaPorIdAsync(plantillaId);
			if (plantillaExistente == null)
				throw new Exception("Plantilla no encontrada");

			plantillaExistente.TipoNotificacionId = plantillaDto.TipoNotificacionId;
			plantillaExistente.RolId = plantillaDto.RolId;
			plantillaExistente.Titulo = plantillaDto.Titulo;
			plantillaExistente.Cuerpo = plantillaDto.Cuerpo;
			plantillaExistente.DiaSemana = plantillaDto.DiaSemana;
			plantillaExistente.MomentoDia = plantillaDto.MomentoDia;

			return await _plantillaRepository.ActualizarPlantillaAsync(plantillaExistente);
		}

		public async Task<bool> EliminarPlantillaAsync(int plantillaId)
		{
			return await _plantillaRepository.EliminarPlantillaAsync(plantillaId);
		}

		public async Task<List<PlantillaNotificacionDto>> ObtenerPlantillasAsync()
		{
			var plantillas = await _plantillaRepository.ObtenerPlantillasAsync();
			return plantillas.Select(p => new PlantillaNotificacionDto
			{
				Id = p.Id,
				TipoNotificacionId = p.TipoNotificacionId,
				RolId = p.RolId,
				Titulo = p.Titulo,
				Cuerpo = p.Cuerpo,
				DiaSemana = p.DiaSemana,
				MomentoDia = p.MomentoDia
			}).ToList();
		}

		public async Task<PlantillaNotificacionDto> ObtenerPlantillaPorIdAsync(int plantillaId)
		{
			var plantilla = await _plantillaRepository.ObtenerPlantillaPorIdAsync(plantillaId);
			return new PlantillaNotificacionDto
			{
				Id = plantilla.Id,
				TipoNotificacionId = plantilla.TipoNotificacionId,
				RolId = plantilla.RolId,
				Titulo = plantilla.Titulo,
				Cuerpo = plantilla.Cuerpo,
				DiaSemana = plantilla.DiaSemana,
				MomentoDia = plantilla.MomentoDia
			};
		}
	}
}
