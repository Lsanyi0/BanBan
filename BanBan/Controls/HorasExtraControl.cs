using BanBan.Model;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace BanBan.Controls
{
    public class HorasExtraControl : HorasExtraCommonControl
    {
        private const string filenamex = "he.xml";
        private const string filename = "he.no";
        public HorasExtraControl(HorasExtraOfflineModel heom)
        {
            sc = heom;
        }
        public HorasExtraControl()
        {
            sc = new HorasExtraOfflineModel();
            sc.Empleados = GetEmpleados();
            sc.Sucursales = GetSucursales();
            sc.TipoHoras = GetTipoHoras();
            sc.Usuarios = GetUsuarios();
            sc.Trabajos = GetTrabajos();
            sc.Dispositivos = GetDispositivos();
        }
        public BindingList<string> GetCBEmplados()
        {
            BindingList<string> CBEmplados = new BindingList<string>();
            foreach (var em in sc.Empleados)
            {
                CBEmplados.Add(em.nombre + " " + em.apellido);
            }
            return CBEmplados;
        }
        public void GuardarDatos(BindingList<HorasExtraModel> he)
        {
            XmlSerializer xml = new XmlSerializer(typeof(BindingList<HorasExtraModel>));
            using (StreamWriter sw = new StreamWriter(filename))
            {
                using (XmlWriter writer = XmlWriter.Create(sw, new XmlWriterSettings { Indent = false }))
                {
                    xml.Serialize(writer, he);
                }
            }
            Crypto.Encrypt(filename, filenamex);
        }
        public BindingList<HorasExtraModel> CargarDatos() 
        {
            Crypto.Decrypt(filenamex, filename);
            HorasExtraModel.Load = true;
            XmlSerializer xml = new XmlSerializer(typeof(BindingList<HorasExtraModel>));
            BindingList<HorasExtraModel> he;
            try
            {
                using (FileStream fileStream = new FileStream(filename, FileMode.Open))
                {
                    he = (BindingList<HorasExtraModel>)xml.Deserialize(fileStream);
                }
            }
            catch (System.Exception)
            {
                he = new BindingList<HorasExtraModel>();
            }

            File.Delete(filename);
            HorasExtraModel.Load = false;
            return he;
        }
    }
}
