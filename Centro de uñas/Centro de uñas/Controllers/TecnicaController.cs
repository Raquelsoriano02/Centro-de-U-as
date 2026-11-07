using CentroUnas.Domain.Entities;
using CentroUnas.Infrastructure.Exceptions;
using CentroUnas.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Centro_de_uñas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TecnicaController : ControllerBase
    {
        private readonly TecnicaRepository _repository;

        public TecnicaController(TecnicaRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTecnicas()
        {
            var tecnicas = await _repository.GetAll();
            return Ok(tecnicas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTecnicaById(int id)
        {
            var tecnica = await _repository.GetById(id);
            if (tecnica == null)
            {
                return NotFound($"Técnica con id {id} no encontrada");
            }

            return Ok(tecnica);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTecnica([FromBody] Tecnica datos)
        {
            try
            {
                var tecnica = await _repository.CreateTecnicaAsync(datos);
                return CreatedAtAction(nameof(GetTecnicaById), new { id = tecnica.Id }, tecnica);
            }
            catch (TecnicaException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTecnica(int id, [FromBody] Tecnica datos)
        {
            try
            {
                var actualizado = await _repository.UpdateTecnicaAsync(id, datos);
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
