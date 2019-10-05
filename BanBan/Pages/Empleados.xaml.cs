using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using BanBan.Controls;
using System.Windows;

namespace BanBan.Pages
{
    /// <summary>
    /// Interaction logic for Empleados.xaml
    /// </summary>
    public partial class Empleados : Page
    {
        //sb es una instancia del "contexto" de la base de datos (base de datos mapeada)
        EmpleadosControl emp = new EmpleadosControl();
        public Empleados()
        {
            InitializeComponent();
            empleadosLoad();
        }
        private void empleadosLoad()
        {
            dpFechaContrato.SelectedDate = System.DateTime.Now;

            cbCargo.ItemsSource = emp.getCargo();
            cbSucursal.ItemsSource = emp.getSucursal();
            cbAfiliacion.ItemsSource = emp.getSistemaPension();

            cbCargo.SelectedIndex = 0;
            cbSucursal.SelectedIndex = 0;
            cbAfiliacion.SelectedIndex = 0;
        }

        private void btGuardarClick(object sender, System.Windows.RoutedEventArgs e)
        {
            string val = emp.save(tbNombre.Text, tbApellido.Text, tbDUI.Text, tbNIT.Text,
                    dpFechaContrato.DisplayDate, cbAfiliacion.Text, tbNumeroAfiliado.Text,
                    cbSucursal.Text, cbCargo.Text, tbSueldoBase.Text, null, cbxActivo.IsEnabled);
            if (val != "OK") MessageBox.Show("Advertencia: " + val, "Advertencia!",MessageBoxButton.OK,MessageBoxImage.Exclamation) ;
        }
    }
}
