using BanBan.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;


namespace BanBan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //**Para no perder el progreso en un form crear un stack de las paginas abiertas
        //buscar una instancia de la pagina a la que se desea navegar, si existe cargarla, sino crearla
        private bool anim;
        private List<Storyboard> anims;
        private LoginControl lgc;
        public MainWindow()
        {
            InitializeComponent();
            lgc = new LoginControl();
            anims = new List<Storyboard>
            {
                FindResource("stbLoginCorrecto") as Storyboard,
                FindResource("stbMostrarOpciones") as Storyboard,
                FindResource("stbOcultarOpciones") as Storyboard
            };
            anim = true;
        }

        //Usado como trigger para la animacion de fade-out del login
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ = await lgc.verificarUsuario();
            BeginStoryboard(anims[0]);
            frPpal.Content = new Pages.Planillas();
            frPpal.IsEnabled = true;
        }
        //Trigger para las animaciones de btOpciones
        private void BtOpciones_Click(object sender, RoutedEventArgs e)
        {
            if (anim) { BeginStoryboard(anims[1]); anim = false; }
            else { BeginStoryboard(anims[2]); anim = true; }
        }
    }
}
