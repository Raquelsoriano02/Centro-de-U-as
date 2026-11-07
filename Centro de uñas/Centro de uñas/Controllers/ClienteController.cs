using CentroUnas.Domain.Entities;
using CentroUnas.Infrastructure.Exceptions;
using CentroUnas.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Centro_de_uñas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteRepository _repository;

        public ClienteController(ClienteRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClientes()
        {
            var clientes = await _repository.GetAll();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClienteById(int id)
        {
            var cliente = await _repository.GetById(id);
            if (cliente == null)
            {
                return NotFound($"Cliente con id {id} no encontrado");
            }

            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCliente([FromBody] Cliente datos)
        {
            try
            {
                var cliente = await _repository.CreateClienteAsync(datos);
                return CreatedAtAction(nameof(GetClienteById), new { id = cliente.Id }, cliente);
            }
            catch (ClienteException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] Cliente datos)
        {
            try
            {
                var actualizado = await _repository.UpdateClienteAsync(id, datos);
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
