using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgressusWebApi.DataContext;
using ProgressusWebApi.Dtos.MerchDtos;
using System.Linq;
using ProgressusWebApi.Services.MerchServices.Interfaces;

namespace ProgressusWebApi.Controllers.MerchControllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class MerchController : ControllerBase
	{
		private readonly IMerchService _merchService;
		private ProgressusDataContext _context;

		// Constructor que inyecta el servicio de merch
		public MerchController(IMerchService merchService, ProgressusDataContext context)
		{
			_merchService = merchService;
			_context = context;
		}

		// Obtener todos los elementos del merch
		[HttpGet("ObtenerTodos")]
		public async Task<IActionResult> ObtenerTodos()
		{
			var merchs = await _merchService.ObtenerTodosMerchAsync();
			return Ok(merchs);
		}
		// Crear un nuevo elemento de merch
		[HttpPost("Crear")]
		public async Task<IActionResult> Crear([FromBody] MerchDtos merchDto)
		{
			if (merchDto == null)
			{
				return BadRequest("Los datos del merch son inválidos.");
			}

			// Llamamos al servicio para crear el merch
			var merchCreado = await _merchService.CrearMerchAsync(merchDto);
			if (merchCreado == null)
			{
				return BadRequest("Hubo un error al crear el inventario.");
			}

			return Ok("Ojeto.");
		}


		// Obtener un elemento de merch por su Id
		[HttpGet("ObtenerPorId/{id}")]
		public async Task<IActionResult> ObtenerPorId(string id)
		{
			var merch = await _merchService.ObtenerMerchPorIdAsync(id);
			if (merch == null) return NotFound();
			return Ok(merch);
		}

		[HttpPost("ObtenerPorIds")]
		public async Task<IActionResult> ObtenerPorIds([FromBody] string[] ids)
		{
			if (ids == null || ids.Length == 0)
			{
				return BadRequest("Debe proporcionar al menos un ID");
			}

			try
			{
				// Buscar los items que coincidan con los IDs proporcionados
				var merchItems = await _context.Merch
.Where(m => ids.Contains(m.Id.ToString())) // Si Id es numérico
.ToListAsync();

				if (merchItems == null || !merchItems.Any())
				{
					return NotFound("No se encontraron items con los IDs proporcionados");
				}

				return Ok(merchItems);
			} catch (Exception ex)
			{
				return StatusCode(500, $"Error interno del servidor: {ex.Message}");
			}
		}

		// Actualizar un elemento de merch
		[HttpPut("Editar/{id}")]
		public async Task<IActionResult> Editar(string id, [FromBody] MerchDtos merchDto)
		{
			if (merchDto == null)
			{
				return BadRequest("Los datos del merch son inválidos.");
			}

			var merchActualizado = await _merchService.EditarMerchAsync(id, merchDto);
			if (merchActualizado == null) return NotFound();
			return Ok(merchActualizado);
		}

		// Eliminar un elemento de merch
		[HttpDelete("Eliminar/{id}")]
		public async Task<IActionResult> Eliminar(string id)
		{
			var merchEliminado = await _merchService.EliminarMerchAsync(id);
			if (merchEliminado == null) return NotFound();
			return Ok(merchEliminado);
		}

		// Endpoint para obtener todas las categorías de Merch
		[HttpGet("ObtenerCategorias")]
		public async Task<IActionResult> ObtenerCategorias()
		{
			var categorias = await _merchService.ObtenerCategoriasAsync();
			return Ok(categorias);
		}
	}
}