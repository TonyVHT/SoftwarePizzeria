using ItaliaPizza.Server.Domain;
using ItaliaPizza.Server.Dto;
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
        public async Task<IActionResult> RegistrarMerma([FromBody] MermaDto mermaDto)
        {
            var (success, message) = await _mermaService.RegistrarMermaAsync(mermaDto);

            if (!success)
                return BadRequest(new { message });

            return Ok(new { message = "Merma registrada correctamente." });
        }
    }
}
