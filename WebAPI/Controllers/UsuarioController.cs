using Domain.DTO;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services.IServices;

namespace WebAPI.Controllers
{
    //Ruteamos para que aparezcan las opciones al cargar la pagina
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioServices _usuarioServices;


        //Inyecto el servicio de las interfaces
        public UsuarioController(IUsuarioServices usuarioServices)
        {
            _usuarioServices = usuarioServices;
        }

        [HttpGet] //Hace una consulta a la base de datos de los usuarios
        public async Task<IActionResult> GetUsers()
        {
            var response = await _usuarioServices.GetAll();

            return Ok(response);
        }

        [HttpGet("{id}")] //Busco al usuario por el id
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _usuarioServices.ById(id));
        }

        [HttpPost] //Hace el envio a la base de datos de la informacion del usuario
        public async Task<IActionResult> Crear(UsuarioRequest request)
        {
            var response = await _usuarioServices.Crear(request);
            return Ok(response);
        }

        [HttpPut("{id}")] //Le enviamos el id para actualizar especificamente el que tenga ese identificador
        public async Task<IActionResult> Update(int id, [FromBody] UsuarioRequest request)//El objeto (info)
        {
            var response = await _usuarioServices.Update(id, request);
           
            return Ok(response);
        }

        [HttpDelete("{id}")]//Borra el usuario con el id que se manda
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _usuarioServices.Delete(id);

            return Ok(response);
        }

    }
}
