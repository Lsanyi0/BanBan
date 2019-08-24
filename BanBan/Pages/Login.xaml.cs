using System.Windows.Controls;
using BanBan.Controls;

namespace BanBan.Pages
{

    public partial class Login : Page
    {
        /// <summary>
        /// Interaction logic for MainWindow.xaml
        /// </summary>
        public Login()
        {
            InitializeComponent();
        }

        private  void BtIniciarSesion_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LoginControl.UsuarioValido = true;
        }
    }
}
