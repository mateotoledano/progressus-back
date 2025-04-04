using ProgressusWebApi.Models.MerchModels;
using System.Collections.Concurrent;
using WebApiMercadoPago.Repositories.Interface;

namespace ProgressusWebApi.Services.CarritoServices
{
    public class CarritoService : ICarritoService
    {
        private static ConcurrentDictionary<string, Carrito> _carritos = new ConcurrentDictionary<string, Carrito>();

        public async Task<Carrito> ObtenerCarritoAsync(string usuarioId)
        {
            if (_carritos.TryGetValue(usuarioId, out var carrito))
            {
                // Calcular el total sumando los subtotales de todos los items
                carrito.Total = carrito.Items.Sum(item => item.Subtotal);
                return carrito;
            }

            // Si no existe, crear un nuevo carrito vacío
            // return new Carrito { UsuarioId = usuarioId };
            return new Carrito
            {
                UsuarioId = usuarioId,
                Items = new List<CarritoItem>(),
                Total = 0
            };
        }

        public async Task AgregarItemAlCarritoAsync(string usuarioId, CarritoItem item)
        {
            var carrito = await ObtenerCarritoAsync(usuarioId);
            var itemExistente = carrito.Items.FirstOrDefault(i => i.ProductoId == item.ProductoId);

            if (itemExistente != null)
            {
                itemExistente.Cantidad += item.Cantidad;
            }
            else
            {
                carrito.Items.Add(item);
            }

            carrito.Total = carrito.Items.Sum(i => i.Subtotal);
            _carritos[usuarioId] = carrito;
        }

        public async Task EliminarItemDelCarritoAsync(string usuarioId, string productoId)
        {
            if (!int.TryParse(productoId, out int proId))
                return;

            var carrito = await ObtenerCarritoAsync(usuarioId);
            var itemExistente = carrito.Items.FirstOrDefault(i => i.ProductoId == proId);

            if (itemExistente != null)
            {
                carrito.Items.Remove(itemExistente);
                carrito.Total = carrito.Items.Sum(i => i.Subtotal);
                _carritos[usuarioId] = carrito;
            }
        }

        public async Task VaciarCarritoAsync(string usuarioId)
        {
            _carritos.TryRemove(usuarioId, out _);
        }
    }
}

