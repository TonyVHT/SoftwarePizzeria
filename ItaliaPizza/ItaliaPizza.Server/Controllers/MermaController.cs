using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Dto;
using ItaliaPizza.Server.DTOs;
using ItaliaPizza.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ItaliaPizza.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MermaController : ControllerBase
    {
        private readonly IMermaService _mermaService;

        public MermaController(IMermaService mermaService)
        {
            _mermaService = mermaService;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarMerma([FromBody] Dto.MermaDto mermaDto)
        {
            var (success, message) = await _mermaService.RegistrarMermaAsync(mermaDto);

            if (!success)
                return BadRequest(new { message });

            return Ok(new { message = "Merma registrada correctamente." });
        }

        [HttpGet("reporte")]
        public async Task<ActionResult<List<DTOs.MermaDto>>> ObtenerReporteMermas()
        {
            var reporte = await _mermaService.ObtenerReporteMermasAsync();
            return Ok(reporte);
        }

    }
}
