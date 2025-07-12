using Microsoft.AspNetCore.Mvc;
using WebAPI.Services.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MonedaController : Controller
    {
        private readonly BanxicoService _banxicoService;

        public MonedaController(BanxicoService banxicoService)
        {
            _banxicoService = banxicoService;
        }

        [HttpGet("tasa/{serieId}")]
        public async Task<IActionResult> GetRate(string serieId)
        {
            var rate = await _banxicoService.GetExchangeRate(serieId);
            return Ok(new { rate });
        }
    }
}
