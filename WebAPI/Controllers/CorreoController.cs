using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CorreoController : Controller
    {
        private readonly CorreoServices _correoService;

        public CorreoController(CorreoServices correoService)
        {
            _correoService = correoService;
        }

        [HttpPost("enviar")]
        public async Task<IActionResult> EnviarEmail(CorreoRequest request)
        {
            var asunto = "Gracias por registrarte";
            var mensaje = "<h1>Bienvenido</h1><p>Te damos la bienvenida a nuestro sistema. Estamos a tus órdenes.</p>";

            await _correoService.EnviarEmailAsync(
                request.Destinatario,
                asunto,
                mensaje
            );

            return Ok(new { message = "Correo enviado correctamente." });
        }

    }
}
