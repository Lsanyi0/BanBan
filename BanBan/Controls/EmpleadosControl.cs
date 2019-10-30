using BanBan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

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
        private bool edit { get; set; }
        public EmpleadosControl()
        {
            sp = from sisp in sb.sistemapension select sisp;
            cr = from car in sb.cargo select car;
            at = from aten in sb.atencion select aten;
            em = from empl in sb.empleado select empl;
            edit = false;
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
            if (emp.fechaIngreso > emp.fechaSalida) return "Fecha de despido no puede ser anterior a fecha de contrato";
            //los combo se validan en la vista en teoria no pueden estar vacios
            if (string.IsNullOrWhiteSpace(emp.numeroPension)) return "Numero afiliado " + v;
            if (emp.numeroPension.Length != 9) return "Numero de afiliado " + i;

            if (string.IsNullOrWhiteSpace(emp.numeroISSS)) return "Numero de ISSS " + v;
            if (emp.numeroISSS.Length != 12) return "Numero de ISSS " + i;

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
            List<string> sucursales, List<AtencionesModel> atenciones, bool edit)
        {
            this.edit = edit;
            emp = edit ? cargarEmpleado(Pages.Empleados.idEdit) : new empleado();

            emp.nombre = nombre;
            emp.apellido = apellido;
            emp.dui = DUI;
            emp.nit = NIT;
            emp.fechaIngreso = fechaC;
            emp.numeroPension = nAfiliado;
            emp.numeroISSS = ISSS;
            emp.estado = estado;
            emp.fechaSalida = fechaD;
            //sueldo se asigna en la validacion
            var valid = isValid(sueldo, telefono);

            if (valid != "OK") return valid;

            emp.idSistemaPension = getIdSistemaPensiones(afiliadoA);
            emp.idCargo = getIdCargo(cargo);

            if (!edit)
            {
                sb.empleado.Add(emp);
                sb.SaveChangesAsync();
            }

            guardarTrabajo(emp.idEmpleado, getIdSucursal(sucursal));
            if (!sucursales.Contains(sucursal)) sucursales.Add(sucursal);

            sb.SaveChangesAsync();

            if (!string.IsNullOrEmpty(telefono)) guardarTelefonos(telefono);
            guardarSucursalesA(sucursales);
            if (atenciones.Count > 0) guardarAtenciones(atenciones);
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

        public void guardarAtenciones(List<AtencionesModel> atenciones)
        {
            if (edit)
            {
                foreach (var at in atenciones.Reverse<AtencionesModel>())
                {
                    int idAtencion = getIdAtencion(at.Atencion);
                    var ad = (from adt in sb.atenciondetalle where adt.idEmpleado.Equals(emp.idEmpleado) && adt.idAtencion.Equals(idAtencion) select adt).FirstOrDefault() ?? null;
                    if (ad != null)
                    {
                        ad.estado = at.Activa;
                        atenciones.Remove(at);
                    }
                }
            }
            foreach (var atencion in atenciones)
            {
                atenciondetalle atd = new atenciondetalle()
                {
                    idAtencion = getIdAtencion(atencion.Atencion),
                    idEmpleado = emp.idEmpleado,
                    estado = true
                };
                sb.atenciondetalle.Add(atd);
            }
        }

        private void guardarSucursalesA(List<string> SucursalesASupervisar)
        {
            if (edit)
            {
                var SucursalesBD = (from tb in sb.trabajo.Include("sucursal") where tb.idEmpleado.Equals(emp.idEmpleado) select tb).ToList();
                List<string> SucursalesAconservar = new List<string>();
                foreach (var sc in SucursalesBD)
                {
                    if (SucursalesASupervisar.Contains(sc.sucursal.sucursal1))
                    {
                        SucursalesASupervisar.Remove(sc.sucursal.sucursal1);
                        SucursalesAconservar.Add(sc.sucursal.sucursal1);
                    }
                }
                guardarSucursalA(SucursalesASupervisar);
                try { sb.SaveChanges(); }
                catch (Exception ex) { MessageBox.Show(ex.Message); throw; }
                SucursalesBD = (from tb in sb.trabajo.Include("sucursal") where tb.idEmpleado.Equals(emp.idEmpleado) select tb).ToList();
                foreach (var sc in SucursalesBD)
                {
                    if (!SucursalesAconservar.Contains(sc.sucursal.sucursal1) && !SucursalesASupervisar.Contains(sc.sucursal.sucursal1))
                    {
                        sb.trabajo.Remove(sc);
                    }
                }
            }
            else
            {
                guardarSucursalA(SucursalesASupervisar);
            }
        }

        private void guardarSucursalA(List<string> sucursalesASupervisar)
        {
            foreach (var sc in sucursalesASupervisar)
            {
                trabajo trabajo = new trabajo()
                {
                    idSucursal = getIdSucursal(sc),
                    idEmpleado = emp.idEmpleado
                };
                sb.trabajo.Add(trabajo);
            }
        }

        private void guardarTelefonos(string telefono)
        {
            if (edit)
            {
                var telefonos = telefono.Split(',').Select(p => p.Trim()).ToList();
                var telefonosBD = (from t in sb.telefono where t.idEmpleado.Equals(emp.idEmpleado) select t).ToList();
                List<string> telefonosConservar = new List<string>();
                //Comparamos que telefonos se encuentan en la DB si existe se remueve de la lista del form para evitar duplicados
                foreach (var tel in telefonosBD)
                {
                    if (telefonos.Contains(tel.telefono1))
                    {
                        telefonos.Remove(tel.telefono1);
                        telefonosConservar.Add(tel.telefono1);
                    }
                }
                //Guardamos los telefonos nuevos
                foreach (var tel in telefonos)
                {
                    guardarTelefono(tel);
                }
                try { sb.SaveChanges(); }
                catch (Exception ex) { MessageBox.Show(ex.Message); throw; }
                //Removemos los telefonos que no se encuentran en la lista del form ni en la lista de tel a conservar
                telefonosBD = (from t in sb.telefono where t.idEmpleado.Equals(emp.idEmpleado) select t).ToList();
                foreach (var tel in telefonosBD)
                {
                    if (!telefonos.Contains(tel.telefono1) && !telefonosConservar.Contains(tel.telefono1))
                    {
                        sb.telefono.Remove(tel);
                    }
                }
            }
            else
            {
                foreach (var tel in telefono.Split(',').Select(p => p.Trim()).ToList())
                {
                    guardarTelefono(tel);
                }
            }
        }

        private void guardarTelefono(string tel)
        {
            telefono tele = new telefono()
            {
                idEmpleado = emp.idEmpleado,
                telefono1 = tel
            };
            sb.telefono.Add(tele);
        }

        private void guardarTrabajo(int idEmpleado, int idSucursal)
        {
            trabajo tb = edit ? (from trb in sb.trabajo where trb.idEmpleado.Equals(idEmpleado) select trb).First() : new trabajo();
            tb.idEmpleado = idEmpleado;
            tb.idSucursal = idSucursal;
            if (!edit) sb.trabajo.Add(tb);
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