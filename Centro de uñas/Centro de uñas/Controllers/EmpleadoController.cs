using CentroUnas.Application.Dtos.Empleado;
using CentroUnas.Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace Centro_de_uñas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadoController : ControllerBase
    {
        private readonly EmpleadoService _service;

        public EmpleadoController(EmpleadoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmpleados()
        {
            var result = await _service.GetAllEmpleadosAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmpleadoById(int id)
        {
            var result = await _service.GetEmpleadoByIdAsync(id);
            if (result.NotFound)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmpleado([FromBody] CreateEmpleadoDto dto)
        {
            var result = await _service.CreateEmpleadoAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetEmpleadoById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmpleado(int id, [FromBody] UpdateEmpleadoDto dto)
        {
            var result = await _service.UpdateEmpleadoAsync(id, dto);
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
        public async Task<IActionResult> DeleteEmpleado(int id)
        {
            var result = await _service.DeleteEmpleadoAsync(id);
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
