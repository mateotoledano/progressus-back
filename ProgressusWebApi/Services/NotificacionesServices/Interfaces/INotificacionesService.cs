using ProgressusWebApi.Models.NotificacionModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProgressusWebApi.Services.NotificacionesServices.Interfaces
{
    public interface INotificacionesService
    {
        Task<List<Notificacion>> ObtenerNotificacionesPorUsuarioAsync(string usuarioId);
        Task<bool> MarcarNotificacionComoLeidaAsync(int id);

    }

}
