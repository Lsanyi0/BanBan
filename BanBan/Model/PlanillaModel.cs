using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanBan.Model
{
    class PlanillaModel
    {
        public int IdEmpleado { get; set; }
        public string Apellido;
        public string Nombre { get; set; }
        public string NombreCompleto
        {
            get { return Nombre + " " + Apellido; }
        }
        public int NumeroDias { get; set; }
        public decimal Sueldo { get; set; }
        public double Horas { get; set; }
        public double PorNoctutnidad { get; set; }
        public double TotalHorasExtra { get; set; }
        public int Asuetos { get; set; }
        public double TotalHorasExtraNocturnas { get; set; }
        public double HorasNocturnas { get; set; }
        public decimal TotalDevengado { get; set; }
        public decimal SeguroEmpleado { get; set; }
        public decimal AFPEmpleado { get; set; }
        public decimal TotalDeduccion { get; set; }
        public decimal NetoAPagar { get; set; }

    }
}
