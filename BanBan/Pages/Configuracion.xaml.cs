using System.Windows.Controls;
using BanBan.Controls;

namespace BanBan.Pages
{
    /// <summary>
    /// Interaction logic for Configuracion.xaml
    /// </summary>
    public partial class Configuracion : Page
    {

        ConfiguracionControl cc = new ConfiguracionControl();

        public Configuracion()
        {
            InitializeComponent();



            if (MainWindow.tiposUsuario[0] == LoginControl.tipoUsuario)
            {
                CargarAdministrador();
            }
            else if (MainWindow.tiposUsuario[2] == LoginControl.tipoUsuario)
            {
                CargarIT();
            }
            else
            {
                return;
            }

        }
        private void CargarAdministrador()
        {
            gdAdministrador.Visibility = System.Windows.Visibility.Visible;
            gdIT.Visibility = System.Windows.Visibility.Hidden;
            btGuardarRoot.Visibility = System.Windows.Visibility.Hidden;

            cbCargo.ItemsSource = cc.getCargos();
            cbAtenciones.ItemsSource = cc.getAtenciones();
            cbHoraExtra.ItemsSource = cc.getTipoHora();

            cbCargo.SelectedIndex = 0;
            cbAtenciones.SelectedIndex = 0;
            cbHoraExtra.SelectedIndex = 0;
        }
        private void CargarIT()
        {
            gdAdministrador.Visibility = System.Windows.Visibility.Hidden;
            gdIT.Visibility = System.Windows.Visibility.Visible;
            btGuardarAdmin.Visibility = System.Windows.Visibility.Hidden;

            cbSucursalDispositivo.ItemsSource = cc.getSucursales();
            cbEmpleado.ItemsSource = cc.getEmpleado();
            cbUsuario.ItemsSource = cc.getUsuario();

            cbSucursalDispositivo.SelectedIndex = 0;
            cbEmpleado.SelectedIndex = 0;
            cbUsuario.SelectedIndex = 0;
        }

        private void btGuardarAdmin_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            cc.updateCargo(cbCargo.Text, decimal.Parse(tbMontoCargo.Text));
        }
    }
}