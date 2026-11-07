using CentroUnas.Domain.Entities;
using CentroUnas.Infrastructure.Context;
using CentroUnas.Infrastructure.Core;
using CentroUnas.Infrastructure.Exceptions;
using CentroUnas.Infrastructure.Interfaces;
using CentroUnas.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CentroUnas.Infrastructure.Repositories
{
    public class ServicioRepository : BaseRepository<Servicio>, IServicioRepository
    {
        public ServicioRepository(CentroUnasContext context) : base(context)
        {
        }

        public async Task<List<ServicioModel>> GetAllServiciosAsync()
        {
            var servicios = await GetAll();
            return servicios.Select(ToModel).ToList();
        }

        public async Task<ServicioModel?> GetServicioByIdAsync(int id)
        {
            var servicio = await GetById(id);
            return servicio == null ? null : ToModel(servicio);
        }

        public async Task<ServicioModel> CreateServicioAsync(ServicioModel model)
        {
            var servicio = new Servicio
            {
                Nombre = model.Nombre,
                Descripcion = model.Descripcion,
                Precio = model.Precio,
                DuracionMinutos = model.DuracionMinutos,
                Activo = model.Activo
            };

            await Create(servicio);
            return ToModel(servicio);
        }

        public async Task<bool> UpdateServicioAsync(int id, ServicioModel model)
        {
            var servicio = await GetById(id);
            if (servicio == null)
            {
                return false;
            }

            servicio.Nombre = model.Nombre;
            servicio.Descripcion = model.Descripcion;
            servicio.Precio = model.Precio;
            servicio.DuracionMinutos = model.DuracionMinutos;
            servicio.Activo = model.Activo;

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

        private static ServicioModel ToModel(Servicio servicio)
        {
            return new ServicioModel
            {
                ServicioId = servicio.Id,
                Nombre = servicio.Nombre,
                Descripcion = servicio.Descripcion,
                Precio = servicio.Precio,
                DuracionMinutos = servicio.DuracionMinutos,
                Activo = servicio.Activo
            };
        }
    }
}
