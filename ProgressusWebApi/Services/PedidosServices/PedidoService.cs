using MercadoPago.Resource.Preference;
using ProgressusWebApi.DataContext;
using ProgressusWebApi.Dtos.MercadoPagoDtos;
using ProgressusWebApi.Dtos.MerchDtos;
using ProgressusWebApi.Models.MerchModels;
using ProgressusWebApi.Services.PedidosServices.Interfaces;
using WebApiMercadoPago.Services;
using WebApiMercadoPago.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ProgressusWebApi.Services.PedidosServices
{
    public class PedidoService : IPedidoService
    {
        private readonly ProgressusDataContext _context;
        private static List<Pedido> _pedidos = new List<Pedido>();

        public PedidoService(ProgressusDataContext context)
        {
            _context = context;
        }
        public async Task<Pedido> RegistrarPedidoAsync(string usuarioId, List<ItemCarritoDto> items)
        {
            if(items == null || items.Count == 0)
                return null;

            var itemIds = items.Select(p => p.Id).ToList();

            var merchItems = _context.Merch.Where(m => itemIds.Contains(m.Id) && m.Stock > 0).ToList();

            // Si falta algún merchitem o alguno no tiene stock
            if (merchItems.Count != itemIds.Count)
                return null;

            var itemsCarrito = new List<CarritoItem>();

            foreach (var item in items)
            {
                var merch = merchItems.FirstOrDefault(m => m.Id == item.Id);

                if (merch == null || merch.Stock < item.Cantidad)
                    return null;

                itemsCarrito.Add(
                    new CarritoItem()
                    {
                        ProductoId= item.Id,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = merch.Precio,                        
                    });
            }

            var carrito = new Carrito()
            {
                Id = Guid.NewGuid().ToString(),
                Total = itemsCarrito.Sum(i => i.PrecioUnitario * i.Cantidad),
                Items = itemsCarrito,
                UsuarioId = usuarioId,    
            };

            _context.Carrito.Add(carrito);

            var pedido = new Pedido
            {
                Id = Guid.NewGuid().ToString(),
                UsuarioId = usuarioId,
                CarritoId = carrito.Id,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Carrito = carrito,
                Total = carrito.Total,
                Estado = "Pendiente" // Acá debería ir a buscar el estado a algún lado pero el campo es un NVARCHAR de 100 :/ 
            };

            _context.Pedido.Add(pedido);

            _context.SaveChanges();

            return pedido;
        }

        public async Task<bool> RegistrarPago(string pedidoId)
        {
            try
            {
                var pedido = _context.Pedido.FirstOrDefault(p => p.Id == pedidoId);
                if (pedido == null)
                    return false;


                pedido.Estado = "Pagado";
                pedido.FechaActualizacion = DateTime.Now;
                
                _context.SaveChanges();


                return true;
            }
            catch
            {
                return false;
            }


        }
    }
}

