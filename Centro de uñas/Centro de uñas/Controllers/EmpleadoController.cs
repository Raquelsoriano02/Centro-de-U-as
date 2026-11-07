using CentroUnas.Domain.Entities;
using CentroUnas.Infrastructure.Exceptions;
using CentroUnas.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Centro_de_uñas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadoController : ControllerBase
    {
        private readonly EmpleadoRepository _repository;

        public EmpleadoController(EmpleadoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmpleados()
        {
            var empleados = await _repository.GetAll();
            return Ok(empleados);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmpleadoById(int id)
        {
            var empleado = await _repository.GetById(id);
            if (empleado == null)
            {
                return NotFound($"Empleado con id {id} no encontrado");
            }

            return Ok(empleado);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmpleado([FromBody] Empleado datos)
        {
            try
            {
                var empleado = await _repository.CreateEmpleadoAsync(datos);
                return CreatedAtAction(nameof(GetEmpleadoById), new { id = empleado.Id }, empleado);
            }
            catch (EmpleadoException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmpleado(int id, [FromBody] Empleado datos)
        {
            try
            {
                var actualizado = await _repository.UpdateEmpleadoAsync(id, datos);
                if (!actualizado)
                {
                    return NotFound($"Empleado con id {id} no encontrado");
                }

                return NoContent();
            }
            catch (EmpleadoException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpleado(int id)
        {
            try
            {
                var eliminado = await _repository.DeleteEmpleadoAsync(id);
                if (!eliminado)
                {
                    return NotFound($"Empleado con id {id} no encontrado");
                }

                return NoContent();
            }
            catch (EmpleadoException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
