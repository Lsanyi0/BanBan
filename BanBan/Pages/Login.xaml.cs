using System.Windows;
using System.Windows.Controls;
using BanBan.Controls;

namespace BanBan.Pages
{

    public partial class Login : Page
    {
        /// <summary>
        /// Interaction logic for MainWindow.xaml
        /// </summary>
        LoginControl lgc;
        public Login()
        {
            InitializeComponent();
            lgc = new LoginControl();
        }

        private void BtIniciarSesion_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbUsuario.Text) && !string.IsNullOrWhiteSpace(pwbClave.Password))
            {
                lgc.isUsuarioValido(tbUsuario.Text, pwbClave.Password);
                if (!LoginControl.UsuarioValido) 
                {
                    MessageBox.Show("Usuario o contraseña no valida\n\n" +
                        "Porfavor intente de nuevo","Advertencia!",MessageBoxButton.OK,MessageBoxImage.Warning);
                }
            }
        }
    }
}
