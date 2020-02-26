using BanBan.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace BanBan.Controls
{
    public class HorasExtraControl : HorasExtraCommonControl
    {
        static readonly string heFN = "ds.xml";
        static readonly string heFNo = "ds.no";
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
        public BindingList<string> GetCBSucursales()
        {
            BindingList<string> CBSucursales = new BindingList<string>();
            foreach (var sc in sc.Sucursales)
            {
                CBSucursales.Add(sc.sucursal1);
            }
            return CBSucursales;
        }
        //almacena los datos de horas extra de empleado en xml
        public void GuardarDatos(DatosSucursalModel dsm)
        {
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(DatosSucursalModel));
                using (StreamWriter sw = new StreamWriter(heFNo))
                {
                    using (XmlWriter writer = XmlWriter.Create(sw, new XmlWriterSettings { Indent = false }))
                    {
                        xml.Serialize(writer, dsm);
                    }
                }
                Crypto.Encrypt(heFNo, heFN);
                System.Windows.MessageBox.Show("Se ha guardado satisfactoriamente", "Guardar", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex, "Error!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
        }
        //almacena los datos de horario y hora extra en un solo xml
        public void GuardarDatos(DatosSucursalModel DatosSucursal, string nombresucursal, DateTime Desde, DateTime Hasta)
        {
            try
            {
                string filename = GetStringArchivo(nombresucursal, Desde, Hasta);
                XmlSerializer xml = new XmlSerializer(typeof(DatosSucursalModel));
                using (StreamWriter sw = new StreamWriter(filename + ".no"))
                {
                    using XmlWriter writer = XmlWriter.Create(sw, new XmlWriterSettings { Indent = true });
                    xml.Serialize(writer, DatosSucursal);
                }
                Crypto.Encrypt(filename + ".no", HorasExtraOfflineControl.pathDB + "\\" + filename + ".sc");
                System.Windows.MessageBox.Show("Datos guardados y colocados en la carpeta configurada", "Guardar", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex, "Error!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

        }
        //Devuelve el nombre de la sucursal segun su id
        public string GetSucursalById(int id)
        {
            return sc.Sucursales.Where(x => x.idSucursal == id).Select(x => x.sucursal1).FirstOrDefault();
        }
        //Devuelve el id de la sucursal segun el mombre de la sucursal
        public int GetIdBySucursal(string sucursal)
        {
            return sc.Sucursales.Where(x => x.sucursal1 == sucursal).Select(x => x.idSucursal).FirstOrDefault();
        }
        public string GetSucursalbyIdEmpleado(int idEmpleado)
        {
            return (from emp in sc.Empleados
                    join tb in sc.Trabajos on emp.idEmpleado equals tb.idEmpleado
                    join sc in sc.Sucursales on tb.idSucursal equals sc.idSucursal
                    where emp.idEmpleado == idEmpleado
                    select sc.sucursal1).FirstOrDefault();
        }
        public string GetNombreEmpleadoById(int idEmpleado)
        {
            return sc.Empleados.Where(x => x.idEmpleado == idEmpleado).Select(x => x.nombre + " " + x.apellido).SingleOrDefault();
        }
        public int GetIdEmpleadoByNombre(string nombre)
        {
            return (from emp in sc.Empleados where (emp.nombre + " " + emp.apellido).Trim().Contains(nombre.Trim()) select emp.idEmpleado).FirstOrDefault();
        }
        private string GetStringArchivo(string NombreSucursal, DateTime Desde, DateTime Hasta)
        {
            return NombreSucursal + " " + Desde.Day + "-" + Desde.ToString("MMM").Substring(0, Desde.ToString("MMM").Length - 1) + " a " + Hasta.Day + "-" + Hasta.ToString("MMM").Substring(0, Hasta.ToString("MMM").Length - 1);
        }

        public DatosSucursalModel CargarDatos()
        {
            Crypto.Decrypt(heFN, heFNo);
            HorasExtraModel.Load = true;
            XmlSerializer xml = new XmlSerializer(typeof(DatosSucursalModel));
            DatosSucursalModel dsm;
            try
            {
                using (FileStream fileStream = new FileStream(heFNo, FileMode.Open))
                {
                    dsm = (DatosSucursalModel)xml.Deserialize(fileStream);
                }
            }
            catch (Exception)
            {
                dsm = new DatosSucursalModel();
            }
            File.Delete(heFNo);
            foreach (var dm in dsm.DatosMarcacion)
            {
                dm._NombreEmpleado = GetNombreEmpleadoById(dm.idEmpleado);
            }
            HorasExtraModel.Load = false;
            return dsm;
        }
    }
}
