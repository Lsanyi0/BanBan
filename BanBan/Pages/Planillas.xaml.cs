using BanBan.Controls;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;

namespace BanBan.Pages
{
    /// <summary>
    /// Interaction logic for Planillas.xaml
    /// </summary>
    public partial class Planillas : Page
    {
        PlanillasControl pc = new PlanillasControl();
        public Planillas()
        {
            InitializeComponent();

            cbSucursal.ItemsSource = pc.getSucursales();
            cbSucursal.SelectedIndex = 0;

            dgvPlanilla.ItemsSource = pc.getEmpleados();
            dgvAtenciones.ItemsSource = pc.getEmpleados();
        }
        private void CbSucursalLostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cbSucursal.Text)) dgvPlanilla.ItemsSource = pc.getEmpleados(cbSucursal.Text);
            else dgvPlanilla.ItemsSource = pc.getEmpleados();
            dgvAtenciones.ItemsSource = dgvPlanilla.Items;
        }
    }
}
