using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Context;
using WebAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Domain.DTO;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtServices _jwtService;

        public AuthenticationController(ApplicationDbContext context, IJwtServices jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] InicioRequest request)
        {
            var user = await _context.Usuarios.Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.UserName == request.UserName && x.Password == request.Password);

            if (user == null)
                return Unauthorized("Credenciales inválidas");

            var token = _jwtService.GenerateToken(user.UserName, user.Roles?.Nombre ?? "Usuario");

            return Ok(new { token });
        }
    }
}
