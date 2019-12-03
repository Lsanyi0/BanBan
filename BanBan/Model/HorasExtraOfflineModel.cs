using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanBan.Model
{
    public class HorasExtraOfflineModel
    {
        public HorasExtraOfflineModel()
        {
            Usuarios = new List<Usuario>();
            Empleados = new List<Empleado>();
            Sucursales = new List<Sucursal>();
            TipoHoras = new List<TipoHora>();
            Trabajos = new List<Trabajo>();
        }
        public List<Usuario> Usuarios { get; set; }
        public List<Empleado> Empleados { get; set; }
        public List<Sucursal> Sucursales { get; set; }
        public List<TipoHora> TipoHoras { get; set; }
        public List<Trabajo> Trabajos { get; set; }
    }
    public class Usuario
    {
        public int idUsuario { get; set; }
        public string usuario { get; set; }
        public string contrasena { get; set; }
        public bool reseteo { get; set; }
        public int idEmpleado { get; set; }
        public int idTipo { get; set; }
    }
    public class Empleado
    {
        public int idEmpleado { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public bool estado { get; set; }
        public int idCargo { get; set; }
    }
    public class Sucursal
    {
        public int idSucursal { get; set; }
        public string sucursal1 { get; set; }
    }
    public class TipoHora
    {
        public int idTipoHora { get; set; }
        public string tipo { get; set; }
    }
    public class Trabajo
    {
        public int idTrabajo { get; set; }
        public int idEmpleado { get; set; }
        public int idSucursal { get; set; }
    }
    public class Dispositivo
    {
        public int idDispositivo { get; set; }
        public string ip { get; set; }
        public int idSucursal { get; set; }
    }
}
