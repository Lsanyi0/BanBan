using BanBan.Controls;
using System.Windows.Controls;
using System.Windows;
using BanBan.Model;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace BanBan.Pages
{
    /// <summary>
    /// Interaction logic for Planillas.xaml
    /// </summary>
    public partial class Planillas : Page
    {
        private PlanillasControl pc = new PlanillasControl();
        private BindingList<PlanillaModel> pm;
        private const string file = "planilla.xml";

        public Planillas()
        {
            InitializeComponent();

            cbSucursal.ItemsSource = pc.getSucursales();
            cbSucursal.SelectedIndex = 0;

            pm = File.Exists(file) ? pc.CargarXML(file) : pc.getEmpleados();

            pm.ListChanged += Actualizar;

            dgvEditar.ItemsSource = pm;

            ActualizarModelo();

            lbNumero.Content = dgvPlanilla.Items.Count;
        }


        private void Actualizar(object sender, ListChangedEventArgs e)
        {
            ActualizarModelo();
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
            if (!string.IsNullOrWhiteSpace(cbSucursal.Text)) pm = pc.getEmpleados(cbSucursal.Text);
            else pm = pc.getEmpleados();
            ActualizarModelo();
            lbNumero.Content = pm.Count;
        }

        private void miEditarEmpleadoClick(object sender, RoutedEventArgs e)
        {
            Empleados.idEdit = ((PlanillaModel)dgvPlanilla.SelectedItems[0]).IdEmpleado;
            Empleados.cargarEdit = true;
        }

        private void ActualizarModelo()
        {
            dgvPlanilla.ItemsSource = pm;
            dgvAtenciones.ItemsSource = pm;
            dgvEditar.ItemsSource = pm;
        }

        private void btGuardar_Click(object sender, RoutedEventArgs e)
        {

            if (pm != null)
            {
                XmlSerializer xml = new XmlSerializer(typeof(BindingList<PlanillaModel>));
                using (StreamWriter sw = new StreamWriter(file))
                {
                    using (XmlWriter writer = XmlWriter.Create(sw))
                    {
                        xml.Serialize(writer, pm);
                    }
                }
            }
        }

        private void btCerrarPlanilla_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btObtenerDatos_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btDescartarCambios_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(file)) File.Delete(file);

            pm = pc.getEmpleados();

            dgvEditar.ItemsSource = pm;

            ActualizarModelo();
        }

        private void tbBuscar_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (tbBuscar.Text.Length>3)
            {
                
            }
        }
    }
}