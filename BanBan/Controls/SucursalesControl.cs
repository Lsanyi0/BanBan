﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanBan.Controls
{
    class SucursalesControl
    {

        private sBanBan sb;
        private Sucursal sc;
        private string retorno="";

        public SucursalesControl()
        {
            sb = new sBanBan();
            sc = new Sucursal();
        }
        public string GuardarSucursal(string sucursal, string direccion,string municipio, string supervisor, List<string> asuetos)
        {
            retorno = ValidarSucursal(sucursal, direccion);
            if (retorno.Equals("valido"))
            {
                var id = from cd in sb.Ciudad where cd.ciudad1.Equals(municipio) select cd.idCiudad;
                
                sc.sucursal1 = "";
                sc.direccion = "";
                sc.idCiudad = int.Parse(id.ToString());


                return "OK";
            }
            else
            {
                return retorno;
            }

        }

        public string ValidarSucursal(string sucursal, string direccion)
        {
            var verificar = from sc in sb.Sucursal where sc.sucursal1.Equals(sucursal) select sc.sucursal1;
            if (verificar!=null)
            {
                return "sucursal existente";
            }
            else 
            {
                if (direccion==null)
                {
                    return "dirección no valida";
                }
                else
                {
                    return "valido";
                }
            }

        }

    }
}