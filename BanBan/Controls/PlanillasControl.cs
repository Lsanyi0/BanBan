using BanBan.Model;
using System.Collections.Generic;
using System.Linq;

namespace BanBan.Controls
{
    class PlanillasControl : Utilidades
    {
        public List<PlanillaModel> getEmpleados()
        {
            List<Empleado> empleados = (from em in sb.Empleado select em).ToList();
            return getPlanillaModels(empleados);
        }
        public List<PlanillaModel> getEmpleados(string sucursal)
        {
            int idSuc = getIdSucursal(sucursal);
            List<Empleado> empleados = (from em in sb.Empleado
                                        join tb in sb.Trabajo on em.idEmpleado equals tb.idEmpleado
                                        join sc in sb.Sucursal on tb.idSucursal equals sc.idSucursal
                                        where sc.idSucursal == idSuc && em.idCargo != 2
                                        select em
                                        ).ToList();
            return getPlanillaModels(empleados);
        }
        private List<PlanillaModel> getPlanillaModels(List<Empleado> empleados)
        {
            if (empleados != null)
            {
                List<PlanillaModel> planillaModels = new List<PlanillaModel>();
                foreach (Empleado empleado in empleados)
                {
                    PlanillaModel pm = new PlanillaModel()
                    {
                        NombreCompleto = empleado.nombre + " " + empleado.apellido
                    };
                    planillaModels.Add(pm);
                }
                return planillaModels;
            }
            return new List<PlanillaModel>();
        }
    }
}
