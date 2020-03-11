using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace BanBan.Model
{
    //Modelo para almacenar los datos del dispositivo en xml
    public class DatosDispositivoModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public string Error { get { return null; } }
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
                        if (Entrada > Salida && !(Entrada == default || Salida == default))
                        {
                            result = "Hora inicial no puede ser mayor a la de salida";
                        }
                        break;
                    case "Salida":
                        if (Salida == default)
                        {
                            result = "No se marco salida";
                        }
                        if (Salida < Entrada && !(Entrada == default || Salida == default))
                        {
                            result = "Hora inicial no puede ser mayor a la de salida";
                        }
                        break;
                }
                return result;
            }
        }
        public int idDDM { get; set; }
        public int idEmpleado { get; set; }
        [XmlIgnore]
        public string _NombreEmpleado { get; set; }
        public string NombreEmpleado { get { return _NombreEmpleado; } }
        private DateTime _Entrada { get; set; }
        public DateTime Entrada
        {
            get { return _Entrada; }
            set
            {
                _Entrada = value;
                OnPropertyChanged("Actualizar");
            }
        }
        private DateTime _Salida { get; set; }
        public DateTime Salida
        {
            get { return _Salida; }
            set
            {
                _Salida = value;
                OnPropertyChanged("Actualizar");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
