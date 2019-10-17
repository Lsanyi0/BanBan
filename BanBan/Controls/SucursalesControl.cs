using BanBan.Model;
using System.Collections.Generic;
using System.Linq;

namespace BanBan.Controls
{
    class SucursalesControl : Utilidades
    {

        private sucursal suc;
        private trabajo tb;
        private string retorno = "";

        public SucursalesControl()
        {
            sb = new sBanBan();
            suc = new sucursal();
            tb = new trabajo();
        }
        public string GuardarSucursal(string sucursal, string direccion, string municipio, string supervisor, List<string> asuetos)
        {
            retorno = ValidarSucursal(sucursal, direccion);
            if (retorno.Equals("valido"))
            {
                var id = from cd in sb.ciudad where cd.ciudad1.Equals(municipio) select cd.idCiudad;
                var sup = from sp in sb.empleado where sp.nombre.Equals(supervisor) select sp.idEmpleado;
                suc.sucursal1 = "";
                suc.direccion = "";
                suc.idCiudad = int.Parse(id.ToString());
                sb.sucursal.Add(suc);
                sb.SaveChangesAsync();
                var sucu = from sc in sb.sucursal where sc.sucursal1.Equals(sucursal) select sc.idSucursal;
                tb.idEmpleado = int.Parse(sup.ToString());
                tb.idSucursal = int.Parse(sucu.ToString());
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
        public SucursalModel getSucursal(string sucursal)
        {
            int idSucursal = getIdSucursal(sucursal);
            SucursalModel ssuc = (from ssc in sb.sucursal
                                  join cd in sb.ciudad on ssc.idCiudad equals cd.idCiudad
                                  join dp in sb.departamento on cd.idCiudad equals dp.idDepartamento
                                  join asu in sb.diapatronal on cd.idCiudad equals asu.idCiudad
                                  join tr in sb.trabajo on ssc.idSucursal equals tr.idSucursal
                                  join emp in sb.empleado on tr.idEmpleado equals emp.idEmpleado
                                  where ssc.idSucursal.Equals(idSucursal)
                                  select new SucursalModel()
                                  {
                                      IdSucursal = ssc.idSucursal,
                                      NombreSucursal = ssc.sucursal1,
                                      IdDepartamento = dp.idDepartamento,
                                      Departamento = dp.departamento1,
                                      Municipio = cd.ciudad1,
                                      IdMunicipio = cd.idCiudad,
                                      Direccion = ssc.direccion,
                                      DiasAsueto = (from diap in sb.diapatronal where diap.idCiudad.Equals(cd.idCiudad) select diap.dia).ToList(),
                                      IdEmpleado = tr.idEmpleado,
                                      NombreEmpleado = emp.nombre
                                  }).FirstOrDefault();
            return ssuc;
        }
    }
}
