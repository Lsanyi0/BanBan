using System.Threading.Tasks;

namespace BanBan.Controls
{
    class LoginControl
    {
        /// <summary>
        /// Lógica de negocios para la página de login
        /// </summary>
        public static bool UsuarioValido { get; set; }
        public LoginControl()
        {
            UsuarioValido = false;
        }
        public bool isUsuarioValido()
        {
            return UsuarioValido;
        }

        public async Task<bool> verificarUsuario()
        {
            while (!isUsuarioValido())
            {
                await Task.Delay(25);
            }
            return true;
        }
    }
}
