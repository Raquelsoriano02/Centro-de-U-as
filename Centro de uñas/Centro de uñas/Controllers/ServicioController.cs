using CentroUnas.Application.Dtos.Servicio;
using CentroUnas.Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace Centro_de_uñas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicioController : ControllerBase
    {
        private readonly ServicioService _service;

        public ServicioController(ServicioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServicios()
        {
            var result = await _service.GetAllServiciosAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServicioById(int id)
        {
            var result = await _service.GetServicioByIdAsync(id);
            if (result.NotFound)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateServicio([FromBody] CreateServicioDto dto)
        {
            var result = await _service.CreateServicioAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetServicioById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServicio(int id, [FromBody] UpdateServicioDto dto)
        {
            var result = await _service.UpdateServicioAsync(id, dto);
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
        public async Task<IActionResult> DeleteServicio(int id)
        {
            var result = await _service.DeleteServicioAsync(id);
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
