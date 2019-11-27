using BanBan.Controls;
using BanBan.Model;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace BanBan.Pages
{
    /// <summary>
    /// Interaction logic for HorasExtra.xaml
    /// </summary>
    public partial class HorasExtra : Page
    {
        BindingList<HorasExtraModel> he = new BindingList<HorasExtraModel>();
        public HorasExtraOfflineControl heoc;
        public HorasExtraControl hec;
        public HorasExtra()
        {
            InitializeComponent();

            dgvPlanilla.ItemsSource = he;

            if (LoginControl.offline)
            {
                heoc = new HorasExtraOfflineControl();
                hec = new HorasExtraControl(heoc.CargarBDOffline());
            }
            else
            {
                hec = new HorasExtraControl();
            }
            cbEmpleado.ItemsSource = hec.GetCBEmplados();
        }

        private void btCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxResult.Yes == MessageBox.Show("Test", "", MessageBoxButton.YesNo, MessageBoxImage.Question))
            {
                he.Clear();
            }
        }

        private void btAgregar_Click(object sender, RoutedEventArgs e)
        {
            he.Add(new HorasExtraModel { Nombre = "Name", Apellido = "Kusei" });
        }

        private void btEnviarDatos_Click(object sender, RoutedEventArgs e)
        {

            heoc.CrearBDOffline();
        }

        private void btObtenerDatos_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
