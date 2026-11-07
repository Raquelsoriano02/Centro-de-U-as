using CentroUnas.Infrastructure.Exceptions;
using CentroUnas.Infrastructure.Interfaces;
using CentroUnas.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace Centro_de_uñas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagoController : ControllerBase
    {
        private readonly IPagoRepository _repository;

        public PagoController(IPagoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPagos()
        {
            var pagos = await _repository.GetAllPagosAsync();
            return Ok(pagos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPagoById(int id)
        {
            var pago = await _repository.GetPagoByIdAsync(id);
            if (pago == null)
            {
                return NotFound($"Pago con id {id} no encontrado");
            }

            return Ok(pago);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePago([FromBody] PagoModel model)
        {
            try
            {
                var pago = await _repository.CreatePagoAsync(model);
                return CreatedAtAction(nameof(GetPagoById), new { id = pago.PagoId }, pago);
            }
            catch (PagoException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePago(int id, [FromBody] PagoModel model)
        {
            try
            {
                var actualizado = await _repository.UpdatePagoAsync(id, model);
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
