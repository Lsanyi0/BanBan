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
        private Pages.Configuracion configuracion;
        private Pages.pruebaDatos pruebaDatos;
        public static readonly List<string> tiposUsuario = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            empleados = new Pages.Empleados();
            planillas = new Pages.Planillas();
            sucursales = new Pages.Sucursales();
            pruebaDatos = new Pages.pruebaDatos();
            lgc = new LoginControl();
            anims = new List<Storyboard>
            {
                FindResource("stbLoginCorrecto") as Storyboard,
                FindResource("stbMostrarOpciones") as Storyboard,
                FindResource("stbOcultarOpciones") as Storyboard
            };
            anim = true;
            //para configurar en caso de que no se utilicen estos nombres
            tiposUsuario.Add("Administrador");
            tiposUsuario.Add("Supervisor");
            tiposUsuario.Add("Root");
        }

        //Usado como trigger para la animacion de fade-out del login
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ = await lgc.verificarUsuario();
            BeginStoryboard(anims[0]);

            if (LoginControl.tipoUsuario != null) configuracion = new Pages.Configuracion();
            else Close();

            if (LoginControl.tipoUsuario == tiposUsuario[0])
            {
                activarTodos();
            }
            else if (LoginControl.tipoUsuario == tiposUsuario[1])
            {
                ocultarTodos();
            }
            else if (LoginControl.tipoUsuario == tiposUsuario[2])
            {
                activarIT();
            }
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

        private void btReportesClick(object sender, RoutedEventArgs e)
        {
            //if (frPpal.Content.GetType() != typeof(Pages.Configuracion))
            //{
            //    
            //}
        }

        private void btConfigurarClick(object sender, RoutedEventArgs e)
        {
            if (frPpal.Content.GetType() != typeof(Pages.Configuracion))
            {
                frPpal.Content = configuracion;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (frPpal.Content.GetType() != typeof(Pages.pruebaDatos))
            {
                frPpal.Content = pruebaDatos;
                btPrueba.Visibility = Visibility.Hidden;
            }
        }
        private void ocultarTodos()
        {
            btOpciones.Visibility = Visibility.Collapsed;
            btSucursales.Visibility = Visibility.Collapsed;
            btPlanillas.Visibility = Visibility.Collapsed;
            btReportes.Visibility = Visibility.Collapsed;
            btAgregarEmpleados.Visibility = Visibility.Collapsed;
            btConfigurar.Visibility = Visibility.Collapsed;
        }
        private void activarTodos()
        {
            btSucursales.Visibility = Visibility.Visible;
            btPlanillas.Visibility = Visibility.Visible;
            btReportes.Visibility = Visibility.Visible;
            btAgregarEmpleados.Visibility = Visibility.Visible;
            frPpal.Content = planillas;
        }
        private void activarIT()
        {
            btSucursales.Visibility = Visibility.Collapsed;
            btPlanillas.Visibility = Visibility.Collapsed;
            btReportes.Visibility = Visibility.Collapsed;
            btAgregarEmpleados.Visibility = Visibility.Collapsed;
            frPpal.Content = configuracion;
        }
    }
}
