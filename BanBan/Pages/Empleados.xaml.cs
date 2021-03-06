﻿using BanBan.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media;
using System.Threading.Tasks;
using BanBan.Model;

namespace BanBan.Pages
{
    /// <summary>
    /// Interaction logic for Empleados.xaml
    /// </summary>
    public partial class Empleados : Page
    {
        private EmpleadosControl emp = new EmpleadosControl();
        public static bool cargarEdit = false;
        public static bool edit = false;
        private bool editState = false;
        public static int idEdit = -1;
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
            cbEditarEmpleado.ItemsSource = emp.getEmpleados();

            cbCargo.SelectedIndex = 0;
            cbSucursal.SelectedIndex = 0;
            cbAfiliacion.SelectedIndex = 0;
            cbEditarEmpleado.SelectedIndex = 0;
        }

        private void btGuardarClick(object sender, RoutedEventArgs e)
        {
            if (!dpFechaContrato.SelectedDate.HasValue) { dpFechaContrato.IsDropDownOpen = true; return; }

            string val = emp.save(tbNombre.Text.Trim(), tbApellido.Text.Trim(), tbDUI.Text, tbNIT.Text,
                     dpFechaContrato.SelectedDate.Value, dpSalidaEmpresa.SelectedDate, cbAfiliacion.Text,
                     tbNumeroAfiliado.Text, tbISSS.Text, cbSucursal.Text, cbCargo.Text, tbSueldoBase.Text,
                     tbTelefono.Text, cbxActivo.IsChecked.Value, getASupervisar(), getAtenciones(), editState);

            if (val != "OK") MessageBox.Show("Advertencia: " + val,
                "Advertencia!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else _ = editState ? MessageBox.Show("Registro modificado correctamente", "OK", MessageBoxButton.OK, MessageBoxImage.Information)
                               : MessageBox.Show("Registro guardado correctamente", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
            empleadosLoad();
        }

        private void btCancelarClick(object sender, RoutedEventArgs e)
        {
            emp.ClearTextboxes(this);
        }

        private void PageGotFocus(object sender, RoutedEventArgs e)
        {
            if (edit)
            {
                emp.ClearTextboxes(this);
                cbxActivo.IsEnabled = true;
                dpSalidaEmpresa.IsEnabled = true;
                var em = emp.getEmpleado(idEdit);
                tbNombre.Text = em.Nombre;
                tbApellido.Text = em.Apellido;
                tbDUI.Text = em.DUI;
                tbNIT.Text = em.NIT;
                dpFechaContrato.SelectedDate = em.FechaContrato;
                dpSalidaEmpresa.SelectedDate = em.FechaDespido;
                cbAfiliacion.SelectedItem = em.AfiliadoA;
                tbNumeroAfiliado.Text = em.NumeroAfiliado;
                tbISSS.Text = em.ISSS;
                cbCargo.SelectedItem = em.Cargo;
                cbSucursal.SelectedItem = em.Sucursales[0];
                foreach (var tel in em.Telefonos) tbTelefono.Text += tel + ", ";
                tbTelefono.Text = tbTelefono.Text.Length > 0 ? tbTelefono.Text.Substring(0, tbTelefono.Text.Length - 2) : "";
                tbSueldoBase.Text = em.SueldoBase.HasValue ? em.SueldoBase.ToString() : "0.00";

                lsSucursalesSupervisor.SelectedItems.Clear();
                if (em.Sucursales.Count > 1)
                {
                    foreach (var suc in em.Sucursales) lsSucursalesSupervisor.SelectedItems.Add(suc);
                }
                lsAtenciones.SelectedItems.Clear();
                foreach (var at in em.Atenciones) lsAtenciones.SelectedItems.Add(at);
                cbxActivo.IsChecked = em.Activo;
                cbEditarEmpleado.SelectedIndex = default;
                editState = true;
                edit = false;
            }
        }

        private List<string> getASupervisar()
        {
            List<string> ret = new List<string>();
            var asupervisar = lsSucursalesSupervisor.SelectedItems;
            foreach (var asu in asupervisar) ret.Add(asu.ToString());
            return ret;
        }
        private List<AtencionesModel> getAtenciones()
        {
            List<AtencionesModel> atenciones = new List<AtencionesModel>();
            var ats = lsAtenciones.SelectedItems;
            var at = lsAtenciones.Items;
            foreach (var ate in at)
            {
                AtencionesModel atencion = new AtencionesModel()
                {
                    Atencion = ate.ToString(),
                    Activa = ats.Contains(ate)
                };
                atenciones.Add(atencion);
            }
            return atenciones;
        }

        private void btCargarEmpleadoClick(object sender, RoutedEventArgs e)
        {
            if (cbEditarEmpleado.SelectedIndex > -1)
            {
                if (MessageBox.Show($"Editar {cbEditarEmpleado.Text}?, se perderan los datos actuales del formulario\n\n Desea continuar?", "Editar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    edit = true;
                    idEdit = emp.getIdEmpleado(cbEditarEmpleado.Text);
                    tbNombre.Focus();
                }
            }
        }
        public async Task<bool> Edit()
        {
            while (!cargarEdit)
            {
                await Task.Delay(100);
            }
            return true;
        }

        private void cbCargo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCargo.SelectedItem == null)
            {
                return;
            }
            if (cbCargo.SelectedValue.ToString() == "Supervisor")
            {
                lbSucursalesSupervisor.Visibility = Visibility.Visible;
                lsSucursalesSupervisor.IsEnabled = true;
                lsSucursalesSupervisor.Visibility = Visibility.Visible;
            }
            else
            {
                lbSucursalesSupervisor.Visibility = Visibility.Collapsed;
                lsSucursalesSupervisor.IsEnabled = false;
                lsSucursalesSupervisor.Visibility = Visibility.Collapsed;
                lsSucursalesSupervisor.SelectedIndex = -1;
            }
        }
    }
}