using Domain.DTO;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Context;
using WebAPI.Services.IServices;

namespace WebAPI.Services.Services
{
    public class UsuarioServices : IUsuarioServices
    {
        //Esta seccion nos permite realizar las acciones a la bd
        private readonly ApplicationDbContext _context;

        public UsuarioServices(ApplicationDbContext context)
        {
            _context = context;
        }

        //Lista de usuarios
        public async Task<Response<List<Usuario>>> GetAll() //Trae la lista de usuarios
        {
            try
            {

                List<Usuario> response = await _context.Usuarios.Include(x=> x.Roles).ToListAsync();

                return new Response<List<Usuario>>(response, "Lista de usuarios");

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error " + ex.Message);
            }
        }

        //Busco al usuario por el id
        public async Task<Response<Usuario>> ById(int id) //Busca en la base de datos por el id
        {
            try
            {
                Usuario usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.PkUsuario == id);

                return new Response<Usuario>(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error" + ex.Message);
            }
        }

        //Creo un usuario a partir de los datos que pide el Usuariorequest
        public async Task<Response<Usuario>> Crear(UsuarioRequest request) //Agrega a la bd un nuevo usuario
        {
            try
            {
                //Datos que le pido al usuario 
                Usuario usuario1 = new Usuario()
                {
                    Nombre = request.Nombre,
                    Password = request.Password,
                    UserName = request.UserName,
                    FkRol = request.FkRol,

                };

                //Guardo la informacion en la bd
                _context.Usuarios.Add(usuario1);
                await _context.SaveChangesAsync();

                return new Response<Usuario>(usuario1);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error" + ex.Message);
            }
        }

        //Permite la actualizacion del usuario, busca por el id
        public async Task<Response<string>> Update(int id, UsuarioRequest request) //Actualiza al usuario
        {
            try
            {
                //busco al usuario en la tabla Usuarios
                var usuario = await _context.Usuarios.FindAsync(id);
                if(usuario == null)
                {
                    return new Response<string>(null, "Usuario no encontrado");
                }

                //Son los datos que se modifican 
                usuario.Nombre = request.Nombre;
                usuario.UserName = request.UserName;
                usuario.Password = request.Password;
                usuario.FkRol = request.FkRol;

                //guardo en la bd
                await _context.SaveChangesAsync();

                //mensaje de retorno
                return new Response<string>(null, "Usuario actualizado correctamente");
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error" + ex.Message);
            }
        }

        //Permite eliminar un usuario, buscamos por el id
        public async Task<Response<string>> Delete(int id)
        {
            try
            {
                //se busca el id en la tabla Usuarios
                var usuario = await _context.Usuarios.FindAsync(id);

                if(usuario == null)
                {
                    return new Response<string>("Usuario no encontrado");
                }

                //Guardamos el cambio en la bd y retorno el mensaje
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();

                return new Response<string>(null,"Usuario elimindado correctamente");
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error" + ex.Message);
            }
        }
    }
}
