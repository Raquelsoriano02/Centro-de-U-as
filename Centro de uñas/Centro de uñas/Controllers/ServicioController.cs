using CentroUnas.Infrastructure.Exceptions;
using CentroUnas.Infrastructure.Interfaces;
using CentroUnas.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace Centro_de_uñas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicioController : ControllerBase
    {
        private readonly IServicioRepository _repository;

        public ServicioController(IServicioRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServicios()
        {
            var servicios = await _repository.GetAllServiciosAsync();
            return Ok(servicios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServicioById(int id)
        {
            var servicio = await _repository.GetServicioByIdAsync(id);
            if (servicio == null)
            {
                return NotFound($"Servicio con id {id} no encontrado");
            }

            return Ok(servicio);
        }

        [HttpPost]
        public async Task<IActionResult> CreateServicio([FromBody] ServicioModel model)
        {
            var servicio = await _repository.CreateServicioAsync(model);
            return CreatedAtAction(nameof(GetServicioById), new { id = servicio.ServicioId }, servicio);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServicio(int id, [FromBody] ServicioModel model)
        {
            var actualizado = await _repository.UpdateServicioAsync(id, model);
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
