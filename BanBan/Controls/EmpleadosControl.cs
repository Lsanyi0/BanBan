using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanBan.Controls
{
    class EmpleadosControl
    {
        private sBanBan sb;
        private Empleado emp;

        public EmpleadosControl()
        {
            sb = new sBanBan();
            emp = new Empleado();
        }
        public List<string> getSistemaPension()
        {
            var sp = from sisp in sb.SistemaPension select sisp.sistemaP;
            if (sp != null) return sp.ToList();
            else return null;
        }
        public List<string> getSucursal()
        {
            var sc = from suc in sb.Sucursal select suc.sucursal1;
            if (sc != null) return sc.ToList();
            else return null;
        }
        public List<string> getCargo()
        {
            var cr = from car in sb.Cargo select car.cargo1;
            if (cr != null) return cr.ToList();
            else return null;
        }
        //el metodo isVaild() retorna string para facilitar que campos son incorrectos al usuario
        private string isValid(string sueldo)
        {
            string v = "vacio";
            string e = "ya existente";
            if (string.IsNullOrEmpty(emp.nombre)) return "Nombre " + v;

            if (string.IsNullOrEmpty(emp.apellido)) return "Apellido " + v;
            //if nombre + apellido existe aqui and so on
            if (string.IsNullOrEmpty(emp.dui)) return "DUI " + v;
            //if Formato valido
            if (string.IsNullOrEmpty(emp.nit)) return "NIT " + v;
            if (emp.fechaIngreso == null) return "Fecha de contrato " + v; //<-- no la parece viable/util
            if (DateTime.Now < emp.fechaIngreso) return "Fecha de contrato no valida";
            //los combo se validan en la vista
            if (string.IsNullOrEmpty(emp.numeroPension)) return "Numero afiliado" + v;
            try
            {
                var sueld = decimal.Parse(sueldo);
            }
            catch (Exception)
            {
                return "sueldo " + v;
            }
            return "OK";
        }
        //falta telefono
        public string save(string nombre, string apellido, string DUI, string NIT, DateTime fechaC, string afiliadoA, string nAfiliado, string sucursal, string cargo, string sueldo, string telefono, bool estado)
        {

            emp.nombre = nombre;
            emp.apellido = apellido;
            emp.dui = DUI;
            emp.nit = NIT;
            emp.fechaIngreso = fechaC;
            emp.numeroPension = nAfiliado;
            emp.numeroISSS = "456465";
            emp.estado = estado;

            var valid = isValid(sueldo);

            if (valid != "OK") return valid;

            emp.sueldo = decimal.Parse(sueldo);
            //maybe un try
            emp.idSistemaPension = (from sp in sb.SistemaPension where sp.sistemaP == afiliadoA select sp.idSistemaPension).First();
            emp.idCargo = (from cg in sb.Cargo where cg.cargo1 == cargo select cg.idCargo).First();

            sb.Empleado.Add(emp);
            sb.SaveChangesAsync();
            Trabajo tb = new Trabajo();
            tb.idEmpleado = emp.idEmpleado;
            tb.idSucursal = (from sc in sb.Sucursal where sc.sucursal1 == sucursal select sc.idSucursal).First();
            sb.Trabajo.Add(tb);
            sb.SaveChangesAsync();

            return "OK";
        }
    }
}
