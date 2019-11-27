using BanBan.Model;
using System.ComponentModel;

namespace BanBan.Controls
{
    public class HorasExtraControl : HorasExtraCommonControl
    {
        public HorasExtraControl(HorasExtraOfflineModel heom)
        {
            sc = heom;
        }
        public HorasExtraControl()
        {
            sc = new HorasExtraOfflineModel();
            sc.Empleados = GetEmpleados();
            sc.Sucursales = GetSucursales();
            sc.TipoHoras = GetTipoHoras();
            sc.Usuarios = GetUsuarios();
            sc.Trabajos = GetTrabajos();
        }
        public BindingList<string> GetCBEmplados()
        {
            BindingList<string> CBEmplados = new BindingList<string>();
            foreach (var em in sc.Empleados)
            {
                CBEmplados.Add(em.nombre + " " + em.apellido);
            }
            return CBEmplados;
        }
    }
}
