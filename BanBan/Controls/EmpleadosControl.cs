using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BanBan.Controls
{
    class EmpleadosControl
    {
        private Empleado emp;
        private List<string> lv = new List<string>();
        //IQueryable para evitar hacer tantas consultas a la BD
        private readonly IQueryable<SistemaPension> sp;
        private readonly IQueryable<Sucursal> sc;
        private readonly IQueryable<Cargo> cr;
        private readonly IQueryable<Atencion> at;
        public EmpleadosControl()
        {
            emp = new Empleado();
            sp = from sisp in Utilidades.sb.SistemaPension select sisp;
            sc = from suc in Utilidades.sb.Sucursal select suc;
            cr = from car in Utilidades.sb.Cargo select car;
            at = from aten in Utilidades.sb.Atencion select aten;
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
            return (from suc in sc where suc.sucursal1.Equals(sucursal) select suc.idSucursal).First();
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
            return (from car in cr where car.cargo1.Equals(cargo) select car.idCargo).First();
        }
        public List<string> getAtenciones()
        {
            if (at != null) return (from aten in at select aten.atencion1).ToList();
            else return lv;
        }

        private int getIdAtencion(string atencion)
        {
            return (from aten in at where aten.atencion1.Contains(atencion) select aten.idAtencion).First();
        }
        //el metodo isVaild() retorna string para facilitar que campos son incorrectos al usuario
        private string isValid(string sueldo, string tel)
        {
            Regex duiRegex = new Regex(@"^\d{8}-\d{1}$");
            Regex nitRegex = new Regex(@"^\d{4}-\d{6}-\d{3}-\d{1}$");
            string v = "vacio.";
            //string e = "ya existente";
            string i = "contiene formato invalido.";
            if (string.IsNullOrWhiteSpace(emp.nombre)) return "Nombre " + v;

            if (string.IsNullOrWhiteSpace(emp.apellido)) return "Apellido " + v;
            //if nombre + apellido existe aqui and so on
            if (string.IsNullOrWhiteSpace(emp.dui)) return "DUI " + v;
            if (!duiRegex.IsMatch(emp.dui)) return "DUI" + i;
            //if Formato valido
            if (string.IsNullOrWhiteSpace(emp.nit)) return "NIT " + v;
            if (!nitRegex.IsMatch(emp.nit)) return "NIT " + i;
            if (emp.fechaIngreso == null) return "Fecha de contrato " + v; //<-- no la parece viable/util

            if (DateTime.Now < emp.fechaIngreso) return "Fecha de contrato no valida";
            //los combo se validan en la vista en teoria no pueden estar vacios
            if (string.IsNullOrWhiteSpace(emp.numeroPension)) return "Numero afiliado " + v;
            if (string.IsNullOrWhiteSpace(emp.numeroISSS)) return "Numero de ISSS " + v;
            try { emp.sueldo = decimal.Parse(sueldo); }
            catch (Exception) { return "sueldo " + v; }
            if (!string.IsNullOrWhiteSpace(tel))
            {
                if (Regex.Matches(tel, @"(\d{4}-\d{4}(,?, ?)?)\1?").Count != tel.Split(',').Count())
                {
                    return "Numero de telefono " + v + " o " + i
                    + " \n\nRevise cada uno de los telefonos y no coloque ',' cuando ingrese el ultimo telefono.";
                }
            }
            return "OK";
        }
        //falta ISSS
        public string save(string nombre, string apellido, string DUI, string NIT, DateTime fechaC, string afiliadoA,
            string nAfiliado, string ISSS, string sucursal, string cargo, string sueldo, string telefono, bool estado,
            List<string> sucursales, List<string> atenciones)
        {
            Utilidades.sb = new sBanBan();
            emp.nombre = nombre;
            emp.apellido = apellido;
            emp.dui = DUI;
            emp.nit = NIT;
            emp.fechaIngreso = fechaC;
            emp.numeroPension = nAfiliado;
            emp.numeroISSS = ISSS;
            emp.estado = estado;
            //sueldo se asigna en la validacion
            var valid = isValid(sueldo, telefono);

            if (valid != "OK") return valid;

            emp.idSistemaPension = getIdSistemaPensiones(afiliadoA);
            emp.idCargo = getIdCargo(cargo);

            Utilidades.sb.Empleado.Add(emp);
            Utilidades.sb.SaveChangesAsync();

            guardarTrabajo(emp.idEmpleado, getIdSucursal(sucursal));
            sucursales.Remove(sucursal);

            if (!string.IsNullOrEmpty(telefono)) guardarTelefonos(telefono);
            if (sucursales.Count > 0) guardarSucursalesA(sucursales, emp.idEmpleado);
            if (atenciones.Count > 0) guardarAtenciones(atenciones, emp.idEmpleado);
            try
            {
                Utilidades.sb.SaveChanges();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "OK";
        }

        private void guardarAtenciones(List<string> atenciones, int idEmpleado)
        {
            foreach (var atencion in atenciones)
            {
                AtencionDetalle atd = new AtencionDetalle()
                {
                    idAtencion = getIdAtencion(atencion),
                    idEmpleado = idEmpleado,
                    estado = true
                };
                Utilidades.sb.AtencionDetalle.Add(atd);
            }
        }

        public void guardarSucursalesA(List<string> sucursales, int idEmpleado)
        {
            foreach (var sucursal in sucursales)
            {
                guardarTrabajo(idEmpleado, getIdSucursal(sucursal));
            }
        }

        private void guardarTelefonos(string telefono)
        {
            foreach (var tel in telefono.Split(','))
            {
                Telefono tele = new Telefono()
                {
                    idEmpleado = emp.idEmpleado,
                    telefono1 = tel.Trim()
                };
                Utilidades.sb.Telefono.Add(tele);
            }
        }
        private void guardarTrabajo(int idEmpleado, int idSucursal)
        {
            Trabajo tb = new Trabajo()
            {
                idEmpleado = idEmpleado,
                idSucursal = idSucursal
            };
            Utilidades.sb.Trabajo.Add(tb);
        }
    }
}
