using System.IO;

namespace BanBan.Controls
{
    class Crypto
    {
        private const string Clave = "La Contrasena mas larga que se me pudo ocurrir";
        public static void Encrypt(string pathi, string pathf)
        {
            SharpAESCrypt.SharpAESCrypt.Encrypt(Clave, pathi, pathf);
            File.Delete(pathi);
        }
        public static void Decrypt(string pathi, string pathf)
        {
            SharpAESCrypt.SharpAESCrypt.Decrypt(Clave, pathi, pathf);
        }
    }
}
