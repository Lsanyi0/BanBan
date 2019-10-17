using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanBan.Controls
{
    class ConfiguracionControl : Utilidades
    {
        public List<string> getCargos()
        {
            var carg = from cr in sb.cargo
                       select cr.cargo1;
            if (carg != null)
            {
                return carg.ToList();
            }
            return lv;
        }
        public List<string> getAtenciones()
        {
            var aten = from at in sb.atencion
                       select at.atencion1;
            if (aten != null)
            {
                return aten.ToList();
            }
            return lv;
        }
        public List<string> getTipoHora()
        {
            var hre = from he in sb.tipohora
                      select he.tipo;
            if (hre != null)
            {
                return hre.ToList();
            }
            return lv;
        }
        public List<string> getEmpleado()
        {
            var emp = from em in sb.empleado
                      select em.nombre;
            if (emp != null)
            {
                return emp.ToList();
            }
            return lv;
        }
        public List<string> getUsuario()
        {
            var usu = from us in sb.usuario
                      select us.usuario1;
            if (usu != null)
            {
                return usu.ToList();
            }
            return lv;
        }

        public void updateCargo(string cargo, decimal monto)
        {
            var carg = (from cg in sb.cargo where cg.cargo1 == cargo select cg).FirstOrDefault();
            carg.atenciones = monto;
            sb.SaveChanges();
        }
    }
}