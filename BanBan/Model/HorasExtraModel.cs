using System;
using System.ComponentModel;

namespace BanBan.Model
{
    public class HorasExtraModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static bool Load;
        private bool AlertShown = false;
        public int IdHe { get; set; }
        public string Sucursal { get; set; }
        public int IdEmpleado { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NombreCompleto { get { return Nombre + " " + Apellido; } set { } }
        private DateTime _HoraInicio { get; set; }
        public DateTime HoraInicio
        {
            get { return _HoraInicio; }
            set
            {
                _HoraInicio = value.AddTicks(-(value.Ticks % TimeSpan.TicksPerMinute));
                OnPropertyChanged("Actualizar");
            }
        }
        private DateTime _HoraFinal { get; set; }
        public DateTime HoraFinal
        {
            get { return _HoraFinal; }
            set
            {
                _HoraFinal = value.AddTicks(-(value.Ticks % TimeSpan.TicksPerMinute)); //para descontar los segundos
                OnPropertyChanged("Actualizar");
            }
        }
        private bool _HoraExtra { get; set; }
        public bool HoraExtra
        {
            get { return _HoraExtra; }
            set
            {
                _HoraExtra = CheckForCell() ? value : default;
                OnPropertyChanged("Actualizar");
            }
        }
        private bool _HoraAsueto { get; set; }
        public bool HoraAsueto
        {
            get { return _HoraAsueto; }
            set
            {
                _HoraAsueto = CheckForCell() ? value : default;
                OnPropertyChanged("Actualizar");
            }
        }
        private bool _HoraExtraNocturna { get; set; }
        public bool HoraExtraNocturna
        {
            get { return _HoraExtraNocturna; }
            set
            {
                _HoraExtraNocturna = CheckForCell() ? value : default;
                OnPropertyChanged("Actualizar");
            }
        }
        private bool _HoraDescanso { get; set; }
        public bool HoraDescanso
        {
            get { return _HoraDescanso; }
            set
            {
                _HoraDescanso = CheckForCell() ? value : default;
                OnPropertyChanged("Actualizar");
            }
        }
        private decimal _Descuento { get; set; }
        public decimal Descuento
        {
            get { return _Descuento; }
            set
            {
                _Descuento = CheckForCell() ? value : default;
                OnPropertyChanged("Actualizar");
            }
        }
        public string _Comentario { get; set; }
        public string Comentario
        {
            get { return _Comentario; }
            set
            {
                _Comentario = value;
                OnPropertyChanged("Actualizar");
            }
        }

        private bool CheckForCell()
        {
            bool cell = true;
            if (!Load)
            {
                if (HoraExtra) cell = false;
                if (HoraExtraNocturna) cell = false;
                if (HoraAsueto) cell = false;
                if (HoraDescanso) cell = false;
                if (Descuento != default) cell = false;
                if (!cell && !AlertShown)
                {
                    System.Windows.MessageBox.Show("Cada columna solo puede contener un tipo de hora " +
                    "(o descuento) más un comentario (opcional)", "Advertencia", System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);
                    AlertShown = true; // para evitar que se muestre de manera constante; una vez por ejecucion.
                }
            }
            return cell;
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
