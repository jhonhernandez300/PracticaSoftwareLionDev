using System.Security.Claims;

namespace LionDev.Models
{
    public class Jwt
    {
        public required string Key { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public required string Subject { get; set; }

        public static dynamic validarToken(ClaimsIdentity identity)
        {
            try
            {
                //This condition checks if the ClaimsIdentity has no claims. If this condition is true, it means that the token is empty or doesn't contain any claims. 
                if (identity.Claims.Count() == 0)
                {
                    return new
                    {
                        success = false,
                        message = "Token no válido",
                        result = ""
                    };
                }

                var id = identity.Claims.FirstOrDefault(x => x.Type == "id").Value;
                Usuario usuario = Usuario.DB().FirstOrDefault(x => x.idUsuario == id);

                return new
                {
                    success = true,
                    message = "exito",
                    result = usuario
                };
            }
            catch (Exception ex) 
            {
                return new
                {
                    success = false,
                    message = "Catch: " + ex.Message,
                    result = ""
                };
            }
        }
    }
}
