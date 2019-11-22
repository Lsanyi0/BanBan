using BanBan.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BanBan.Controls
{
    class PlanillasControl : Utilidades
    {
        public BindingList<PlanillaModel> getEmpleados()
        {
            List<empleado> empleados = (from em in sb.empleado.Include("sistemapension") select em).ToList();
            return getPlanillaModels();
        }
        public BindingList<PlanillaModel> getEmpleados(string sucursal)
        {
            int idSuc = getIdSucursal(sucursal);
            List<empleado> empleados = (from em in sb.empleado.Include("sistemapension")
                                        join tb in sb.trabajo on em.idEmpleado equals tb.idEmpleado
                                        join sc in sb.sucursal on tb.idSucursal equals sc.idSucursal
                                        where sc.idSucursal == idSuc && em.idCargo != 2 //xD
                                        select em
                                        ).ToList();
            return getPlanillaModels(empleados);
        }
        private BindingList<PlanillaModel> getPlanillaModels()
        {
            BindingList<PlanillaModel> pm = new BindingList<PlanillaModel>();
            pm.Add(new PlanillaModel()
            {
                Nombre = "Carolina",
                Apellido = "Alegria",
                NumeroDias = 15,
                Horas = (15 * 8),
                SueldoBase = 1.267375m,
                HorasNocturnas = 2,
                HorasExtra = 5,
                HorasAsueto = 8,
                AFPEmpleado = 7.25M,
            });
            pm.Add(new PlanillaModel()
            {
                Nombre = "Ernesto",
                Apellido = "Ramirez",
                NumeroDias = 14,
                Horas = (14 * 8),
                SueldoBase = 1.267375m,
                HorasNocturnas = 5,
                HorasAsueto = 8,
                AFPEmpleado = 7.25M
            });
            pm.Add(new PlanillaModel
            {
                Nombre = "Gerardo",
                Apellido = "Flores",
                NumeroDias = 12,
                Horas = (12 * 8),
                SueldoBase = 1.267375m,
                HorasNocturnas = 5,
                HorasExtra = 37,
                HorasAsueto = 8,
                AFPEmpleado = 7.25M
            }) ;
            return pm;
        }
        private BindingList<PlanillaModel> getPlanillaModels(List<empleado> empleados)
        {
            if (empleados != null)
            {
                BindingList<PlanillaModel> planillaModels = new BindingList<PlanillaModel>();
                foreach (empleado empleado in empleados)
                {
                    PlanillaModel pm = new PlanillaModel()
                    {
                        IdEmpleado = empleado.idEmpleado,
                        Nombre = empleado.nombre,
                        Apellido = empleado.apellido,
                        SueldoBase = empleado.sueldo ?? 0,
                        AFPEmpleado = empleado.sistemapension.descuento,
                        PorcentajeCargo = empleado.cargo.atenciones ?? 0,
                    };
                    List<TimeSpan?> Inicio = (from pln in sb.planillahorario where pln.idEmpleado.Equals(pm.IdEmpleado) select pln.entrada).ToList();
                    List<TimeSpan?> Fin = (from pln in sb.planillahorario where pln.idEmpleado.Equals(pm.IdEmpleado) select pln.salida).ToList();
                    pm.Horas = getHorasTrabajadas(Inicio, Fin);
                    planillaModels.Add(pm);
                }
                return planillaModels;
            }
            return new BindingList<PlanillaModel>();
        }
        public decimal getHorasTrabajadas(List<TimeSpan?> Iniciales, List<TimeSpan?> Finales)
        {
            decimal Horas = 0;
            for (int i = 0; i < Iniciales.Count; i++)
            {
                Horas += decimal.Parse((Finales[i] - Iniciales[i]).ToString().Split(':')[0]);
            }
            return Horas;
        }
        //TimeSpan x = new TimeSpan(9, 0, 0);
        //TimeSpan y = new TimeSpan(17, 0, 0);
        public string getDetalle(List<string> Detalle, List<double> Monto)
        {
            string Detalles = "";
            for (int i = 0; i < Detalle.Count; i++)
            {
                Detalles += Detalle[i] + ": $";
                if ((i + 1) != Detalle.Count)
                {
                    Detalles += Monto[i] + ", ";
                }
                else Detalles += Monto[i];
            }
            return Detalles;
        }
    }
}
