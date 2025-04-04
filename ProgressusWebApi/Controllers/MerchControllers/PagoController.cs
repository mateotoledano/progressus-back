using Microsoft.AspNetCore.Mvc;
using ProgressusWebApi.DataContext;
using ProgressusWebApi.Dtos.MerchDtos;
using ProgressusWebApi.Models.MerchModels;
using ProgressusWebApi.Services.CarritoServices;
using ProgressusWebApi.Services.PedidosServices.Interfaces;
using WebApiMercadoPago.Services.Interface;
using Microsoft.EntityFrameworkCore;
namespace ProgressusWebApi.Controllers.MerchControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagoController : ControllerBase
    {
        private readonly IMercadoPagoService _mercadoPagoService;
        private readonly IPedidoService _pedidoService;
        private readonly ICarritoService _carritoService;
        private readonly ProgressusDataContext _context;
        public PagoController(IMercadoPagoService mercadoPagoService, IPedidoService
        pedidoService, ICarritoService carritoService, ProgressusDataContext context)
        {
            _mercadoPagoService = mercadoPagoService;
            _pedidoService = pedidoService;
            _carritoService = carritoService;
            _context = context;
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
        [HttpGet("ObtenerPedidos")]
        public async Task<IActionResult> ObtenerPedidos()
        {
            try
            {
                var pedidos = await _context.Pedido
                    .Include(p => p.Carrito)
                    .ThenInclude(c => c.Items)
                    .ToListAsync();

                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Ocurrió un error al obtener los pedidos.");
            }
        }

        [HttpGet("ObtenerPedidoPorId/{pedidoId}")]
        public async Task<IActionResult> ObtenerPedidoPorId(string pedidoId)
        {
            try
            {
                var pedido = await _context.Pedido
                    .Include(p => p.Carrito)
                    .ThenInclude(c => c.Items)
                    .FirstOrDefaultAsync(p => p.Id == pedidoId);

                if (pedido == null)
                {
                    return NotFound($"No se encontró el pedido con ID {pedidoId}");
                }

                return Ok(pedido);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Ocurrió un error al obtener el pedido con ID {pedidoId}");
            }
        }

        [HttpGet("ObtenerPedidosPorUsuario/{usuarioId}")]
        public async Task<IActionResult> ObtenerPedidosPorUsuario(string usuarioId)
        {
            try
            {
                var pedidos = await _context.Pedido
                    .Include(p => p.Carrito)
                    .ThenInclude(c => c.Items)
                    .Where(p => p.UsuarioId == usuarioId)
                    .ToListAsync();

                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Ocurrió un error al obtener los pedidos del usuario {usuarioId}");
            }
        }

        [HttpGet("ObtenerEstadosPedido")]
        public async Task<IActionResult> ObtenerEstadosPedido()
        {
            try
            {
                var estados = await _context.Pedido
                    .Select(p => p.Estado)
                    .Distinct()
                    .ToListAsync();

                return Ok(estados);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Ocurrió un error al obtener los estados de pedido");
            }
        }

        [HttpPut("ActualizarEstadoPedido/{pedidoId}")]
        public async Task<IActionResult> ActualizarEstadoPedido(string pedidoId, [FromBody] string nuevoEstado)
        {
            try
            {
                var pedido = await _context.Pedido.FindAsync(pedidoId);
                if (pedido == null)
                {
                    return NotFound($"No se encontró el pedido con ID {pedidoId}");
                }

                pedido.Estado = nuevoEstado;
                pedido.FechaActualizacion = DateTime.UtcNow;

                _context.Pedido.Update(pedido);
                await _context.SaveChangesAsync();

                return Ok(pedido);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Ocurrió un error al actualizar el estado del pedido {pedidoId}");
            }
        }
    }



}

