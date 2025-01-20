using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProgressusWebApi.Models.AlimentosModels;
using ProgressusWebApi.Services.AlimentoServices;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlimentoCalculoController : ControllerBase
    {
        private readonly AlimentoCalculoService _service;

        public AlimentoCalculoController(AlimentoCalculoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var alimentos = await _service.GetAllAsync();
            return Ok(alimentos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var alimento = await _service.GetByIdAsync(id);
            if (alimento == null) return NotFound("Alimento no encontrado");
            return Ok(alimento);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Alimento alimento)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var newAlimento = await _service.AddAsync(alimento);
            return CreatedAtAction(nameof(GetById), new { id = newAlimento.Id }, newAlimento);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Alimento alimento)
        {
            if (id != alimento.Id) return BadRequest("ID no coincide");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var updatedAlimento = await _service.UpdateAsync(alimento);
                return Ok(updatedAlimento);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound("Alimento no encontrado");
            return NoContent();
        }
    }
}
