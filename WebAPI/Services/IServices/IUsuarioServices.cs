using Domain.DTO;
using Domain.Entities;

namespace WebAPI.Services.IServices
{
    public interface IUsuarioServices
    {
        //Aqui se encuentran las interfaces del usuario
        public Task<Response<List<Usuario>>> GetAll();

        public Task<Response<Usuario>> ById(int id);

        public Task<Response<Usuario>> Crear(UsuarioRequest request);

        public Task<Response<string>> Update(int id, UsuarioRequest request);

        public Task<Response<string>> Delete(int id);

        public Task<Usuario> CrearUsuarioGoogle(string email, string nombre);
    }
}
