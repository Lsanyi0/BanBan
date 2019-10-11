using System.Linq;
using System.Windows.Controls;

namespace BanBan.Pages
{
    /// <summary>
    /// Interaction logic for Configuracion.xaml
    /// </summary>
    public partial class Configuracion : Page
    {

        sBanBan sb = new sBanBan();

        public Configuracion()
        {
            InitializeComponent();

            var carg = from cr in sb.cargo
                       select cr.cargo1;
            var aten = from at in sb.atencion
                       select at.atencion1;
            var hre = from he in sb.tipohora
                      select he.tipo;
            var suc = from sc in sb.sucursal
                      select sc.sucursal1;
            var emp = from em in sb.empleado
                      select em.nombre;
            var usu = from us in sb.usuario
                      select us.usuario1;

            if (carg != null)
            {
                cbCargo.ItemsSource = carg.ToList();
                cbCargo.SelectedIndex = 0;
            }
            if (aten != null)
            {
                cbAtenciones.ItemsSource = aten.ToList();
                cbAtenciones.SelectedIndex = 0;
            }
            if (hre != null)
            {
                cbHoraExtra.ItemsSource = hre.ToList();
                cbHoraExtra.SelectedIndex = 0;
            }
            if (suc != null)
            {
                cbSucursalDispositivo.ItemsSource = suc.ToList();
                cbSucursalDispositivo.SelectedIndex = 0;
            }
            if (emp != null)
            {
                cbEmpleado.ItemsSource = emp.ToList();
                cbEmpleado.SelectedIndex = 0;
            }
            if (usu != null)
            {
                cbUsuario.ItemsSource = usu.ToList();
                cbUsuario.SelectedIndex = 0;
            }
        }
    }
}