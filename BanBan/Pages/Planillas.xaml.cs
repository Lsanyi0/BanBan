using BanBan.Controls;
using System.Windows.Controls;
using System.Windows;

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

            lbNumero.Content = dgvPlanilla.Items.Count;
        }

        private void cbSucursalKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                cbSucursalDropDownClosed(sender, e);
            }
        }

        private void cbSucursalDropDownClosed(object sender, System.EventArgs e)
        {
            if (!cbSucursal.Items.Contains(cbSucursal.Text)) return;
            if (!string.IsNullOrWhiteSpace(cbSucursal.Text)) dgvPlanilla.ItemsSource = pc.getEmpleados(cbSucursal.Text);
            else dgvPlanilla.ItemsSource = pc.getEmpleados();
            dgvAtenciones.ItemsSource = dgvPlanilla.Items;
            lbNumero.Content = dgvPlanilla.Items.Count;
        }
    }
}