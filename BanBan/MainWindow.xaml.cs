using System.Windows;
using System.Windows.Media.Animation;
using BanBan.Controls;


namespace BanBan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LoginControl lgc;
        public MainWindow()
        {
            InitializeComponent();
            lgc  = new LoginControl();
        }

        //Usado como trigger para la animacion de fade-out del login
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ = await lgc.verificarUsuario();
            Storyboard sb = new MainWindow().FindResource("stbLoginCorrecto") as Storyboard;
            if (sb != null)
            {
                BeginStoryboard(sb);
            }
        }
    }
}
