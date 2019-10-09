using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BanBan.Controls;


namespace BanBan.Pages
{

    /// <summary>
    /// Interaction logic for Sucursales.xaml
    /// </summary>
    public partial class Sucursales : Page
    {
        sBanBan sb = new sBanBan();
        SucursalesControl sc = new SucursalesControl();
        private string date="";
        private int ind = 0;
        private List<string> lista;
        public Sucursales()
        {
            InitializeComponent();
        lista = new List<string>();
        var mn = from muni in sb.Ciudad select muni.ciudad1;
            var dp = from dep in sb.Departamento select dep.departamento1;
            var sup = from emp in sb.Empleado join cg in sb.Cargo
                     on emp.idCargo equals cg.idCargo
                      where cg.cargo1 == "supervisor"
                     select emp.nombre;
           

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
            List<string> asuetos = new List<string>();

            for (int i = 0; i < lsAsuetos.Items.Count - 1; i++)
            {
                asuetos.Add(lsAsuetos.Items[i].ToString());
            }

            string val = sc.GuardarSucursal(tbNombreSucursal.Text, tbDireccion.Text, cbMunicipio.Text,
                                            cbSupervisor.Text, asuetos);

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
            var asu = from at1 in sb.DiaPatronal
                       join mun in sb.Ciudad
                        on at1.idCiudad equals mun.idCiudad
                       where mun.ciudad1 == cbMunicipio.Text
                       select at1.diaInicial;

            if (asu == null)
            {

            }
            else
            {
                List<string> asuetos = new List<string>();
                asuetos = sc.DeterminarAsuetos(asu.ToString());
                for (int i = 0; i < asuetos.Count - 1; i++)
                {
                    lsAsuetos.Items.Add(asuetos[i]);
                }
            }
        }

        private void tbAsueto_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            date = tbAsueto.SelectedDate.ToString();
        }

        private void btAgregar_Click(object sender, RoutedEventArgs e)
        {
            agregarAsueto();
        }

        public void agregarAsueto()
        {
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
            for (int i = 0; i < lista.Count; i++)
            {
                lsAsuetos.Items.Add(lista[i]);
            }
        }
        private void btQuitar_Click(object sender, RoutedEventArgs e)
        {
            lsAsuetos.Items.Clear();
            eliminarAsueto();
        }
      

        private void lsAsuetos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ind=lsAsuetos.SelectedIndex;
            ind = ind*(-1);
        }

        private void btLimpiar_Click(object sender, RoutedEventArgs e)
        {
            lsAsuetos.Items.Clear();
            lista.Clear();
        }
    }
}
