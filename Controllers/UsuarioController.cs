using LionDev.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Security.AccessControl;

namespace LionDev.Controllers
{
    //indicate that the class is an API controller.It enables some behaviors like automatic model validation and response formatting.
    [ApiController]
    //for example, the login action will be accessible at the endpoint /usuario/login.
    [Route("usuario")]
    public class UsuarioController : ControllerBase
    {
        // IConfiguration parameter, which is used to access configuration settings. In this case, it's used to retrieve JWT-related configuration.
        public IConfiguration _configuration;

        public UsuarioController(IConfiguration configuration) 
        { 
            _configuration = configuration; 
        }

        [HttpPost]
        [Route("login")]
        public dynamic login([FromBody] Object optData)
        {
            var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());
            string user = data.usuario.ToString();
            string password = data.password.ToString();


            try
            {
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

                // to retrieve a specific section of the configuration. In this case, it's getting the section named "Jwt."
                //.Get<Jwt>(): The Get<T> method is used to deserialize the configuration section into an instance of a specified type (Jwt in this case). The <Jwt> part indicates the type to which the configuration data should be bound.
                var jwt = _configuration.GetSection("Jwt")
                .Get<Jwt>();

                //claims, which are pieces of information associated with the subject of a JSON Web Token (JWT). 
                var claims = new[]
                {
                    // typically the unique identifier of the user or entity
                    new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                    // This represents the standard JWT ID claim. A new GUID (Globally Unique Identifier) is generated
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    //represents the standard issued at claim in a JWT. The current UTC timestamp
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("id", usuario.idUsuario),
                    new Claim("usuario", usuario.usuario)
                };

                //creating a symmetric security key (SymmetricSecurityKey) based on the secret key (jwt.Key) obtained from the JWT configuration.
                //jwt.Key: The secret key used to sign the JWT
                //Encoding.UTF8.GetBytes(jwt.Key): Converts the string key into a byte array 
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                //algorithm used for signing the JWT
                var singIng = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                        jwt.Issuer,
                        jwt.Audience,
                        claims,
                        expires: DateTime.Now.AddMinutes(60),
                        signingCredentials: singIng
                );

                return new
                {
                    success = true,
                    message = "exito",
                    result = new JwtSecurityTokenHandler().WriteToken(token)
                };
            }
            catch (Exception ex) 
            { 
                return ex.Message;
            }
        }

      
    }
}
