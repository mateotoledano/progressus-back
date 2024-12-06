using Microsoft.AspNetCore.Mvc;
using ProgressusWebApi.Dtos.MerchDtos;
using ProgressusWebApi.Services.MerchServices.Interfaces;

namespace ProgressusWebApi.Controllers.MerchControllers
{

        [Route("api/[controller]")]
        [ApiController]
        public class MerchController : ControllerBase
        {
            private readonly IMerchService _merchService;

            // Constructor que inyecta el servicio de merch
            public MerchController(IMerchService merchService)
            {
                _merchService = merchService; 
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
