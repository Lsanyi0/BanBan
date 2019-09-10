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
        private bool anim;
        private List<Storyboard> anims;
        private LoginControl lgc;
        private Pages.Empleados empleados;
        private Pages.Planillas planillas;
        private Pages.Sucursales sucursales;
        public MainWindow()
        {
            InitializeComponent();
            empleados = new Pages.Empleados();
            planillas = new Pages.Planillas();
            sucursales = new Pages.Sucursales();
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
            frPpal.Content = planillas;
            frPpal.IsEnabled = true;
        }

        //Trigger para las animaciones de btOpciones
        private void BtOpciones_Click(object sender, RoutedEventArgs e)
        {
            if (anim) { BeginStoryboard(anims[1]); anim = false; }
            else { BeginStoryboard(anims[2]); anim = true; }
        }

        //Cargar contenido en el formulario ppal
        private void btEmpleadoClick(object sender, RoutedEventArgs e)
        {
            if (frPpal.Content.GetType() != typeof(Pages.Empleados))
            {
                frPpal.Content = empleados;
            }
        }
        private void btPlanillasClick(object sender, RoutedEventArgs e)
        {
            if (frPpal.Content.GetType() != typeof(Pages.Planillas))
            {
                frPpal.Content = planillas;
            }
        }
        private void btSucursalesClick(object sender, RoutedEventArgs e)
        {
            if (frPpal.Content.GetType() != typeof(Pages.Sucursales))
            {
                frPpal.Content = sucursales;
            }
        }
    }
}
