using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BanBan.Pages
{
    /// <summary>
    /// Interaction logic for Configuracion.xaml
    /// </summary>
    public partial class Configuracion : Page
    {

        sBanBan sb = new sBanBan();

        public Configuracion()
        {
            InitializeComponent();

            var carg = from cr in sb.Cargo
                       select cr.cargo1;
            var aten = from at in sb.Atencion
                       select at.atencion1;
            var hre = from he in sb.tipoHora
                      select he.tipo;

            if (carg != null)
            {
                cbCargo.ItemsSource = carg.ToList();
                cbCargo.SelectedIndex = 0;
            }
            if (aten != null)
            {
                cbAtenciones.ItemsSource = aten.ToList();
                cbAtenciones.SelectedIndex = 0;
            }
            if (hre != null)
            {
                cbHoraExtra.ItemsSource = hre.ToList();
                cbHoraExtra.SelectedIndex = 0;
            }

        }
    }
}
