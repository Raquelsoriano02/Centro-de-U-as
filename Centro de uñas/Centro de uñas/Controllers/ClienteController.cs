using CentroUnas.Infrastructure.Exceptions;
using CentroUnas.Infrastructure.Interfaces;
using CentroUnas.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace Centro_de_uñas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteRepository _repository;

        public ClienteController(IClienteRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClientes()
        {
            var clientes = await _repository.GetAllClientesAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClienteById(int id)
        {
            var cliente = await _repository.GetClienteByIdAsync(id);
            if (cliente == null)
            {
                return NotFound($"Cliente con id {id} no encontrado");
            }

            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCliente([FromBody] ClienteModel model)
        {
            try
            {
                var cliente = await _repository.CreateClienteAsync(model);
                return CreatedAtAction(nameof(GetClienteById), new { id = cliente.ClienteId }, cliente);
            }
            catch (ClienteException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] ClienteModel model)
        {
            try
            {
                var actualizado = await _repository.UpdateClienteAsync(id, model);
                if (!actualizado)
                {
                    return NotFound($"Cliente con id {id} no encontrado");
                }

                return NoContent();
            }
            catch (ClienteException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            try
            {
                var eliminado = await _repository.DeleteClienteAsync(id);
                if (!eliminado)
                {
                    return NotFound($"Cliente con id {id} no encontrado");
                }

                return NoContent();
            }
            catch (ClienteException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
