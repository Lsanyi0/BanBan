using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanBan.Model
{
    //Modelo para almacenar los datos del dispositivo en xml
    public class DatosDispositivoModel
    {
        public int idEmpleado { get; set; }
        public DateTime Entradas{ get; set; }
        public DateTime Salidas { get; set; }
    }
}
