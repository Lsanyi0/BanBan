using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;

namespace BanBan.Pages
{
    /// <summary>
    /// Interaction logic for Empleados.xaml
    /// </summary>
    public partial class Empleados : Page
    {
        //sb es una instancia del "contexto" de la base de datos (base de datos mapeada)
        sBanBan sb = new sBanBan();
        public Empleados()
        {
            InitializeComponent();
            //var es generica pero parece no causar problemas...
            var sp = from sisp in sb.SistemaPension select sisp.sistemaP;
            var sc = from suc in sb.Sucursal select suc.sucursal1;
            var cr = from car in sb.Cargo select car.cargo1;
            if (sp != null)
            {
                cbAfiliacion.ItemsSource = sp.ToList();
                cbAfiliacion.SelectedIndex = 0;
            }
            if (sc != null)
            {
                cbSucursal.ItemsSource = sc.ToList();
                cbSucursal.SelectedIndex = 0;
            }
            if (cr != null) 
            {
                cbCargo.ItemsSource = cr.ToList();
                cbCargo.SelectedIndex = 0;
            }
        }
    }
}
