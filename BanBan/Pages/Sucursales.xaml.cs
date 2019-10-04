using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace BanBan.Pages
{

    /// <summary>
    /// Interaction logic for Sucursales.xaml
    /// </summary>
    public partial class Sucursales : Page
    {
        int f=0;
        sBanBan sb = new sBanBan();
        public Sucursales()
        {
            InitializeComponent();
            var mn = from muni in sb.Ciudad select muni.ciudad1;

            var dp = from dep in sb.Departamento select dep.departamento1;
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
        }
    }
}
