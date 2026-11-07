using CentroUnas.Domain.Core;
using CentroUnas.Domain.Entities;
using CentroUnas.Infrastructure.Exceptions;
using CentroUnas.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Centro_de_uñas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TurnoController : ControllerBase
    {
        private readonly TurnoRepository _repository;

        public TurnoController(TurnoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTurnos()
        {
            var turnos = await _repository.GetAll();
            return Ok(turnos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTurnoById(int id)
        {
            var turno = await _repository.GetById(id);
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
        public async Task<IActionResult> CreateTurno([FromBody] Turno datos)
        {
            try
            {
                var turno = await _repository.CreateTurnoAsync(datos);
                return CreatedAtAction(nameof(GetTurnoById), new { id = turno.Id }, turno);
            }
            catch (TurnoException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTurno(int id, [FromBody] Turno datos)
        {
            try
            {
                var actualizado = await _repository.UpdateTurnoAsync(id, datos);
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
        public async Task<IActionResult> UpdateEstado(int id, [FromBody] EstadoTurno estado)
        {
            try
            {
                var actualizado = await _repository.UpdateEstadoAsync(id, estado);
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
