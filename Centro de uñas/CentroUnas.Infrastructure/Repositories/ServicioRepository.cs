using CentroUnas.Domain.Entities;
using CentroUnas.Infrastructure.Context;
using CentroUnas.Infrastructure.Core;
using CentroUnas.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CentroUnas.Infrastructure.Repositories
{
    public class ServicioRepository : BaseRepository<Servicio>
    {
        public ServicioRepository(CentroUnasContext context) : base(context)
        {
        }

        public async Task<Servicio> CreateServicioAsync(Servicio datos)
        {
            var servicio = new Servicio
            {
                Nombre = datos.Nombre,
                Descripcion = datos.Descripcion,
                Precio = datos.Precio,
                DuracionMinutos = datos.DuracionMinutos,
                Activo = datos.Activo
            };

            return await Create(servicio);
        }

        public async Task<bool> UpdateServicioAsync(int id, Servicio datos)
        {
            var servicio = await GetById(id);
            if (servicio == null)
            {
                return false;
            }

            servicio.Nombre = datos.Nombre;
            servicio.Descripcion = datos.Descripcion;
            servicio.Precio = datos.Precio;
            servicio.DuracionMinutos = datos.DuracionMinutos;
            servicio.Activo = datos.Activo;

            await Update(servicio);
            return true;
        }

        public async Task<bool> DeleteServicioAsync(int id)
        {
            var servicio = await GetById(id);
            if (servicio == null)
            {
                return false;
            }

            var turnos = await _context.Turnos.CountAsync(t => t.ServicioId == id);
            if (turnos > 0)
            {
                throw new ServicioException($"No se puede eliminar el servicio porque tiene {turnos} turno(s) asociado(s)");
            }

            await Delete(id);
            return true;
        }
    }
}
