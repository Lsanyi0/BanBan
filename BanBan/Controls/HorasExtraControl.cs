using BanBan.Model;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace BanBan.Controls
{
    public class HorasExtraControl : HorasExtraCommonControl
    {
        private const string heFN = "he.xml";
        private const string heFNo = "he.no";
        private const string htFN = "ht.xml";
        private const string htFNo = "ht.no";
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
        //almacena los datos de horas extra de empleado en xml
        public void GuardarDatos(BindingList<HorasExtraModel> he)
        {
            XmlSerializer xml = new XmlSerializer(typeof(BindingList<HorasExtraModel>));
            using (StreamWriter sw = new StreamWriter(heFNo))
            {
                using (XmlWriter writer = XmlWriter.Create(sw, new XmlWriterSettings { Indent = false }))
                {
                    xml.Serialize(writer, he);
                }
            }
            Crypto.Encrypt(heFNo, heFN);
        }
        //almacena los datos de horario de empleado en xml
        public void GuardarDatos(List<DatosDispositivoModel> HorasEmpleados)
        {
            XmlSerializer xml = new XmlSerializer(typeof(List<DatosDispositivoModel>));
            using (StreamWriter sw = new StreamWriter(htFNo))
            {
                using (XmlWriter writer = XmlWriter.Create(sw, new XmlWriterSettings { Indent = false }))
                {
                    xml.Serialize(writer, HorasEmpleados);
                }
            }
            Crypto.Encrypt(htFNo, htFN);
        }
        public BindingList<HorasExtraModel> CargarDatos()
        {
            Crypto.Decrypt(heFN, heFNo);
            HorasExtraModel.Load = true;
            XmlSerializer xml = new XmlSerializer(typeof(BindingList<HorasExtraModel>));
            BindingList<HorasExtraModel> he;
            try
            {
                using (FileStream fileStream = new FileStream(heFNo, FileMode.Open))
                {
                    he = (BindingList<HorasExtraModel>)xml.Deserialize(fileStream);
                }
            }
            catch (System.Exception)
            {
                he = new BindingList<HorasExtraModel>();
            }

            File.Delete(heFNo);
            HorasExtraModel.Load = false;
            return he;
        }
    }
}
