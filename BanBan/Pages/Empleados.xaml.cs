using BanBan.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media;

namespace BanBan.Pages
{
    /// <summary>
    /// Interaction logic for Empleados.xaml
    /// </summary>
    public partial class Empleados : Page
    {
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
            lsAtenciones.ItemsSource = emp.getAtenciones();
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
                     dpFechaContrato.SelectedDate.Value, cbAfiliacion.Text, tbNumeroAfiliado.Text, tbISSS.Text,
                     cbSucursal.Text, cbCargo.Text, tbSueldoBase.Text, tbTelefono.Text, cbxActivo.IsChecked.Value,
                     getASupervisar(), getAtenciones());

            if (val != "OK") MessageBox.Show("Advertencia: " + val,
                "Advertencia!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void btCancelarClick(object sender, RoutedEventArgs e)
        {
            emp.ClearTextboxes(this);
        }

        private void cbCargoDropDownClosed(object sender, System.EventArgs e)
        {
            if (cbCargo.Text == "Supervisor") lsSucursalesSupervisor.IsEnabled = true;
            else { lsSucursalesSupervisor.IsEnabled = false; lsSucursalesSupervisor.SelectedIndex = -1; }
        }

        private void PageGotFocus(object sender, RoutedEventArgs e)
        {
            cbxActivo.IsEnabled = edit;
            dpSalidaEmpresa.IsEnabled = edit;
        }

        private List<string> getASupervisar()
        {
            List<string> ret = new List<string>();
            var asupervisar = lsSucursalesSupervisor.SelectedItems;
            foreach (var asu in asupervisar) ret.Add(asu.ToString());
            return ret;
        }
        private List<string> getAtenciones()
        {
            List<string> ret = new List<string>();
            var at = lsAtenciones.SelectedItems;
            foreach (var ate in at) ret.Add(ate.ToString());
            return ret;
        }
    }
}