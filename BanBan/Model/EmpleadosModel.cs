using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanBan.Model
{
    class EmpleadosModel
    {
        public int IdEmpleado { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string DUI {get; set;}
        public string NIT { get; set; }
        public DateTime FechaContrato { get; set; }
        public DateTime? FechaDespido { get; set; }
        public string AfiliadoA { get; set; }
        public string NumeroAfiliado { get; set; }
        public string ISSS { get; set; }
        public string Cargo { get; set; }
        public List<string> Telefonos { get; set; }
        public decimal? SueldoBase { get; set; }
        public List<string> Atenciones { get; set; }
        public List<string> Sucursales { get; set; }
        public bool Activo { get; set; }
    }
}
