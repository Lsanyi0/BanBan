using BanBan.Controls;
using BanBan.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace BanBan.Pages
{

    /// <summary>
    /// Interaction logic for Sucursales.xaml
    /// </summary>
    public partial class Sucursales : Page
    {
        sBanBan sb = new sBanBan();
        SucursalesControl sc = new SucursalesControl();
        private string date = "";
        private int ind = 0;
        private List<string> lista;
        private bool edit;
        public Sucursales()
        {
            InitializeComponent();
            lista = new List<string>();
            var mn = from muni in sb.ciudad select muni.ciudad1;
            var dp = from dep in sb.departamento select dep.departamento1;
            var sup = from emp in sb.empleado
                      join cg in sb.cargo on emp.idCargo equals cg.idCargo
                      where cg.cargo1 == "supervisor"
                      select emp.nombre;
            cbEditarSucursal.ItemsSource = sc.getSucursales();
            if (cbEditarSucursal.Items.Count > 0) cbEditarSucursal.SelectedIndex = 0;
            edit = false;
            if (mn != null)
            {
                cbMunicipio.ItemsSource = mn.ToList();
                cbMunicipio.SelectedIndex = 0;
            }
            if (dp != null)
            {
                cbDepartamento.ItemsSource = dp.ToList();
                cbDepartamento.SelectedIndex = 0;
            }
            if (sup != null)
            {
                cbSupervisor.ItemsSource = sup.ToList();
                cbSupervisor.SelectedIndex = 0;
            }
        }

        private void btGuardarClick(object sender, RoutedEventArgs e)
        {

            string val = sc.GuardarSucursal(tbNombreSucursal.Text, tbDireccion.Text, cbMunicipio.Text,
                                            cbSupervisor.Text, cbDepartamento.Text, lista);

            if (val.Equals("OK"))
            {
                MessageBox.Show("Sucursal almacenada con exito");
            }
            else
            {
                MessageBox.Show(val);
            }
        }

        private void CbMunicipio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var asu = from at1 in sb.diapatronal
                      join mun in sb.ciudad
                       on at1.idCiudad equals mun.idCiudad
                      where mun.ciudad1 == cbMunicipio.Text
                      select at1.dia;
            if (asu == null)
            {

            }
            else
            {

                foreach (var list in asu)
                {
                    lsAsuetos.Items.Add(list);
                }

            }
        }

        private void tbAsueto_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            date = tbAsueto.SelectedDate.ToString();
            string[] fecha = date.Split(' ');
            date = fecha[0];
        }

        private void btAgregar_Click(object sender, RoutedEventArgs e)
        {
            agregarAsueto();
        }

        public void agregarAsueto()
        {
            lista.Clear();
            for (int i = 0; i < lsAsuetos.Items.Count; i++)
            {
                lista.Add(lsAsuetos.Items[i].ToString());
            }

            lsAsuetos.Items.Clear();
            lista.Add(date.ToString());
            for (int i = 0; i < lista.Count; i++)
            {
                lsAsuetos.Items.Add(lista[i]);
            }
        }

        public void eliminarAsueto()
        {
            lista.RemoveAt(ind);
            if (lista.Count == 0)
            {
                lsAsuetos.SelectedIndex = 0;
                lsAsuetos.Items.Clear();
            }
            else
            {
                lsAsuetos.Items.Clear();
                for (int i = 0; i < lista.Count; i++)
                {
                    lsAsuetos.Items.Add(lista[i]);
                }
            }
        }
        private void btQuitar_Click(object sender, RoutedEventArgs e)
        {
            if (lsAsuetos.SelectedItem == null)
            {
                return;
            }
            string asuetoL = lsAsuetos.SelectedItem.ToString();
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].ToString() == asuetoL)
                {
                    ind = i;
                }
            }

            eliminarAsueto();
        }



        private void btLimpiar_Click(object sender, RoutedEventArgs e)
        {
            lsAsuetos.Items.Clear();
            lista.Clear();
        }

        private void cbMunicipio_DropDownClosed(object sender, System.EventArgs e)
        {
            lista.Clear();
            lista = sc.DeterminarAsuetos(cbMunicipio.Text);
            lsAsuetos.Items.Clear();
            if (lista.Count == 0)
            {

            }
            else
            {
                MessageBox.Show("El municipio seleccionado ya tiene asuetos, pero puede agregar más");
                foreach (var item in lista)
                {
                    lsAsuetos.Items.Add(item);
                }

            }

        }
        private void btCargarSucursalesClick(object sender, RoutedEventArgs e)
        {
            if (cbEditarSucursal.SelectedIndex > -1)
            {
                if (MessageBox.Show($"Editar {cbEditarSucursal.Text}, se perderan los datos actuales del formulario\n\n Desea continuar?", "Editar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    edit = true;
                    SucursalModel suc = sc.getSucursal(cbEditarSucursal.Text);
                    tbNombreSucursal.Text = suc.NombreSucursal;
                    cbDepartamento.SelectedItem = suc.Departamento;
                    cbMunicipio.SelectedItem = suc.Municipio;
                    tbDireccion.Text = suc.Direccion;
                    foreach (var asu in suc.DiasAsueto) lsAsuetos.Items.Add(asu);
                    cbSupervisor.SelectedItem = suc.NombreEmpleado;
                }
            }
        }
    }
}
