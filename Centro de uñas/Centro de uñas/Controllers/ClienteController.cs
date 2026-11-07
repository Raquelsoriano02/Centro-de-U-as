using CentroUnas.Application.Dtos.Cliente;
using CentroUnas.Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace Centro_de_uñas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _service;

        public ClienteController(ClienteService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClientes()
        {
            var result = await _service.GetAllClientesAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClienteById(int id)
        {
            var result = await _service.GetClienteByIdAsync(id);
            if (result.NotFound)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCliente([FromBody] CreateClienteDto dto)
        {
            var result = await _service.CreateClienteAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetClienteById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] UpdateClienteDto dto)
        {
            var result = await _service.UpdateClienteAsync(id, dto);
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
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var result = await _service.DeleteClienteAsync(id);
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
