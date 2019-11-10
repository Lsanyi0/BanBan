using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanBan.Model 
{
    class PlanillaModel : AtencionesPlanillaModel
    {
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
            get { return Sueldo; }
            set { Sueldo = NumeroDias * Horas * SueldoBase; }
        }
        //Cantidad de Horas trabajadas redondeadas al digito inferior
        public decimal Horas
        {
            get { return Horas; }
            set { Horas = decimal.Floor(Horas); }
        }
        //Cantidad de horas despues de la hora de trabajo normal aprovadas
        public decimal HorasExtra { get; set; }
        //???
        public decimal Nocturnidad { get; set; } //falta
        //Indica la cantidad de Horas trabajadas en horario nocturno rutinarias
        public decimal HorasNocturnas
        {
            get { return HorasNocturnas; }
            set { HorasNocturnas = decimal.Floor(HorasNocturnas); }
        }
        //Indica la cantidad de Horas extra trabajadas de noche
        public decimal HorasNocturnasExtra { get; set; }
        //Monto por horas rutinarias trabajadas de noche 
        public decimal TotalHorasNocturnas
        {
            get { return HorasNocturnas; }
            set { TotalHorasNocturnas = NumeroDias * HorasNocturnas * (1.25M * 0.25M); }
        }
        //Monto por las horas trabajadas despues del horario normal
        public decimal TotalHorasExtra
        {
            get { return TotalHorasExtra; }
            set { TotalHorasExtra = (HorasExtra / 2) * 1.25M; }
        }
        //Indica la cantidad de dinero por las horas nocturnas trabajadas
        public decimal TotalHorasExtraNocturnas
        {
            get { return TotalHorasExtra; }
            set { TotalHorasExtra = HorasNocturnasExtra * (3.125M); }
        }
        //Horas trabajadas en dia de asueto
        public decimal HorasAsueto { get; set; }
        //Monto por horas trabajadas en dias de asueto
        public decimal TotalAsuetos
        {
            get { return TotalAsuetos; }
            set { TotalAsuetos = HorasAsueto * 1.25M; }
        }
        //Total sin descuentos ni prestaciones
        public decimal TotalDevengado
        {
            get { return TotalDevengado; }
            set
            {
                TotalDevengado = Sueldo
                  + TotalHorasExtra
                  + TotalHorasNocturnas
                  + TotalHorasExtraNocturnas
                  + TotalAsuetos;
            }
        }
        //Porcentage de descuento por seguro (ISSS)
        public const decimal SeguroEmpleado = 0.03M;
        public decimal TotalSeguroEmpleado
        {
            get { return TotalSeguroEmpleado; }
            set { TotalSeguroEmpleado = SeguroEmpleado * TotalDevengado; }
        }
        //Porcentage de retencion por AFP
        public decimal AFPEmpleado { get; set; }
        public decimal TotalAFPEmpleado
        {
            get { return TotalAFPEmpleado; }
            set { TotalAFPEmpleado = AFPEmpleado * TotalDevengado; }
        }
        public decimal Renta { get; set; } //falta
        //Total con deducciones, descuentos
        public decimal TotalDeduccion
        {
            get { return TotalDeduccion; }
            set { TotalDeduccion = SeguroEmpleado + AFPEmpleado + Renta + TotalDesuento; }
        }

        //Total con deducciones, descuentos y prestaciones (Atenciones)
        public decimal NetoAPagar
        {
            get { return NetoAPagar; }
            set { NetoAPagar = TotalDevengado - TotalDeduccion; }
        }
    }
}