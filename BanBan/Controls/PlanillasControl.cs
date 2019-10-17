using BanBan.Model;
using System.Collections.Generic;
using System.Linq;

namespace BanBan.Controls
{
    class PlanillasControl : Utilidades
    {
        public List<PlanillaModel> getEmpleados()
        {
            List<empleado> empleados = (from em in sb.empleado select em).ToList();
            return getPlanillaModels(empleados);
        }
        public List<PlanillaModel> getEmpleados(string sucursal)
        {
            int idSuc = getIdSucursal(sucursal);
            List<empleado> empleados = (from em in sb.empleado
                                        join tb in sb.trabajo on em.idEmpleado equals tb.idEmpleado
                                        join sc in sb.sucursal on tb.idSucursal equals sc.idSucursal
                                        where sc.idSucursal == idSuc && em.idCargo != 2
                                        select em
                                        ).ToList();
            return getPlanillaModels(empleados);
        }
        private List<PlanillaModel> getPlanillaModels(List<empleado> empleados)
        {
            if (empleados != null)
            {
                List<PlanillaModel> planillaModels = new List<PlanillaModel>();
                foreach (empleado empleado in empleados)
                {
                    PlanillaModel pm = new PlanillaModel()
                    {
                        IdEmpleado = empleado.idEmpleado,
                        Nombre = empleado.nombre,
                        Apellido = empleado.apellido

                    };
                    planillaModels.Add(pm);
                }
                return planillaModels;
            }
            return new List<PlanillaModel>();
        }
    }
}
