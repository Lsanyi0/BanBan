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
                    MessageBox.Show("Usuario o contraseña no valida.\n\n" +
                        "Porfavor intente de nuevo", "Advertencia!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else MessageBox.Show("Usuario o contraseña vacia.\n\n Porfavor ingrese sus datos.",
                "Advertencia!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void pwbClaveKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                BtIniciarSesion_Click(sender, e);
            }
        }
        private void tbUsuarioKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                BtIniciarSesion_Click(sender, e);
            }
        }

        private void tbUsuarioGotFocus(object sender, RoutedEventArgs e)
        {
            if (tbUsuario.Text == "Usuario")
            {
                tbUsuario.Clear();
            }
        }

        private void pwbClaveGotFocus(object sender, RoutedEventArgs e)
        {
            if (pwbClave.Password == "     ")
            {
                pwbClave.Clear();
            }
        }

        private void pwbClaveLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pwbClave.Password))
            {
                pwbClave.Password = "     ";
            }
        }

        private void tbUsuarioLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbUsuario.Text))
            {
                tbUsuario.Text = "Usuario";
            }
        }
    }
}
