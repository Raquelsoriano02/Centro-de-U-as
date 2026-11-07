using CentroUnas.Application.Dtos.Tecnica;
using CentroUnas.Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace Centro_de_uñas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TecnicaController : ControllerBase
    {
        private readonly TecnicaService _service;

        public TecnicaController(TecnicaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTecnicas()
        {
            var result = await _service.GetAllTecnicasAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTecnicaById(int id)
        {
            var result = await _service.GetTecnicaByIdAsync(id);
            if (result.NotFound)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTecnica([FromBody] CreateTecnicaDto dto)
        {
            var result = await _service.CreateTecnicaAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetTecnicaById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTecnica(int id, [FromBody] UpdateTecnicaDto dto)
        {
            var result = await _service.UpdateTecnicaAsync(id, dto);
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
        public async Task<IActionResult> DeleteTecnica(int id)
        {
            var result = await _service.DeleteTecnicaAsync(id);
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
