using EntidadApi.Data;
using EntidadApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EntidadApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntidadesController : ControllerBase
    {
        private readonly IEntidadRepository _repository;

        public EntidadesController(IEntidadRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> ListarPersonasNaturales()
        {
            var personas = await _repository.ListarPersonasNaturalesAsync();
            return Ok(personas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonaNaturalById(int id)
        {
            var persona = await _repository.GetPersonaNaturalByIdAsync(id);

            if (persona == null)
            {
                return NotFound($"No se encontró la entidad con ID {id}.");
            }

            return Ok(persona);
        }

        [HttpPost]
        public async Task<IActionResult> CrearPersonaNatural([FromBody] PersonaNatural persona)
        {
            if (persona == null)
            {
                return BadRequest("Los datos de la persona no pueden ser nulos.");
            }

            var nuevoId = await _repository.CrearPersonaNaturalAsync(persona);
            persona.EntidadID = nuevoId;
            return StatusCode(201, persona);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPersonaNatural(int id, [FromBody] PersonaNatural persona)
        {
            if (id != persona.EntidadID)
            {
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo.");
            }
            await _repository.ActualizarPersonaNaturalAsync(persona);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarEntidad(int id)
        {
            await _repository.EliminarEntidadAsync(id);

            return NoContent();
        }
    }
}
