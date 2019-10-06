using BanBan.Controls;
using System.Windows;
using System.Windows.Controls;

namespace BanBan.Pages
{
    /// <summary>
    /// Interaction logic for Empleados.xaml
    /// </summary>
    public partial class Empleados : Page
    {
        //sb es una instancia del "contexto" de la base de datos (base de datos mapeada)
        private EmpleadosControl emp = new EmpleadosControl();
        public static bool edit = false;
        public Empleados()
        {
            InitializeComponent();
            empleadosLoad();
        }
        private void empleadosLoad()
        {
            dpFechaContrato.SelectedDate = System.DateTime.Now;

            lsSucursalesSupervisor.ItemsSource = emp.getSucursales();
            cbCargo.ItemsSource = emp.getCargos();
            cbSucursal.ItemsSource = emp.getSucursales();
            cbAfiliacion.ItemsSource = emp.getSistemaPensiones();

            cbCargo.SelectedIndex = 0;
            cbSucursal.SelectedIndex = 0;
            cbAfiliacion.SelectedIndex = 0;
        }

        private void btGuardarClick(object sender, RoutedEventArgs e)
        {
            if (!dpFechaContrato.SelectedDate.HasValue) { dpFechaContrato.IsDropDownOpen = true; return; }
            string val = emp.save(tbNombre.Text, tbApellido.Text, tbDUI.Text, tbNIT.Text,
                    dpFechaContrato.SelectedDate.Value, cbAfiliacion.Text, tbNumeroAfiliado.Text,
                    cbSucursal.Text, cbCargo.Text, tbSueldoBase.Text, tbTelefono.Text, cbxActivo.IsEnabled);
            var asd = lsSucursalesSupervisor.SelectedItems;
            if (val != "OK") MessageBox.Show("Advertencia: " + val,
                "Advertencia!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void btCancelarClick(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(emp.getIdSistemaPensiones(cbAfiliacion.Text).ToString());
        }

        private void cbCargoDropDownClosed(object sender, System.EventArgs e)
        {
            if (cbCargo.Text == "Supervisor") lsSucursalesSupervisor.IsEnabled = true;
            else
            {
                lsSucursalesSupervisor.IsEnabled = false;
                lsSucursalesSupervisor.SelectedIndex = -1;
            }
        }

        private void PageGotFocus(object sender, RoutedEventArgs e)
        {
            cbxActivo.IsEnabled = edit;
        }
    }
}
