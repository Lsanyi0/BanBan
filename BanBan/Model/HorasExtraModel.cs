using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanBan.Model
{
    class HorasExtraModel
    {
        public int IdEmpleado { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NombreCompleto { get { return Nombre + " " + Apellido; } set { } }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFinal { get; set; }
        public decimal HoraExtra { get; set; }
        public decimal HoraExtraNocturna { get; set; }
        public decimal HoraAsueto { get; set; }
        public decimal Descuento { get; set; }
        public string Comentario { get; set; }
    }
}
