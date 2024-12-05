using Microsoft.AspNetCore.Mvc;
using ProgressusWebApi.Dtos.InventarioDtos;
using ProgressusWebApi.Services.InventarioServices.Interfaces;
using System.Threading.Tasks;

namespace ProgressusWebApi.Controllers.InventarioControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventarioController : ControllerBase
    {
        private readonly IInventarioService _inventarioService;

        // Constructor que inyecta el servicio de inventario
        public InventarioController(IInventarioService inventarioService)
        {
            _inventarioService = inventarioService;  // Asignación correcta
        }

        // Obtener todos los elementos del inventario
        [HttpGet("ObtenerTodos")]
        public async Task<IActionResult> ObtenerTodos()
        {
            var inventarios = await _inventarioService.ObtenerTodosInventariosAsync();
            return Ok(inventarios);
        }
        // Crear un nuevo elemento de inventario
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] InventarioDtos inventarioDto)
        {
            if (inventarioDto == null)
            {
                return BadRequest("Los datos del inventario son inválidos.");
            }

            // Llamamos al servicio para crear el inventario
            var inventarioCreado = await _inventarioService.CrearInventarioAsync(inventarioDto);
            if (inventarioCreado == null)
            {
                return BadRequest("Hubo un error al crear el inventario.");
            }

            return Ok("Ojeto.");
        }


        // Obtener un elemento de inventario por su Id
        [HttpGet("ObtenerPorId/{id}")]
        public async Task<IActionResult> ObtenerPorId(string id)
        {
            var inventario = await _inventarioService.ObtenerInventarioPorIdAsync(id);
            if (inventario == null) return NotFound();
            return Ok(inventario);
        }

        // Actualizar un elemento de inventario
        [HttpPut("Editar/{id}")]
        public async Task<IActionResult> Editar(string id, [FromBody] InventarioDtos inventarioDto)
        {
            if (inventarioDto == null)
            {
                return BadRequest("Los datos del inventario son inválidos.");
            }

            var inventarioActualizado = await _inventarioService.EditarInventarioAsync(id, inventarioDto);
            if (inventarioActualizado == null) return NotFound();
            return Ok(inventarioActualizado);
        }

        // Eliminar un elemento de inventario
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar(string id)
        {
            var inventarioEliminado = await _inventarioService.EliminarInventarioAsync(id);
            if (inventarioEliminado == null) return NotFound();
            return Ok(inventarioEliminado);
        }

        // Obtener elementos del inventario de mantenimiento
        [HttpGet("ObtenerInventarioMantenimiento")]
        public async Task<IActionResult> ObtenerInventarioMantenimiento()
        {
            var inventariosMantenimiento = await _inventarioService.ObtenerInventarioMantenimientoAsync();
            return Ok(inventariosMantenimiento);
        }
    }
}
