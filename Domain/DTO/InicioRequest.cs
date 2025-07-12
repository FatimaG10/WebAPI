using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class InicioRequest
    {
        //Solo le pido estos datos para el inicio de sesion
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
