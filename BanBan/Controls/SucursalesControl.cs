using System.Collections.Generic;
using System.Linq;

namespace BanBan.Controls
{
    class SucursalesControl
    {

        private sBanBan sb;
        private sucursal sc;
        private trabajo tb;
        private string retorno = "";

        public SucursalesControl()
        {
            sb = new sBanBan();
            sc = new sucursal();
            tb = new trabajo();
        }
        public string GuardarSucursal(string sucursal, string direccion, string municipio, string supervisor, List<string> asuetos)
        {
            retorno = ValidarSucursal(sucursal, direccion);
            if (retorno.Equals("valido"))
            {
                var id = from cd in sb.ciudad where cd.ciudad1.Equals(municipio) select cd.idCiudad;
                var sup = from sp in sb.empleado where sp.nombre.Equals(supervisor) select sp.idEmpleado;
                sc.sucursal1 = "";
                sc.direccion = "";
                sc.idCiudad = int.Parse(id.ToString());
                sb.sucursal.Add(sc);
                sb.SaveChangesAsync();
                var suc = from sc in sb.sucursal where sc.sucursal1.Equals(sucursal) select sc.idSucursal;
                tb.idEmpleado = int.Parse(sup.ToString());
                tb.idSucursal = int.Parse(suc.ToString());
                sb.trabajo.Add(tb);


                return "OK";
            }
            else
            {
                return retorno;
            }

        }

        public string ValidarSucursal(string sucursal, string direccion)
        {
            var verificar = from sc in sb.sucursal where sc.sucursal1.Equals(sucursal) select sc.idSucursal;
            if (verificar != null)
            {
                return "sucursal existente";
            }
            else
            {
                if (direccion == null)
                {
                    return "dirección no valida";
                }
                else
                {
                    return "valido";
                }
            }

        }

        public List<string> DeterminarAsuetos(string asuetos)
        {
            List<string> asueto = new List<string>();
            var asu = from at1 in sb.diapatronal
                      join mun in sb.ciudad
                       on at1.idCiudad equals mun.idCiudad
                      where mun.ciudad1 == asuetos
                      select at1.dia;
            if (asu == null)
            {
                return asueto;
            }
            else
            {

                foreach (var list in asu)
                {
                    asueto.Add(list.ToString());
                }

            }
            return asueto;
        }

    }
}
