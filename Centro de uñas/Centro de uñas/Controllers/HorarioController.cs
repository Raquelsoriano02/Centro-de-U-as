using CentroUnas.Infrastructure.Exceptions;
using CentroUnas.Infrastructure.Interfaces;
using CentroUnas.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace Centro_de_uñas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HorarioController : ControllerBase
    {
        private readonly IHorarioRepository _repository;

        public HorarioController(IHorarioRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHorarios()
        {
            var horarios = await _repository.GetAllHorariosAsync();
            return Ok(horarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHorarioById(int id)
        {
            var horario = await _repository.GetHorarioByIdAsync(id);
            if (horario == null)
            {
                return NotFound($"Horario con id {id} no encontrado");
            }

            return Ok(horario);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHorario([FromBody] HorarioModel model)
        {
            try
            {
                var horario = await _repository.CreateHorarioAsync(model);
                return CreatedAtAction(nameof(GetHorarioById), new { id = horario.HorarioId }, horario);
            }
            catch (HorarioException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHorario(int id, [FromBody] HorarioModel model)
        {
            try
            {
                var actualizado = await _repository.UpdateHorarioAsync(id, model);
                if (!actualizado)
                {
                    return NotFound($"Horario con id {id} no encontrado");
                }

                return NoContent();
            }
            catch (HorarioException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHorario(int id)
        {
            var eliminado = await _repository.DeleteHorarioAsync(id);
            if (!eliminado)
            {
                return NotFound($"Horario con id {id} no encontrado");
            }

            return NoContent();
        }
    }
}
