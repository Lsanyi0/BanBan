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
            var suc = from sc in sb.Sucursal
                      select sc.sucursal1;
            var emp = from em in sb.Empleado
                      select em.nombre;
            var usu = from us in sb.Usuario
                      select us.usuario1;

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
            if (suc != null)
            {
                cbSucursalDispositivo.ItemsSource = suc.ToList();
                cbSucursalDispositivo.SelectedIndex = 0;
            }
            if (emp != null)
            {
                cbEmpleado.ItemsSource = emp.ToList();
                cbEmpleado.SelectedIndex = 0;
            }
            if (usu != null)
            {
                cbUsuario.ItemsSource = usu.ToList();
                cbUsuario.SelectedIndex = 0;
            }
        }
    }
}
