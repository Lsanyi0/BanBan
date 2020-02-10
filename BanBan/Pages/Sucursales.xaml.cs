using BanBan.Controls;
using BanBan.Model;
using System;
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
        diapatronal dp;
        SucursalesControl sc = new SucursalesControl();
        private string date = "";
        private int ind = 0;
        private List<string> lista;

        public Sucursales()
        {
            InitializeComponent();
            llenarDatos();
        }

        private void llenarDatos()
        {
            lista = new List<string>();
            var mn = from muni in sb.ciudad select muni.ciudad1;
            var dp = from dep in sb.departamento select dep.departamento1;
            var sup = from emp in sb.empleado
                      join cg in sb.cargo on emp.idCargo equals cg.idCargo
                      where cg.cargo1.Contains("supervisor")
                      select emp.nombre;
            cbEditarSucursal.ItemsSource = sc.getSucursales();
            if (cbEditarSucursal.Items.Count > 0) cbEditarSucursal.SelectedIndex = 0;
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
            if (string.IsNullOrEmpty(cbSupervisor.Text))
            {
                MessageBox.Show("Seleccione un supervisor para la sucursal");//revisar
                return;
            }

            string val = sc.GuardarSucursal(tbNombreSucursal.Text, tbDireccion.Text, cbMunicipio.Text,
                                            cbSupervisor.Text, cbDepartamento.Text, lista);

            if (val.Equals("OK"))
            {
                MessageBox.Show("Sucursal almacenada con exito");
                llenarDatos();
            }
            else
            {
                if (val.Equals("sucursal existente"))
                {
                    List<string> listaA = new List<string>();
                    listaA= sc.DeterminarAsuetos(cbMunicipio.Text);
                    if (listaA.Count()==lsAsuetos.Items.Count)
                    {
                        MessageBox.Show(val);
                    }
                    else
                    {
                        //Aqui prro
                        foreach (var item in lsAsuetos.Items)
                        {
                            DateTime dia = Convert.ToDateTime(item);
                            var mn = from muni in sb.ciudad where muni.ciudad1.Equals(cbMunicipio.Text) select muni.idCiudad;
                            int id = mn.FirstOrDefault();
                            var asut = from asu in sb.diapatronal
                                       join cd in sb.ciudad on asu.idCiudad equals cd.idCiudad
                                       where asu.dia == dia && asu.idCiudad == id
                                       select asu.idDiaPatronal;
                            foreach (var asuetos in asut)
                            {
                                var diaD = new diapatronal { idDiaPatronal = int.Parse(asuetos.ToString())};
                                sb.Entry(diaD).State = System.Data.Entity.EntityState.Deleted;
                                sb.SaveChangesAsync();
                            }
                                dp = new diapatronal();
                                dp.dia = dia;
                                dp.idCiudad = id;
                                sb.diapatronal.Add(dp);
                                sb.SaveChangesAsync();
                            

                        }
                        MessageBox.Show("datos actualizados");
                    }
                }
            }
        }

        private void CbMunicipio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var asu = (from at1 in sb.diapatronal
                      join mun in sb.ciudad
                       on at1.idCiudad equals mun.idCiudad
                      where mun.ciudad1 == cbMunicipio.Text
                      select at1.dia).ToList();
            if (asu == null)
            {

            }
            else
            {

                lsAsuetos.ItemsSource = asu;

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
            List<string> list = new List<string>();
            
            foreach (var item in lsAsuetos.Items)
            {
                list.Add(item.ToString());
            }
            lista.Clear();
            foreach (var item in list)
            {
                lista.Add(item);
            }
            lsAsuetos.ItemsSource="";
            lista.Add(date.ToString());
            lsAsuetos.ItemsSource = lista;
            
        }

        public void eliminarAsueto(string asuetoL)
        {
            string var = "";
            foreach (var item in lista)
            {
                if (item.ToString()==asuetoL)
                {
                    var = item;
                }
            }
            lista.Remove(var);

            lsAsuetos.ItemsSource = "";
            lsAsuetos.ItemsSource = lista;
            //lista.RemoveAt(ind);
            //if (lista.Count == 0)
            //{
            //    lsAsuetos.SelectedIndex = 0;
            //    lsAsuetos.Items.Clear();
            //}
            //else
            //{
            //    lsAsuetos.Items.Clear();
            //    for (int i = 0; i < lista.Count; i++)
            //    {
            //        lsAsuetos.Items.Add(lista[i]);
            //    }
            //}
        }
        private void btQuitar_Click(object sender, RoutedEventArgs e)
        {
            List<string> list = new List<string>();

            if (lsAsuetos.SelectedItem == null)
            {
                return;
            }
            string asuetoL = lsAsuetos.SelectedItem.ToString();
            //foreach (var item in lsAsuetos.Items)
            //{
            //    list.Add(item.ToString());
            //}
            //lista.Clear();
            //foreach (var item in list)
            //{
            //    lista.Add(item);
            //}
            //for (int i = 0; i < lista.Count; i++)
            //{
            //    if (lista[i].ToString() == asuetoL)
            //    {
            //        ind = i;
            //    }
            //}

            eliminarAsueto(asuetoL);
        }



        private void btLimpiar_Click(object sender, RoutedEventArgs e)
        {
            lsAsuetos.ItemsSource="";
            lista.Clear();
        }

        private void cbMunicipio_DropDownClosed(object sender, System.EventArgs e)
        {
            lista.Clear();
            lista = sc.DeterminarAsuetos(cbMunicipio.Text);
            lsAsuetos.ItemsSource="";
            if (lista.Count == 0)
            {
                lsAsuetos.ItemsSource = lista;
            }
            else
            {
                MessageBox.Show("El municipio seleccionado ya tiene asuetos, pero puede agregar más");
                lsAsuetos.ItemsSource = lista;

            }

        }
        private void btCargarSucursalesClick(object sender, RoutedEventArgs e)
        {
            if (cbEditarSucursal.SelectedIndex > -1)
            {
                if (MessageBox.Show($"Editar {cbEditarSucursal.Text}, se perderan los datos actuales del formulario\n\n Desea continuar?", "Editar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    lsAsuetos.ItemsSource = "";
                    SucursalModel suc = sc.getSucursal(cbEditarSucursal.Text);
                    tbNombreSucursal.Text = suc.NombreSucursal ?? "";
                    cbDepartamento.SelectedItem = suc.Departamento;
                    cbMunicipio.SelectedItem = suc.Municipio;
                    tbDireccion.Text = suc.Direccion;
                    lsAsuetos.ItemsSource = "";
                    List<string> list = new List<string>();
                    foreach (var asu in suc.DiasAsueto) list.Add(asu.ToString());
                        lsAsuetos.ItemsSource=list;
                    cbSupervisor.SelectedItem = suc.NombreEmpleado;
                }
            }
        }
    }
}
