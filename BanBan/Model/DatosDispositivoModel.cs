using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanBan.Model
{
    //Modelo para almacenar los datos del dispositivo en xml
    public class DatosDispositivoModel : IDataErrorInfo
    {
        public string this[string name]
        {
            get 
            {
                string result = null;
                switch (name)
                {
                    case "Entrada":
                        if (Entrada == default)
                        {
                            result = "No se marco entrada";
                        }
                        break;
                    case "Salida":
                        if (Salida == default)
                        {
                            result = "No se marco salida";
                        }
                        break;
                }
                return result;
            }
        }

        public int idEmpleado { get; set; }
        public DateTime Entrada { get; set; }
        public DateTime Salida { get; set; }

        public string Error { get { return null; } }
    }
}
