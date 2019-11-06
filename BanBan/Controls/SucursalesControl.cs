using BanBan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace BanBan.Controls
{
    class SucursalesControl : Utilidades
    {

        private sucursal suc;
        private trabajo tb;
        private ciudad cd;
        private diapatronal dp;
        private string retorno, var = "";

        public SucursalesControl()
        {
            sb = new sBanBan();
            suc = new sucursal();
            tb = new trabajo();
            cd = new ciudad();
        }
        public string GuardarSucursal(string sucursal, string direccion, string municipio, string supervisor, string departamento, List<string> asuetos)
        {
            retorno = ValidarSucursal(sucursal, direccion);

            if (retorno.Equals("valido"))
            {
                retorno = ValidarCiudad(municipio, departamento, asuetos);
                if (retorno.Equals("Existe"))
                {
                    var id = from cd in sb.ciudad where cd.ciudad1==municipio select cd.idCiudad;
                    var sup = from sp in sb.empleado where sp.nombre==supervisor select sp.idEmpleado;
                    suc.sucursal1 = sucursal;
                    suc.direccion = direccion;
                    suc.idHangar = 1;
                    foreach (var item in id)
                    {
                        suc.idCiudad = int.Parse(item.ToString());
                    }
                    sb.sucursal.Add(suc);
                    
                    var sucu = from sc in sb.sucursal where sc.sucursal1.Equals(sucursal) select sc.idSucursal;
                    foreach (var item in sup)
                    {
                        tb.idEmpleado = int.Parse(item.ToString());
                    }
                    foreach (var item in sucu)
                    {
                        tb.idSucursal = int.Parse(item.ToString());
                    }

                    sb.trabajo.Add(tb);
                    sb.SaveChangesAsync();
                }
                else
                {
                    MessageBox.Show("La ciudad fue agregada a la base de datos");
                    var id = from cd in sb.ciudad where cd.ciudad1 == municipio select cd.idCiudad;
                    var sup = from sp in sb.empleado where sp.nombre.Equals(supervisor) select sp.idEmpleado;
                    suc.sucursal1 = sucursal;
                    suc.direccion = direccion;
                    suc.idHangar = 1;
                    foreach (var item in id)
                    {
                        suc.idCiudad = int.Parse(item.ToString());
                    }
                    sb.sucursal.Add(suc);

                    var sucu = from sc in sb.sucursal where sc.sucursal1.Equals(sucursal) select sc.idSucursal;
                    foreach (var item in sup)
                    {
                        tb.idEmpleado = int.Parse(item.ToString());
                    }
                    foreach (var item in sucu)
                    {
                        tb.idSucursal = int.Parse(item.ToString());
                    }

                    sb.trabajo.Add(tb);
                    sb.SaveChangesAsync();
                }

                return "OK";
            }
            else
            {
                return retorno;
            }

        }

        public string ValidarCiudad(string ciudad, string departamento, List<string> asuetos)
        {
            var id = from cd in sb.ciudad where cd.ciudad1.Equals(ciudad) select cd.idCiudad;
            var idD = from dep in sb.departamento where dep.departamento1.Equals(departamento) select dep.idDepartamento;
            
            if (id==null)
            {
                cd.ciudad1 = ciudad;
                cd.idDepartamento = int.Parse(idD.ToString());
                sb.ciudad.Add(cd);
                //Asuetos(asuetos, int.Parse(id.ToString()));
                sb.SaveChangesAsync();
                
            }
            else
            {
                //Asuetos(asuetos, int.Parse(id.ToString()));
                foreach (var item in asuetos)
                {
                    var asut = from asu in sb.diapatronal
                               join cd in sb.ciudad on asu.idCiudad equals cd.idCiudad
                               where asu.dia == Convert.ToDateTime(item) && asu.idCiudad == int.Parse(id.ToString())
                    select asu.idDiaPatronal;
                    if (asut != null)
                    {

                    }
                    else
                    {
                        dp = new diapatronal();
                        dp.dia = Convert.ToDateTime(item);
                        dp.idCiudad = int.Parse(id.ToString());
                        sb.diapatronal.Add(dp);
                    }

                }
                sb.SaveChangesAsync();
                var ="Existe";
            }

            return var;
        }

        public void Asuetos(List<string> asuetos, int id)
        {
            foreach (var item in asuetos)
            {
                var asut = from asu in sb.diapatronal
                           join cd in sb.ciudad on asu.idCiudad equals cd.idCiudad
                           where asu.dia == Convert.ToDateTime(item) && asu.idCiudad == id
                           select asu.idDiaPatronal;
                if (asut != null)
                {

                }
                else
                {
                    dp = new diapatronal();
                    dp.dia = Convert.ToDateTime(item);
                    dp.idCiudad = int.Parse(id.ToString());
                    sb.diapatronal.Add(dp);
                }

            }
        }
        public string ValidarSucursal(string sucursal, string direccion)
        {
            int c=0;
            var verificar = from sc in sb.sucursal where sc.sucursal1.Equals(sucursal) select sc.idSucursal;
            foreach (var item in verificar)
            {
                c++;
            }
            if (c > 0)
            {
                return "sucursal existente";
            }
            else
            {
                if (direccion == null)
                {
                    direccion = "";
                    return "valido";
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
                                  }).FirstOrDefault() ?? new SucursalModel();
            ssuc.DiasAsueto ??= new List<DateTime>(); 
            return ssuc;
        }
    }
}
