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
        sBanBan sb = new sBanBan();
        public Sucursales()
        {
            InitializeComponent();
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
    }
}
