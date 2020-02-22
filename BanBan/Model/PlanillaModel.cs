using System.Collections.Generic;
using System.ComponentModel;

namespace BanBan.Model
{
    public class PlanillaModel : AtencionesPlanillaModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public PlanillaModel()
        {
            Descuento = new List<decimal>();
            Atencion = new List<decimal>();
        }
        public const decimal JornadaLaboral = 8;
        public int IdEmpleado { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string NombreCompleto
        {
            get { return Nombre + " " + Apellido; }
            set { }
        }
        //Cantidad de dias trabajados
        public int NumeroDias { get; set; }
        //Sueldo Base del empleado
        public decimal SueldoBase { get; set; }
        //Sueldo basico sin prestaciones ni Horas extra
        public decimal Sueldo
        {
            get { return (Horas / JornadaLaboral) * SueldoBase; }
            set { }
        }
        public decimal DescuentoAusencia
        {
            get { return HorasAusencia * (SueldoBase / JornadaLaboral); }
            set { }
        }
        public decimal HorasAusencia { get; set; }
        //Cantidad de Horas trabajadas
        public decimal Horas { get; set; }
        //Cantidad de horas despues de la hora de trabajo normal 
        private decimal _HorasExtra;
        public decimal HorasExtra
        {
            get { return _HorasExtra; }
            set
            {
                _HorasExtra = value;
                OnPropertyChanged("HorasExtra");
            }
        }
        //Indica la cantidad de Horas trabajadas en horario nocturno rutinarias
        public decimal HorasNocturnas { get; set; }
        //Indica la cantidad de Horas extra trabajadas de noche
        private decimal _HorasNocturnasExtra;
        public decimal HorasNocturnasExtra
        {
            get { return _HorasNocturnasExtra; }
            set
            {
                _HorasNocturnasExtra = value;
                OnPropertyChanged("HorasNocturnasExtra");
            }
        }
        //Monto por horas rutinarias trabajadas de noche 
        public decimal TotalHorasNocturnas
        {
            get { return HorasNocturnas * (1.25M * 0.25M); }
            set { }
        }
        //Monto por las horas trabajadas despues del horario normal
        public decimal TotalHorasExtra
        {
            get { return HorasExtra * 1.25M; }
            set { }
        }
        //Indica la cantidad de dinero por las horas nocturnas trabajadas
        public decimal TotalHorasExtraNocturnas
        {
            get { return HorasNocturnasExtra * (3.125M); }
            set { }
        }
        //Horas trabajadas en dia de asueto
        private decimal _HorasAsueto;
        public decimal HorasAsueto
        {
            get { return _HorasAsueto; }
            set
            {
                _HorasAsueto = value;
                OnPropertyChanged("HorasAsueto");
            }
        }
        private decimal _HorasDescanso { get; set; }
        public decimal HorasDescanso
        {
            get { return _HorasDescanso; }
            set
            {
                _HorasDescanso = value;
                OnPropertyChanged("HorasDescanso");
            }
        }
        public decimal TotalHorasDescanso
        {
            get { return HorasDescanso * 1.25M; }
            set { }
        }
        //Monto por horas trabajadas en dias de asueto
        public decimal TotalAsuetos
        {
            get { return HorasAsueto * 1.25M; }
            set { }
        }
        //Total sin descuentos ni prestaciones
        public decimal TotalDevengado
        {
            get
            {
                return (Sueldo - DescuentoAusencia)
                + TotalHorasExtra
                + TotalHorasNocturnas //Nocturnidad
                + TotalHorasExtraNocturnas
                + TotalAsuetos
                + TotalHorasDescanso;
            }
            set { }
        }
        //Porcentage de descuento por seguro (ISSS)
        public const decimal SeguroEmpleado = 0.03M;

        public decimal TotalSeguroEmpleado
        {
            get { return SeguroEmpleado * TotalDevengado; }
            set { }
        }
        //Porcentage de retencion por AFP
        public decimal AFPEmpleado { get; set; }
        public decimal TotalAFPEmpleado
        {
            get { return (AFPEmpleado * TotalDevengado)/10; }//Cambio monto afp
            set { }
        }
        public decimal Renta { get; set; } //falta
        //Total con deducciones, descuentos
        public decimal TotalDeduccion
        {
            get { return TotalSeguroEmpleado + TotalAFPEmpleado + Renta + TotalDescuento; }
            set { }
        }
        //Total con deducciones, descuentos y prestaciones (Atenciones)
        public decimal NetoAPagar
        {
            get { return TotalDevengado - TotalDeduccion; }
            set { }
        }
        public decimal TotalNeto
        {
            get { return PorcentajeCargo + (TotalAtenciones * NumeroDias) - TotalDescuento; }
            set { }
        }
        private bool _Revisado { get; set; }
        public bool Revisado
        {
            get { return _Revisado; }
            set
            {
                _Revisado = value;
                OnPropertyChanged("Revisado");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}