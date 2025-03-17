using Microsoft.AspNetCore.Mvc;
using ProgressusWebApi.Dtos.MerchDtos;
using ProgressusWebApi.Models.MerchModels;
using ProgressusWebApi.Services.CarritoServices;
using ProgressusWebApi.Services.PedidosServices.Interfaces;
using WebApiMercadoPago.Services.Interface;

namespace ProgressusWebApi.Controllers.MerchControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagoController : ControllerBase
    {
        private readonly IMercadoPagoService _mercadoPagoService;
        private readonly IPedidoService _pedidoService;
        private readonly ICarritoService _carritoService;
        public PagoController(IMercadoPagoService mercadoPagoService, IPedidoService
        pedidoService, ICarritoService carritoService)
        {
            _mercadoPagoService = mercadoPagoService;
            _pedidoService = pedidoService;
            _carritoService = carritoService;
        }
        [HttpPost("CrearPago/{usuarioId}")]
        public async Task<IActionResult> CrearPago(string usuarioId, [FromBody] List<ItemCarritoDto>? items)
        {
            try
            {
                var pedido = await _pedidoService.RegistrarPedidoAsync(usuarioId, items);
                if (pedido == null)
                {
                    return StatusCode(500, "No se encontró su pedido u ocurrió un error al buscarlo.");
                }

                var preferencia = await _mercadoPagoService.CrearPreferenciaDePagoCarritoAsync(pedido.Carrito, pedido.Id);
                if (preferencia == null)
                {
                    return StatusCode(500, "Ocurrió un error al crear la preferencia de pago");
                }

                return Ok(new { Preference = preferencia, PedidoId = pedido.Id });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Ocurrió un error al procesar el pedido.");
            }
            
        }
    }

}