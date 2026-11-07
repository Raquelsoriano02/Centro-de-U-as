using CentroUnas.Application.Dtos.Turno;
using CentroUnas.Application.Service;
using CentroUnas.Domain.Core;
using Microsoft.AspNetCore.Mvc;

namespace Centro_de_uñas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TurnoController : ControllerBase
    {
        private readonly TurnoService _service;

        public TurnoController(TurnoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTurnos()
        {
            var result = await _service.GetAllTurnosAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTurnoById(int id)
        {
            var result = await _service.GetTurnoByIdAsync(id);
            if (result.NotFound)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpGet("disponibilidad")]
        public async Task<IActionResult> GetDisponibilidad([FromQuery] int tecnicaId, [FromQuery] int servicioId, [FromQuery] DateOnly fecha)
        {
            var result = await _service.GetDisponibilidadAsync(tecnicaId, servicioId, fecha);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTurno([FromBody] CreateTurnoDto dto)
        {
            var result = await _service.CreateTurnoAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetTurnoById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTurno(int id, [FromBody] UpdateTurnoDto dto)
        {
            var result = await _service.UpdateTurnoAsync(id, dto);
            if (result.NotFound)
            {
                return NotFound(result.Message);
            }

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return NoContent();
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> UpdateEstado(int id, [FromBody] EstadoTurno estado)
        {
            var result = await _service.UpdateEstadoAsync(id, estado);
            if (result.NotFound)
            {
                return NotFound(result.Message);
            }

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTurno(int id)
        {
            var result = await _service.DeleteTurnoAsync(id);
            if (result.NotFound)
            {
                return NotFound(result.Message);
            }

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return NoContent();
        }
    }
}
