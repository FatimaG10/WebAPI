using Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services.IServices;

namespace WebAPI.Controllers
{

    //Ruta para que aparezca al cargar la pagina
    [Authorize]
    [ApiController]
    [Route("[controller]")]

    public class RolController : ControllerBase
    {
        private readonly IRolServices _rolServices;

        //Inyecto el servicio de la interfaz
        public RolController(IRolServices rolServices)
        {
            _rolServices = rolServices;
        }

        [HttpGet] //Muestra la lista de roles
        public async Task<IActionResult> GetAll()
        {
            var response = await _rolServices.GetAll();
            return Ok(response);
        }

        [HttpGet("{id}")] //Devuelve un rol especifico
        public async Task<IActionResult> ById(int id)
        {
            return Ok(await _rolServices.ById(id));
        }

        [HttpPost] //Llamo al metodo para crear un nuevo rol
        public async Task<IActionResult> Crear(RolRequest request)
        {
            var response = await _rolServices.Crear(request);
            return Ok(response);
        }

        [HttpPut("{id}")] //Usamos el id para identificar al rol que actualizaremos
        public async Task<IActionResult> Update(int id, [FromBody] RolRequest request)
        {
            var response = await _rolServices.Update(id, request);
            return Ok(response);
        }

        [HttpDelete("{id}")] //Se utliza el id para identificar el rol que se va a eliminar
        public async Task<IActionResult> Delete(int id)
        {
            var respoonse = await _rolServices.Delete(id);
            return Ok(respoonse);
        }
    }
}
