using Domain.DTO;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Services.IServices
{
    public interface IRolServices
    {

        //Aqui se encuentran las interfaces de las acciones
        public Task<Response<List<Rol>>> GetAll();

        public Task<Response<Rol>> ById(int id);

        public Task<Response<Rol>> Crear(RolRequest request);

        public Task<Response<string>> Update(int id, RolRequest request);

        public Task<Response<string>> Delete(int id);
    }
}
