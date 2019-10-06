using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BanBan.Controls
{
    class EmpleadosControl
    {
        private sBanBan sb;
        private Empleado emp;
        private List<string> lv = new List<string>();
        //IQueryable para evitar hacer tantas consultas a la BD
        private readonly IQueryable<SistemaPension> sp;
        private readonly IQueryable<Sucursal> sc;
        private readonly IQueryable<Cargo> cr;
        private readonly IQueryable<Atencion> at;
        public EmpleadosControl()
        {
            sb = new sBanBan();
            emp = new Empleado();
            sp = from sisp in sb.SistemaPension select sisp;
            sc = from suc in sb.Sucursal select suc;
            cr = from car in sb.Cargo select car;
        }
        public List<string> getSistemaPensiones()
        {
            if (sp != null) return (from sisp in sp select sisp.sistemaP).ToList();
            else return lv;
        }
        private int getIdSistemaPensiones(string sistemaP)
        {
            return (from sisp in sp where sisp.sistemaP == sistemaP select sisp.idSistemaPension).First();
        }
        public List<string> getSucursales()
        {
            if (sc != null) return (from suc in sc select suc.sucursal1).ToList();
            else return lv;
        }
        private int getIdSucursal(string sucursal)
        {
            return (from suc in sc where suc.sucursal1 == sucursal select suc.idSucursal).First();
        }
        public List<string> buscarSucursales(string sucursal)
        {
            List<string> sucursales = (from suc in sc where suc.sucursal1.Contains(sucursal) select suc.sucursal1).ToList();
            if (sucursales != null) return sucursales;
            else return (from suc in sc select suc.sucursal1).ToList();
        }
        public List<string> getCargos()
        {
            if (cr != null) return (from car in cr select car.cargo1).ToList();
            else return lv;
        }
        private int getIdCargo(string cargo)
        {
            return (from car in cr where car.cargo1.Contains(cargo) select car.idCargo).First();
        }
        //el metodo isVaild() retorna string para facilitar que campos son incorrectos al usuario
        private string isValid(string sueldo, string tel)
        {
            Regex duiRegex = new Regex(@"^\d{8}-\d{1}$");
            Regex nitRegex = new Regex(@"^\d{4}-\d{6}-\d{3}-\d{1}$");
            string v = "vacio.";
            //string e = "ya existente";
            string i = "contiene formato invalido.";
            if (string.IsNullOrEmpty(emp.nombre)) return "Nombre " + v;

            if (string.IsNullOrEmpty(emp.apellido)) return "Apellido " + v;
            //if nombre + apellido existe aqui and so on
            if (string.IsNullOrEmpty(emp.dui)) return "DUI " + v;
            if (!duiRegex.IsMatch(emp.dui)) return "DUI" + i;
            //if Formato valido
            if (string.IsNullOrEmpty(emp.nit)) return "NIT " + v;
            if (!nitRegex.IsMatch(emp.nit)) return "NIT " + i;
            if (emp.fechaIngreso == null) return "Fecha de contrato " + v; //<-- no la parece viable/util

            if (DateTime.Now < emp.fechaIngreso) return "Fecha de contrato no valida";
            //los combo se validan en la vista en teoria no pueden estar vacios
            if (string.IsNullOrEmpty(emp.numeroPension)) return "Numero afiliado " + v;
            try { emp.sueldo = decimal.Parse(sueldo); }
            catch (Exception) { return "sueldo " + v; }
            if (Regex.Matches(tel, @"(\d{4}-\d{4}(,?, ?)?)\1?").Count != tel.Split(',').Count())
            {
                return "Numero de telefono " + v +" o " + i
                + " \n\nRevise cada uno de los telefonos y no coloque ',' cuando ingrese el ultimo telefono.";
            }
            return "OK";
        }
        //falta ISSS y AFPs
        public string save(string nombre, string apellido, string DUI, string NIT, DateTime fechaC, string afiliadoA, string nAfiliado, string Sucursal, string Cargo, string sueldo, string telefono, bool estado)
        {
            emp.nombre = nombre;
            emp.apellido = apellido;
            emp.dui = DUI;
            emp.nit = NIT;
            emp.fechaIngreso = fechaC;
            emp.numeroPension = nAfiliado;
            emp.numeroISSS = "'vacio' de momento";
            emp.estado = estado;

            var valid = isValid(sueldo, telefono);

            if (valid != "OK") return valid;

            emp.idSistemaPension = getIdSistemaPensiones(afiliadoA);
            emp.idCargo = getIdCargo(Cargo);

            sb.Empleado.Add(emp);
            sb.SaveChangesAsync();
            Trabajo tb = new Trabajo();
            tb.idEmpleado = emp.idEmpleado;
            tb.idSucursal = getIdSucursal(Sucursal);
            sb.Trabajo.Add(tb);
            sb.SaveChangesAsync();

            return "OK";
        }
    }
}
