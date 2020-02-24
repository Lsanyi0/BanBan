using BanBan.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace BanBan.Controls
{
    public class PlanillasControl : Utilidades
    {
        private const int HorasQuincena = 90;
        //private List<PlanillaModel> PlanillaModelPpal;
        public BindingList<PlanillaModel> getEmpleados()
        {
            List<empleado> empleados = (from em in sb.empleado.Include("sistemapension") select em).ToList();
            return getPlanillaModels(empleados);
        }
        public List<empleado> ObtenerEmpleados()
        {
            return (from em in sb.empleado select em).ToList();
        }
        public bool EmpleadoInSucursal(int idempleado, int idsucursal)
        {
            return sb.trabajo.Where(x => x.idSucursal == idsucursal && x.idEmpleado == x.idEmpleado).Any();
        }
        public int GetIdSucursalByNombre(string sucursal)
        {
            return sb.sucursal.Where(x => x.sucursal1.Contains(sucursal)).Select(x => x.idSucursal).FirstOrDefault();
        }
        public BindingList<PlanillaModel> getEmpleados(string sucursal)
        {
            //string xD = "Select e.nombre, s.sucursal " +
            //    "from empleado e join trabajo t on e.idEmpleado = t.idEmpleado  " +
            //    "join sucursal s on t.idSucursal = s.idSucursal where e.idCargo = 2;";
            int idSuc = getIdSucursal(sucursal);

            //List<empleado> empleado = (from em in sb.empleado
            //                           join tr in sb.trabajo on em.idEmpleado equals tr.idEmpleado
            //                           join sc in sb.sucursal on tr.idSucursal equals sc.idSucursal
            //                           where em.idCargo == 2
            //                           select em).ToList();

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
                SueldoBase = 10.1390m,
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
                SueldoBase = 10.1390M,
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
                SueldoBase = 10.1390M,
                HorasNocturnas = 5,
                HorasExtra = 37,
                HorasAsueto = 8,
                AFPEmpleado = 7.25M
            });
            return pm;
        }
        public BindingList<PlanillaModel> getPlanillaModels(List<empleado> empleados)
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
                    List<DateTime?> Inicio = empleado.planillahorario.Select(x => x.entrada).ToList();
                    List<DateTime?> Fin = empleado.planillahorario.Select(x => x.salida).ToList();
                    pm.Horas = GetHorasTrabajadasMax(Inicio, Fin);
                    pm.NumeroDias = GetDiasTrabajados(Inicio);
                    pm.HorasNocturnas = GetNocturnidad(Fin);
                    pm.HorasAusencia = GetHorasAusente(Inicio, Fin);

                    List<decimal> horasExtra = GetTiposDeHora(empleado);
                    try
                    {
                        pm.HorasExtra = horasExtra[0];
                        pm.HorasNocturnasExtra = horasExtra[1];
                        pm.HorasAsueto = horasExtra[2];
                        pm.HorasDescanso = horasExtra[3];
                    }
                    catch (Exception)
                    {
                    }
                    pm.Atencion = (from at in sb.atencion join atd in sb.atenciondetalle on at.idAtencion equals atd.idAtencion where atd.idEmpleado == empleado.idEmpleado select at.montoBase).ToList();
                    List<string> asd = (from at in sb.atencion join atd in sb.atenciondetalle on at.idAtencion equals atd.idAtencion where atd.idEmpleado == empleado.idEmpleado select at.atencion1).ToList();
                    pm.Atenciones = GetDetalle(asd, pm.Atencion);
                    //pm.Descuento = GetDetalle();
                    planillaModels.Add(pm);
                }
                return planillaModels;
            }
            return new BindingList<PlanillaModel>();
        }
        private int GetDiasTrabajados(List<DateTime?> Dias)
        {
            return Dias.Count;
        }
        public decimal GetHorasTrabajadasMax(List<DateTime?> Iniciales, List<DateTime?> Finales)
        {
            decimal Horas = 0;
            for (int i = 0; i < Iniciales.Count; i++)
            {
                Horas += (decimal)Math.Abs((Finales[i] - Iniciales[i]).Value.TotalHours);
                if (Horas >= HorasQuincena)
                {
                    return HorasQuincena;
                }
            }
            return Horas;
        }
        public decimal GetHorasTrabajadas(List<DateTime?> Iniciales, List<DateTime?> Finales)
        {
            decimal Horas = 0;
            for (int i = 0; i < Iniciales.Count; i++)
            {
                Horas += (decimal)Math.Abs((Finales[i] - Iniciales[i]).Value.TotalHours);
            }
            return Horas;
        }
        public decimal GetHorasTrabajadas(List<DateTime> Iniciales, List<DateTime> Finales)
        {
            decimal Horas = 0;
            for (int i = 0; i < Iniciales.Count; i++)
            {
                Horas += (decimal)Math.Abs((Finales[i] - Iniciales[i]).TotalHours);
            }
            return Horas;
        }
        private List<decimal> GetTiposDeHora(empleado emp)
        {
            List<decimal> Horas = new List<decimal>();
            try
            {
                List<DateTime> Inicio = emp.horarioextra.Where(x => x.tipohora.idTipoHora == 2).Select(x => x.horaInicio).ToList() ?? new List<DateTime>();
                List<DateTime> Fin = emp.horarioextra.Where(x => x.tipohora.idTipoHora == 2).Select(x => x.horaFinal).ToList() ?? new List<DateTime>();
                Horas.Add(GetHorasTrabajadas(Inicio, Fin));

                Inicio = emp.horarioextra.Where(x => x.tipohora.idTipoHora == 3).Select(x => x.horaInicio).ToList() ?? new List<DateTime>();
                Fin = emp.horarioextra.Where(x => x.tipohora.idTipoHora == 3).Select(x => x.horaFinal).ToList() ?? new List<DateTime>();
                Horas.Add(GetHorasTrabajadas(Inicio, Fin));

                Inicio = emp.horarioextra.Where(x => x.tipohora.idTipoHora == 4).Select(x => x.horaInicio).ToList() ?? new List<DateTime>();
                Fin = emp.horarioextra.Where(x => x.tipohora.idTipoHora == 4).Select(x => x.horaFinal).ToList() ?? new List<DateTime>();
                Horas.Add(GetHorasTrabajadas(Inicio, Fin));

                Inicio = emp.horarioextra.Where(x => x.tipohora.idTipoHora == 5).Select(x => x.horaInicio).ToList() ?? new List<DateTime>();
                Fin = emp.horarioextra.Where(x => x.tipohora.idTipoHora == 5).Select(x => x.horaFinal).ToList() ?? new List<DateTime>();
                Horas.Add(GetHorasTrabajadas(Inicio, Fin));
            }
            catch (Exception)
            {
            }
            return Horas;
        }
        //Horas > 7:00 pm
        private decimal GetNocturnidad(List<DateTime?> Finales)
        {
            TimeSpan HoraNocturna = DateTime.Parse("7:00 pm").TimeOfDay;
            decimal Horas = 0;
            foreach (DateTime fin in Finales)
            {
                if (fin.TimeOfDay > HoraNocturna)
                {
                    Horas += (fin.TimeOfDay - HoraNocturna).Hours;
                }
            }
            return Horas;
        }
        private decimal GetHorasAusente(List<DateTime?> Iniciales, List<DateTime?> Finales)
        {
            decimal Horas = GetHorasTrabajadas(Iniciales, Finales);
            if (Horas < HorasQuincena)
            {
                return HorasQuincena - Horas;
            }
            return 0M;
        }
        //Funcion par obtener detalle de descuentos o atenciones
        public string GetDetalle(List<string> Detalle, List<decimal> Monto)
        {
            string Detalles = "";
            for (int i = 0; i < Detalle.Count; i++)
            {
                Detalles += Detalle[i] + ": $";
                if ((i + 1) != Detalle.Count)
                {
                    Detalles += Monto[i].ToString("0.##") + ", ";
                }
                else Detalles += Monto[i].ToString("0.##");
            }
            return Detalles;
        }
        public BindingList<PlanillaModel> CargarXML(string file)
        {
            XmlSerializer xml = new XmlSerializer(typeof(BindingList<PlanillaModel>));
            BindingList<PlanillaModel> planillaXML;
            using (FileStream fileStream = new FileStream(file, FileMode.Open))
            {
                planillaXML = (BindingList<PlanillaModel>)xml.Deserialize(fileStream);
            }
            return planillaXML;
        }

    }
}
