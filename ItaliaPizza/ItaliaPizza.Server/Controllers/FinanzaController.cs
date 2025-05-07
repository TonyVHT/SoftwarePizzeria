using ItaliaPizza.Server.DTOs;
using ItaliaPizza.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ItaliaPizza.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinanzaController : ControllerBase
    {
        private readonly IFinanzaService _finanzaService;
        public FinanzaController(IFinanzaService finanzaService)
        {
            _finanzaService = finanzaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<FinanzaDTO>>> GetAllFinanzas()
        {
            var finanzasDto = await _finanzaService.GetAllFinanzasAsync();
            return Ok(finanzasDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FinanzaDTO>> GetFinanzaById(int id)
        {
            var finanzaDto = await _finanzaService.GetFinanzaByIdAsync(id);
            if (finanzaDto == null)
            {
                return NotFound();
            }

            return Ok(finanzaDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateFinanza([FromBody] FinanzaDTO finanzaDto)
        {
            if (finanzaDto == null)
            {
                return BadRequest();
            }

            await _finanzaService.AddFinanzaAsync(finanzaDto);
            return CreatedAtAction(nameof(GetFinanzaById), new { id = finanzaDto.Id }, finanzaDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFinanza(int id, [FromBody] FinanzaDTO finanzaDto)
        {
            if (id != finanzaDto.Id)
            {
                return BadRequest();
            }

            await _finanzaService.UpdateFinanzaAsync(finanzaDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFinanza(int id)
        {
            await _finanzaService.DeleteFinanzaAsync(id);
            return NoContent();
        }

        [HttpGet("balance/{fecha}")]
        public async Task<ActionResult<decimal>> GetBalanceDiario(DateTime fecha)
        {
            var balance = await _finanzaService.GetBalanceDiarioAsync(fecha);
            return Ok(balance);
        }

        [HttpGet("reporte/{fecha}")]
        public async Task<ActionResult<List<FinanzaDTO>>> GetReporteBalanceDiario(DateTime fecha)
        {
            var reporte = await _finanzaService.GetReporteBalanceDiario(fecha);
            return Ok(reporte);
        }

    }
}
