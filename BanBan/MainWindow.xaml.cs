using BanBan.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;
using BanBan.Pages;


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
        private Empleados empleados;
        private Planillas planillas;
        private Sucursales sucursales;
        private Configuracion configuracion;
        private pruebaDatos pruebaDatos;
        public static readonly List<string> tiposUsuario = new List<string>();
        //
        private int contador = 0;
        public MainWindow()
        {
            InitializeComponent();
            empleados = new Empleados();
            planillas = new Planillas();
            sucursales = new Sucursales();
            pruebaDatos = new pruebaDatos();
            lgc = new LoginControl();
            anims = new List<Storyboard>
            {
                FindResource("stbLoginCorrecto") as Storyboard,
                FindResource("stbMostrarOpciones") as Storyboard,
                FindResource("stbOcultarOpciones") as Storyboard,
                FindResource("stbLoginLoad") as Storyboard
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
            anims[0].Begin();
            if (LoginControl.tipoUsuario != null) configuracion = new Configuracion();
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
        //Cierra sesion y ejecuta las animaciones correspondientes
        private void cerrarSesion()
        {
            lgc = new LoginControl();
            frLogin.Content = new Login();
            ocultarTodos();
            frPpal.Content = null;
            grid.IsEnabled = false;
            stpOpciones.IsEnabled = false;
            anims[3].Begin();
            Window_Loaded(null, null);
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
            if (frPpal.Content.GetType() != typeof(Empleados))
            {
                frPpal.Content = empleados;
            }
        }
        private void btPlanillasClick(object sender, RoutedEventArgs e)
        {
            if (frPpal.Content.GetType() != typeof(Planillas))
            {
                frPpal.Content = planillas;
            }
        }
        private void btSucursalesClick(object sender, RoutedEventArgs e)
        {
            if (frPpal.Content.GetType() != typeof(Sucursales))
            {
                frPpal.Content = sucursales;
            }
        }

        private void btReportesClick(object sender, RoutedEventArgs e)
        {
            //if (frPpal.Content.GetType() != typeof(Configuracion))
            //{
            //    
            //}
        }

        private void btConfigurarClick(object sender, RoutedEventArgs e)
        {
            if (frPpal.Content.GetType() != typeof(Configuracion))
            {
                frPpal.Content = configuracion;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (frPpal.Content.GetType() != typeof(pruebaDatos))
            {
                frPpal.Content = pruebaDatos;
                btPrueba.Visibility = Visibility.Hidden;
            }
        }
        private void btCerrarSesionClick(object sender, RoutedEventArgs e)
        {
            cerrarSesion();
        }
        private void ocultarTodos()
        {
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

        private async void frPpal_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (frPpal.Content.GetType() == typeof(Planillas) && contador < 1)
            {
                contador += 1;
                await empleados.Edit();
                Empleados.edit = true;
                empleados.tbApellido.Focus();
                frPpal.Content = empleados;
                Empleados.cargarEdit = false;
                contador = 0;
            }
        }
    }
}
