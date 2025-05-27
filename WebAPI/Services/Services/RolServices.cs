using Domain.DTO;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Context;
using WebAPI.Services.IServices;

namespace WebAPI.Services.Services
{
    public class RolServices : IRolServices //Implemento los metodos de la interfaz
    {

        //Esta parte nos permite realizar las acciones a la bd
        private readonly ApplicationDbContext _context;

        public RolServices(ApplicationDbContext context)
        {
            _context = context;
        }

        //Devuelve la lista de los roles
        public async Task<Response<List<Rol>>> GetAll()
        {
            try
            {
                var roles = await _context.Roles.ToListAsync();

                return new Response<List<Rol>>(roles, "Lista de roles");
            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error " + ex.Message);
            }
        }

        //Busca un rol por el id
        public async Task<Response<Rol>> ById(int id)
        {
            try
            {
                var rol = await _context.Roles.FindAsync(id);

                if(rol == null)
                {
                    return new Response<Rol>(rol, "Rol no encontrado");
                }

                return new Response<Rol>(rol, "Rol encontrado");
            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error " + ex.Message);
            }
        }

        //Crea un nuevo rol a partir de los datos que se piden en el Rolquest
        public async Task<Response<Rol>> Crear(RolRequest request)
        {
            try
            {
                var rol = new Rol()
                {
                    Nombre = request.Nombre,
                };

                _context.Roles.Add(rol);
                await _context.SaveChangesAsync();

                return new Response<Rol>(rol, "Rol creado");
            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error " + ex.Message);
            }
        }

        //Actualiza la informacion del usuario a partir del id porporcionado
        public async Task<Response<string>> Update(int id, RolRequest request)
        {
            //Buscamos en la tabla de datos el id
            var rol = await _context.Roles.FindAsync(id);

            //Aplica en caso de que no exista el rol que se busca
            if( rol == null)
            {
                return new Response<string>("Rol no encontrado");
            }

            //mandamos el dato nuevo y guardamo en la bd
            rol.Nombre = request.Nombre;
            await _context.SaveChangesAsync();

            //Es el mensaje que enviara de retorno
            return new Response<string>("Rol actualizado exitosamente");
        }

        //Busca por el id y elimina el rol 
        public async Task<Response<string>> Delete(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if(rol == null)
            {
                return new Response<string>("Rol no encontrado"); 
            }

            //Remueve de la bd y guarda los datos
            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();

            return new Response<string>("Rol eliminado");
        }
    }
}
