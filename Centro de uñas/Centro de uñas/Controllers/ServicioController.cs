using CentroUnas.Domain.Entities;
using CentroUnas.Infrastructure.Exceptions;
using CentroUnas.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Centro_de_uñas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicioController : ControllerBase
    {
        private readonly ServicioRepository _repository;

        public ServicioController(ServicioRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServicios()
        {
            var servicios = await _repository.GetAll();
            return Ok(servicios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServicioById(int id)
        {
            var servicio = await _repository.GetById(id);
            if (servicio == null)
            {
                return NotFound($"Servicio con id {id} no encontrado");
            }

            return Ok(servicio);
        }

        [HttpPost]
        public async Task<IActionResult> CreateServicio([FromBody] Servicio datos)
        {
            var servicio = await _repository.CreateServicioAsync(datos);
            return CreatedAtAction(nameof(GetServicioById), new { id = servicio.Id }, servicio);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServicio(int id, [FromBody] Servicio datos)
        {
            var actualizado = await _repository.UpdateServicioAsync(id, datos);
            if (!actualizado)
            {
                return NotFound($"Servicio con id {id} no encontrado");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicio(int id)
        {
            try
            {
                var eliminado = await _repository.DeleteServicioAsync(id);
                if (!eliminado)
                {
                    return NotFound($"Servicio con id {id} no encontrado");
                }

                return NoContent();
            }
            catch (ServicioException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
