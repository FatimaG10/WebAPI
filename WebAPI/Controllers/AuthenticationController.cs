using Domain.DTO;
using Domain.Entities;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebAPI.Context;
using WebAPI.Services.IServices;
using WebAPI.Services.Services;



namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtServices _jwtService;
        private readonly IConfiguration _configuration;
        private readonly IUsuarioServices _usuarioService;


        public AuthenticationController(ApplicationDbContext context, IJwtServices jwtService, IConfiguration configuration, IUsuarioServices usuarioService)
        {
            _context = context;
            _jwtService = jwtService; //para poder hacer la validacion
            _configuration = configuration;
            _usuarioService = usuarioService;

        }

        [HttpPost("login")] //Aparezca el endpoint del inicio de sesion 
        public async Task<IActionResult> Login(InicioRequest request)
        {
            //Valido si el usuario existe
            var user = await _context.Usuarios.Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.UserName == request.UserName && x.Password == request.Password);

            if (user == null)
                return Unauthorized("Credenciales inválidas");

            //Se genera el token con su usuario y rol
            var token = _jwtService.GenerateToken(user.UserName, user.Roles?.Nombre ?? "Usuario");

            //Devuelvo el token
            return Ok(new { token });
        }

        // --------------------
        // LOGIN CON GOOGLE
        // --------------------
        [HttpPost("GoogleLogin")]
        public async Task<IActionResult> GoogleLogin(GoogleAuthRequest request)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(
                    request.TokenGoogle,
                    new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new[] { _configuration["Google:ClientId"] }
                    });

                if (payload == null)
                    return Unauthorized("Token inválido.");

                var email = payload.Email;
                var name = payload.Name;

                var user = await _usuarioService.CrearUsuarioGoogle(email, name);

                return Ok(new { message = "Login exitoso", user });
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
                Console.WriteLine("INNER: " + ex.InnerException?.Message);
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }


    }
}
