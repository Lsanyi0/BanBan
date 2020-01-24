using BanBan.Model;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
namespace BanBan.Controls
{
    public class HorasExtraOfflineControl : HorasExtraCommonControl
    {
        private HorasExtraOfflineModel heom;

        private const string filename = "\\heom.no";//hora extra offline model desencriptado
        private const string filenamex = "\\heom.xml";//hora extra offline model encriptado
        private string pathDB = Properties.Settings.Default.Dropbox;
        public HorasExtraOfflineControl()
        {
            heom = new HorasExtraOfflineModel();
        }
        public void CrearBDOffline()
        {
            heom.Empleados = GetEmpleados();
            heom.Sucursales = GetSucursales();
            heom.TipoHoras = GetTipoHoras();
            heom.Usuarios = GetUsuarios();
            heom.Trabajos = GetTrabajos();
            heom.Dispositivos = GetDispositivos();

            XmlSerializer xml = new XmlSerializer(typeof(HorasExtraOfflineModel));
            using (StreamWriter sw = new StreamWriter(pathDB + filename))
            {
                using (XmlWriter writer = XmlWriter.Create(sw, new XmlWriterSettings { Indent = true }))
                {
                    xml.Serialize(writer, heom);
                }
            }
            Crypto.Encrypt(pathDB + filename, pathDB + filenamex);
        }

        public HorasExtraOfflineModel CargarBDOffline()
        {
            Crypto.Decrypt(pathDB + filenamex, pathDB + filename);

            XmlSerializer xml = new XmlSerializer(typeof(HorasExtraOfflineModel));
            HorasExtraOfflineModel HoraExtraOffline;
            try
            {
                using (FileStream fileStream = new FileStream(pathDB + filename, FileMode.Open))
                {
                    HoraExtraOffline = (HorasExtraOfflineModel)xml.Deserialize(fileStream);
                }
            }
            catch (System.Exception)
            {
                HoraExtraOffline = null;
                MessageBox.Show("No se encontro el archivo de base de datos local, " +
                    "porfavor contacte con administrador de sistemas", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            heom = HoraExtraOffline;
            File.Delete(pathDB + filename);
            return heom;
        }
        public bool OfflineLogin(string usuario, string clave)
        {
            CargarDB();
            return heom == null ? false : (from us in heom.Usuarios where us.contrasena.Equals(clave) && us.usuario.Equals(usuario) select us).Any();
        }
        public void CargarDB()
        {
            try
            {
                File.Copy(pathDB + filenamex, System.Environment.CurrentDirectory + filenamex, true);
            }
            catch (System.Exception)
            {
                MessageBox.Show("El archivo de base de datos no existe, porfavor solicite que envien uno.");
            }
            CargarBDOffline();
        }
    }
}