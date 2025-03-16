using Microsoft.AspNetCore.Mvc;
using ProgressusWebApi.Models.MerchModels;
using ProgressusWebApi.Services.CarritoServices;

namespace ProgressusWebApi.Controllers.MerchControllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CarritoController : ControllerBase
    {
        private readonly ICarritoService _carritoService;

        public CarritoController(ICarritoService carritoService)
        {
            _carritoService = carritoService;
        }

        [HttpGet("ObtenerCarrito/{usuarioId}")]
        public async Task<IActionResult> ObtenerCarrito(string usuarioId)
        {
            var carrito = await _carritoService.ObtenerCarritoAsync(usuarioId);
            return Ok(carrito);
        }

        [HttpPost("AgregarItem/{usuarioId}")]
        public async Task<IActionResult> AgregarItem(string usuarioId, [FromBody] CarritoItem item)
        {
            await _carritoService.AgregarItemAlCarritoAsync(usuarioId, item);
            return Ok();
        }

        [HttpDelete("EliminarItem/{usuarioId}/{productoId}")]
        public async Task<IActionResult> EliminarItem(string usuarioId, string productoId)
        {
            await _carritoService.EliminarItemDelCarritoAsync(usuarioId, productoId);
            return Ok();
        }

        [HttpDelete("VaciarCarrito/{usuarioId}")]
        public async Task<IActionResult> VaciarCarrito(string usuarioId)
        {
            await _carritoService.VaciarCarritoAsync(usuarioId);
            return Ok();
        }
    }
}

