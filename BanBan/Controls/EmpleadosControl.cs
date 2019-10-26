using BanBan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BanBan.Controls
{
    class EmpleadosControl : Utilidades
    {
        private empleado emp;
        //IQueryable para evitar hacer tantas consultas a la BD (NO LO HACE :'v)
        private readonly IQueryable<sistemapension> sp;
        private readonly IQueryable<cargo> cr;
        private readonly IQueryable<atencion> at;
        private readonly IQueryable<empleado> em;
        public EmpleadosControl()
        {
            sp = from sisp in sb.sistemapension select sisp;
            cr = from car in sb.cargo select car;
            at = from aten in sb.atencion select aten;
            em = from empl in sb.empleado select empl;
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
        public List<string> getEmpleados()
        {
            return (from emp in em select (emp.nombre + " " + emp.apellido)).ToList() ?? new List<string>();
        }
        public int getIdEmpleado(string empleado)
        {
            return (from emp in em where (emp.nombre + " " + emp.apellido).Equals(empleado) select emp.idEmpleado).First();
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
        public string save(string nombre, string apellido, string DUI, string NIT, DateTime fechaC, DateTime? fechaD, string afiliadoA,
            string nAfiliado, string ISSS, string sucursal, string cargo, string sueldo, string telefono, bool estado,
            List<string> sucursales, List<string> atenciones)
        {

            emp = Pages.Empleados.cargarEdit ? cargarEmpleado(Pages.Empleados.idEdit) : new empleado();

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

            sb.empleado.Add(emp);
            sb.SaveChangesAsync();

            guardarTrabajo(emp.idEmpleado, getIdSucursal(sucursal));
            sucursales.Remove(sucursal);

            if (!string.IsNullOrEmpty(telefono)) guardarTelefonos(telefono);
            if (sucursales.Count > 0) guardarSucursalesA(sucursales, emp.idEmpleado);
            if (atenciones.Count > 0) guardarAtenciones(atenciones, emp.idEmpleado);
            try
            {
                sb.SaveChanges();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "OK";
        }

        private empleado cargarEmpleado(int id)
        {
            return (from em in sb.empleado
                    where em.idEmpleado.Equals(id)
                    select em).Single() ?? new empleado();
        }

        private void guardarAtenciones(List<string> atenciones, int idEmpleado)
        {
            foreach (var atencion in atenciones)
            {
                atenciondetalle atd = new atenciondetalle()
                {
                    idAtencion = getIdAtencion(atencion),
                    idEmpleado = idEmpleado,
                    estado = true
                };
                sb.atenciondetalle.Add(atd);
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
                telefono tele = new telefono()
                {
                    idEmpleado = emp.idEmpleado,
                    telefono1 = tel.Trim()
                };
                sb.telefono.Add(tele);
            }
        }
        private void guardarTrabajo(int idEmpleado, int idSucursal)
        {
            trabajo tb = new trabajo()
            {
                idEmpleado = idEmpleado,
                idSucursal = idSucursal
            };
            sb.trabajo.Add(tb);
        }

        public EmpleadosModel getEmpleado(int idEmpleado)
        {
            var emp = (from emple in sb.empleado
                       join cg in sb.cargo on emple.idCargo equals cg.idCargo
                       join sp in sb.sistemapension on emple.idSistemaPension equals sp.idSistemaPension
                       where emple.idEmpleado.Equals(idEmpleado)
                       select new EmpleadosModel
                       {
                           Nombre = emple.nombre,
                           Apellido = emple.apellido,
                           Activo = emple.estado,
                           DUI = emple.dui,
                           NIT = emple.nit,
                           FechaContrato = emple.fechaIngreso,
                           FechaDespido = emple.fechaSalida,
                           AfiliadoA = sp.sistemaP, 
                           NumeroAfiliado = emple.numeroPension,
                           ISSS = emple.numeroISSS,
                           Cargo = cg.cargo1,
                           SueldoBase = emple.sueldo
                       }).FirstOrDefault() ?? new EmpleadosModel();
            emp.Sucursales = (from sc in sb.sucursal
                              join tb in sb.trabajo on sc.idSucursal equals tb.idSucursal
                              where tb.idEmpleado.Equals(idEmpleado)
                              select sc.sucursal1).ToList() ?? new List<string>();
            emp.Telefonos = (from tf in sb.telefono
                             where tf.idEmpleado.Equals(idEmpleado)
                             select tf.telefono1).ToList() ?? new List<string>();
            emp.Atenciones = (from atd in sb.atenciondetalle
                              join at in sb.atencion on atd.idAtencion equals at.idAtencion
                              where atd.idEmpleado.Equals(idEmpleado)
                              select at.atencion1).ToList() ?? new List<string>();
            return emp;
        }
    }
}