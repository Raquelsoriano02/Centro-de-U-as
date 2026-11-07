using CentroUnas.Infrastructure.Exceptions;
using CentroUnas.Infrastructure.Interfaces;
using CentroUnas.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace Centro_de_uñas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TurnoController : ControllerBase
    {
        private readonly ITurnoRepository _repository;

        public TurnoController(ITurnoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTurnos()
        {
            var turnos = await _repository.GetAllTurnosAsync();
            return Ok(turnos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTurnoById(int id)
        {
            var turno = await _repository.GetTurnoByIdAsync(id);
            if (turno == null)
            {
                return NotFound($"Turno con id {id} no encontrado");
            }

            return Ok(turno);
        }

        [HttpGet("disponibilidad")]
        public async Task<IActionResult> GetDisponibilidad([FromQuery] int tecnicaId, [FromQuery] int servicioId, [FromQuery] DateOnly fecha)
        {
            try
            {
                var disponibles = await _repository.GetDisponibilidadAsync(tecnicaId, servicioId, fecha);
                return Ok(disponibles);
            }
            catch (TurnoException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTurno([FromBody] TurnoModel model)
        {
            try
            {
                var turno = await _repository.CreateTurnoAsync(model);
                return CreatedAtAction(nameof(GetTurnoById), new { id = turno.TurnoId }, turno);
            }
            catch (TurnoException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTurno(int id, [FromBody] TurnoModel model)
        {
            try
            {
                var actualizado = await _repository.UpdateTurnoAsync(id, model);
                if (!actualizado)
                {
                    return NotFound($"Turno con id {id} no encontrado");
                }

                return NoContent();
            }
            catch (TurnoException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> UpdateEstado(int id, [FromBody] EstadoTurnoModel model)
        {
            try
            {
                var actualizado = await _repository.UpdateEstadoAsync(id, model.Estado!.Value);
                if (!actualizado)
                {
                    return NotFound($"Turno con id {id} no encontrado");
                }

                return NoContent();
            }
            catch (TurnoException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTurno(int id)
        {
            try
            {
                var eliminado = await _repository.DeleteTurnoAsync(id);
                if (!eliminado)
                {
                    return NotFound($"Turno con id {id} no encontrado");
                }

                return NoContent();
            }
            catch (TurnoException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
