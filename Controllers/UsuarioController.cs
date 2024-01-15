using LionDev.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LionDev.Controllers
{
    public class UsuarioController : ControllerBase
    {
        public IConfiguration _configuration;

        public UsuarioController(IConfiguration configuration) 
        { 
            _configuration = configuration; 
        }

        public dynamic IniciarSesion([FromBody] Object optData)
        {
            var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());
            string user = data.usuario.ToString();
            string password = data.password.ToString();

            Usuario usuario = Usuario.DB()
                .Where(x => x.usuario == user && x.password == password)
                .FirstOrDefault();

            if (usuario == null)
            {
                return new
                {
                    success = false,
                    message = "Credenciales incorrectas",
                    result = ""
                };
            }

            var jwt = _configuration.GetSection("Jwt")
                .Get<Jwt>();

            var claims = 
        }
    }
}
