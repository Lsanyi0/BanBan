using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace BanBan.Controls
{
    //Clase solo para funciones comunes para varios formularios
    class Utilidades
    {
        //sb es una instancia del "contexto" de la base de datos (base de datos mapeada)
        //Instancia general para todos los formularios
        private protected static sBanBan sb = new sBanBan();

        private protected readonly IQueryable<sucursal> sc;
        //Lista vacia para retornar cuando no encuentra resultados en una consulta
        private protected readonly List<string> lv = new List<string>();
        public Utilidades()
        {
            sc = from suc in sb.sucursal select suc;
        }
        public List<string> getSucursales()
        {
            if (sc != null) return (from suc in sc select suc.sucursal1).ToList();
            else return lv;
        }
        private protected int getIdSucursal(string sucursal)
        {
            return (from suc in sc where suc.sucursal1.Equals(sucursal) select suc.idSucursal).First();
        }
        public List<string> buscarSucursales(string sucursal)
        {
            List<string> sucursales = (from suc in sc where suc.sucursal1.Contains(sucursal) select suc.sucursal1).ToList();
            if (sucursales != null) return sucursales;
            else return (from suc in sc select suc.sucursal1).ToList();
        }
        //Para limpiar todos los textbox en una page o elemento "padre"
        public void ClearTextboxes(Visual visual)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
            {
                Visual ctrl = (Visual)VisualTreeHelper.GetChild(visual, i);
                if (ctrl.GetType() == typeof(TextBox))
                {
                    TextBox tb = (TextBox)ctrl;
                    tb.Clear();
                }
                ClearTextboxes(ctrl);
            }
        }
    }
}