namespace LionDev.Models
{
    public class Usuario
    {
        public required string idUsuario { get; set; }
        public required string usuario { get; set; }
        public required string password { get; set; }
        public required string rol { get; set; }

        public static List<Usuario> DB()
        { 
            var list = new List<Usuario>();
            {
                new Usuario
                {
                    idUsuario = "1",
                    usuario = "Mateo",
                    password = "123.",
                    rol = "empleado"
                };
                new Usuario
                {
                    idUsuario = "2",
                    usuario = "Marcos",
                    password = "123.",
                    rol = "administrador"
                };
                new Usuario
                {
                    idUsuario = "3",
                    usuario = "Lucas",
                    password = "123.",
                    rol = "asesor"
                };            
            };
            return list;
        }
    }
}
