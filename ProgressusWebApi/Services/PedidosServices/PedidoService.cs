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

            // Uso AsNoTracking así el cambio en el stock solo se guarda después del pago
            var merchItems = _context.Merch.AsNoTracking().Where(m => itemIds.Contains(m.Id) && m.Stock > 0).ToList();

            // Si falta algún merch item o alguno no tiene stock
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

                // Recordenoms que esto no se tiene que guardar en la BD
                // Lo hago por si llegara algún item repetido
                merch.Stock -= item.Cantidad;
            }

            var carrito = new Carrito()
            {
                Id = Guid.NewGuid().ToString(),
                Total = itemsCarrito.Sum(i => i.PrecioUnitario * i.Cantidad),
                Items = itemsCarrito,
                UsuarioId = usuarioId,    
            };

            _context.Carrito.Add(carrito);

            var pedidoId = Guid.NewGuid().ToString();
            var pedido = new Pedido
            {
                Id = pedidoId,
                UsuarioId = usuarioId,
                CarritoId = carrito.Id,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                Carrito = carrito,
                Total = carrito.Total,
                Estado = "pagado" // Acá debería ir a buscar el estado a algún lado pero el campo es un NVARCHAR de 100 :/ 
            };

            _context.Pedido.Add(pedido);

            _context.SaveChanges();

            // Lo busco nuevamente para tener disponible las descripciones del merch
            var nuevo = _context.Pedido.Include(p => p.Carrito.Items).ThenInclude(p => p.Merch).FirstOrDefault(p => p.Id == pedidoId);
            return nuevo;
        }

        public async Task<bool> RegistrarPago(string pedidoId)
        {
            try
            {
                var pedido = _context.Pedido.Include(p => p.Carrito.Items).ThenInclude(p => p.Merch)
                                .FirstOrDefault(p => p.Id == pedidoId);
                
                if (pedido == null)
                    return false;

                if (pedido.Estado == "Pagado") // por si MP manda + de 1 vez el request
                    return true;


                pedido.Estado = "Pagado";
                pedido.FechaActualizacion = DateTime.Now;

                pedido.Carrito.Items.ForEach(p => {
                    p.Merch.Stock -= p.Cantidad;                
                });
                
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

