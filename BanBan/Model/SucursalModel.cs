using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanBan.Model
{
    class SucursalModel
    {
        public int IdSucursal { get; set; }
        public string NombreSucursal { get; set; }
        public int IdDepartamento { get; set; }
        public string Departamento { get; set; }
        public int IdMunicipio { get; set; }
        public string Municipio { get; set; }
        public string Direccion { get; set; }
        public List<DateTime> DiasAsueto { get; set; }
        public int IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public string Supervisor { get; set; }
    }
}
