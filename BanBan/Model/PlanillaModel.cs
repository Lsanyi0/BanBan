using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            get { return decimal.Round(decimal.Floor(Horas) * SueldoBase, Decimales); }
            set { }
        }
        //Cantidad de Horas trabajadas redondeadas al digito inferior
        public decimal Horas { get; set; }
        //Cantidad de horas despues de la hora de trabajo normal aprobadas
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
        //???
        public decimal Nocturnidad { get; set; } //falta
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
            get { return decimal.Round(decimal.Floor(HorasNocturnas) * (1.25M * 0.25M), Decimales); }
            set { }
        }
        //Monto por las horas trabajadas despues del horario normal
        public decimal TotalHorasExtra
        {
            get { return decimal.Round(HorasExtra * 1.25M, 2); }
            set { }
        }
        //Indica la cantidad de dinero por las horas nocturnas trabajadas
        public decimal TotalHorasExtraNocturnas
        {
            get { return decimal.Round(HorasNocturnasExtra * (3.125M), Decimales); }
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
        //Monto por horas trabajadas en dias de asueto
        public decimal TotalAsuetos
        {
            get { return decimal.Round(HorasAsueto * 1.25M, Decimales); }
            set { }
        }
        //Total sin descuentos ni prestaciones
        public decimal TotalDevengado
        {
            get
            {
                return Sueldo
                + TotalHorasExtra
                + TotalHorasNocturnas
                + TotalHorasExtraNocturnas
                + TotalAsuetos;
            }
            set { }
        }
        //Porcentage de descuento por seguro (ISSS)
        public const decimal SeguroEmpleado = 0.03M;

        public decimal TotalSeguroEmpleado
        {
            get { return decimal.Round(SeguroEmpleado * TotalDevengado, Decimales); }
            set { }
        }
        //Porcentage de retencion por AFP
        public decimal AFPEmpleado { get; set; }

        public decimal TotalAFPEmpleado
        {
            get { return decimal.Round((AFPEmpleado * TotalDevengado) / 100, Decimales); }
            set { }
        }
        public decimal Renta { get; set; } //falta
        //Total con deducciones, descuentos
        public decimal TotalDeduccion
        {
            get { return decimal.Round(TotalSeguroEmpleado + TotalAFPEmpleado + Renta + TotalDescuento, Decimales); }
            set { }
        }
        //Total con deducciones, descuentos y prestaciones (Atenciones)
        public decimal NetoAPagar
        {
            get { return decimal.Round(TotalDevengado - TotalDeduccion, Decimales); }
            set { }
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}