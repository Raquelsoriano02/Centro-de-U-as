using CentroUnas.Infrastructure.Exceptions;
using CentroUnas.Infrastructure.Interfaces;
using CentroUnas.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace Centro_de_uñas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TecnicaController : ControllerBase
    {
        private readonly ITecnicaRepository _repository;

        public TecnicaController(ITecnicaRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTecnicas()
        {
            var tecnicas = await _repository.GetAllTecnicasAsync();
            return Ok(tecnicas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTecnicaById(int id)
        {
            var tecnica = await _repository.GetTecnicaByIdAsync(id);
            if (tecnica == null)
            {
                return NotFound($"Técnica con id {id} no encontrada");
            }

            return Ok(tecnica);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTecnica([FromBody] TecnicaModel model)
        {
            try
            {
                var tecnica = await _repository.CreateTecnicaAsync(model);
                return CreatedAtAction(nameof(GetTecnicaById), new { id = tecnica.TecnicaId }, tecnica);
            }
            catch (TecnicaException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTecnica(int id, [FromBody] TecnicaModel model)
        {
            try
            {
                var actualizado = await _repository.UpdateTecnicaAsync(id, model);
                if (!actualizado)
                {
                    return NotFound($"Técnica con id {id} no encontrada");
                }

                return NoContent();
            }
            catch (TecnicaException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTecnica(int id)
        {
            try
            {
                var eliminado = await _repository.DeleteTecnicaAsync(id);
                if (!eliminado)
                {
                    return NotFound($"Técnica con id {id} no encontrada");
                }

                return NoContent();
            }
            catch (TecnicaException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
