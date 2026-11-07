using CentroUnas.Domain.Entities;
using CentroUnas.Infrastructure.Exceptions;
using CentroUnas.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Centro_de_uñas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagoController : ControllerBase
    {
        private readonly PagoRepository _repository;

        public PagoController(PagoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPagos()
        {
            var pagos = await _repository.GetAll();
            return Ok(pagos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPagoById(int id)
        {
            var pago = await _repository.GetById(id);
            if (pago == null)
            {
                return NotFound($"Pago con id {id} no encontrado");
            }

            return Ok(pago);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePago([FromBody] Pago datos)
        {
            try
            {
                var pago = await _repository.CreatePagoAsync(datos);
                return CreatedAtAction(nameof(GetPagoById), new { id = pago.Id }, pago);
            }
            catch (PagoException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePago(int id, [FromBody] Pago datos)
        {
            try
            {
                var actualizado = await _repository.UpdatePagoAsync(id, datos);
                if (!actualizado)
                {
                    return NotFound($"Pago con id {id} no encontrado");
                }

                return NoContent();
            }
            catch (PagoException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePago(int id)
        {
            var eliminado = await _repository.DeletePagoAsync(id);
            if (!eliminado)
            {
                return NotFound($"Pago con id {id} no encontrado");
            }

            return NoContent();
        }
    }
}
