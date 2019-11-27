using BanBan.Model;
using System.Collections.Generic;
using System.Linq;

namespace BanBan.Controls
{
    public class HorasExtraCommonControl
    {
        private protected HorasExtraOfflineModel sc;
        private protected sBanBan sb = new sBanBan();
        private protected List<Empleado> GetEmpleados()
        {
            return (from em in sb.empleado
                    select new Empleado
                    {
                        idEmpleado = em.idEmpleado,
                        nombre = em.nombre,
                        apellido = em.apellido,
                        estado = em.estado,
                        idCargo = em.idCargo
                    }).ToList() ?? null;
        }
        private protected List<Sucursal> GetSucursales()
        {
            return (from sc in sb.sucursal
                    select new Sucursal
                    {
                        idSucursal = sc.idSucursal,
                        sucursal1 = sc.sucursal1,
                    }).ToList() ?? null;
        }
        private protected List<TipoHora> GetTipoHoras()
        {
            return (from th in sb.tipohora
                    select new TipoHora
                    {
                        idTipoHora = th.idTipoHora,
                        tipo = th.tipo,
                    }).ToList() ?? null;
        }
        private protected List<Usuario> GetUsuarios()
        {
            return (from us in sb.usuario
                    select new Usuario
                    {
                        idUsuario = us.idUsuario,
                        usuario = us.usuario1,
                        contrasena = us.contrasena,
                        idEmpleado = us.idEmpleado,
                        idTipo = us.idTipo,
                        reseteo = us.reseteo
                    }).ToList() ?? null;
        }
        private protected List<Trabajo> GetTrabajos()
        {
            return (from tb in sb.trabajo
                    select new Trabajo
                    {
                        idTrabajo = tb.idTrabajo,
                        idEmpleado = tb.idEmpleado,
                        idSucursal = tb.idEmpleado
                    }).ToList() ?? null;
        }
    }
}
