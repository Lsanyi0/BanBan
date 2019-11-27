using BanBan.Model;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace BanBan.Controls
{
    public class HorasExtraOfflineControl : HorasExtraCommonControl
    {
        private HorasExtraOfflineModel heom;

        private const string filename = "heom.xml";
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

            XmlSerializer xml = new XmlSerializer(typeof(HorasExtraOfflineModel));
            using (StreamWriter sw = new StreamWriter(filename))
            {
                using (XmlWriter writer = XmlWriter.Create(sw, new XmlWriterSettings { Indent = true }))
                {
                    xml.Serialize(writer, heom);
                }
            }
        }

        public HorasExtraOfflineModel CargarBDOffline()
        {
            XmlSerializer xml = new XmlSerializer(typeof(HorasExtraOfflineModel));
            HorasExtraOfflineModel HoraExtraOffline;
            using (FileStream fileStream = new FileStream(filename, FileMode.Open))
            {
                HoraExtraOffline = (HorasExtraOfflineModel)xml.Deserialize(fileStream);
            }
            heom = HoraExtraOffline;
            return heom;
        }
        public bool OfflineLogin(string usuario, string clave)
        {
            CargarBDOffline();
            return (from us in heom.Usuarios where us.contrasena.Equals(clave) && us.usuario.Equals(usuario) select us).Any();
        }
    }
}