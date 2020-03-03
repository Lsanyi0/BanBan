using BanBan.Controls;
using BanBan.Pages;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
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
        private Empleados empleados;
        private Planillas planillas;
        private Sucursales sucursales;
        private Configuracion configuracion;
        private Reportes reportes;
        private HorasExtra HorasExtra;
        private UsuariosDispositivo UsuariosDispositivo;
        public static readonly List<string> tiposUsuario = new List<string>();
        private int contador = 0;
        public MainWindow()
        {
            LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(
            XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            InitializeComponent();

            anims = new List<Storyboard>
            {
                FindResource("stbLoginCorrecto") as Storyboard,
                FindResource("stbMostrarOpciones") as Storyboard,
                FindResource("stbOcultarOpciones") as Storyboard,
                FindResource("stbLoginLoad") as Storyboard
            };
            //para configurar en caso de que no se utilicen estos nombres
            tiposUsuario.Add("Administrador");
            tiposUsuario.Add("Supervisor");
            tiposUsuario.Add("Root");

            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();

            if (Properties.Settings.Default.Dropbox == "C:\\")
            {
                Properties.Settings.Default.Dropbox = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonDocuments);
                Properties.Settings.Default.Save();
            }
        }

        //Usado como trigger para la animacion de fade-out del login
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lgc = new LoginControl();
            _ = await lgc.verificarUsuario();
            anims[0].Begin();
            if (LoginControl.tipoUsuario == null) Close();

            anim = true;
            Mouse.OverrideCursor = Cursors.Wait;
            if (LoginControl.tipoUsuario == tiposUsuario[0])
            {
                empleados = new Empleados();
                planillas = new Planillas();
                sucursales = new Sucursales();
                configuracion = new Configuracion();
                reportes = new Reportes();
                activarTodos();
            }
            else if (LoginControl.tipoUsuario == tiposUsuario[1])
            {
                HorasExtra = new HorasExtra();
                UsuariosDispositivo = new UsuariosDispositivo();
                ocultarTodos();
            }
            else if (LoginControl.tipoUsuario == tiposUsuario[2])
            {
                configuracion = new Configuracion();
                activarIT();
            }
            frPpal.IsEnabled = true;
            Mouse.OverrideCursor = Cursors.Arrow;
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
                frPpal.Navigate(empleados);
            }
        }
        private void btPlanillasClick(object sender, RoutedEventArgs e)
        {
            if (frPpal.Content.GetType() != typeof(Planillas))
            {
                frPpal.Navigate(planillas);
            }
        }
        private void btSucursalesClick(object sender, RoutedEventArgs e)
        {
            if (frPpal.Content.GetType() != typeof(Sucursales))
            {
                frPpal.Navigate(sucursales);
            }
        }

        private void btReportesClick(object sender, RoutedEventArgs e)
        {
            if (frPpal.Content.GetType() != typeof(Configuracion))
            {
                frPpal.Navigate(reportes);
            }
        }

        private void btConfigurarClick(object sender, RoutedEventArgs e)
        {
            if (frPpal.Content.GetType() != typeof(Configuracion))
            {
                frPpal.Navigate(configuracion);
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
            btPlanillasHE.Visibility = Visibility.Visible;
            btReportes.Visibility = Visibility.Collapsed;
            btAgregarEmpleados.Visibility = Visibility.Collapsed;
            btConfigurar.Visibility = Visibility.Collapsed;
            btConfigurarHorasExtra.Visibility = Visibility.Visible;
            if (LoginControl.tipoUsuario == tiposUsuario[1])
            {
                frPpal.Navigate(HorasExtra);
            }
        }
        private void activarTodos()
        {
            btSucursales.Visibility = Visibility.Visible;
            btPlanillas.Visibility = Visibility.Visible;
            btPlanillasHE.Visibility = Visibility.Collapsed;
            btReportes.Visibility = Visibility.Visible;
            btAgregarEmpleados.Visibility = Visibility.Visible;
            btConfigurar.Visibility = 0;
            btConfigurarHorasExtra.Visibility = Visibility.Collapsed;
            frPpal.Navigate(planillas);
        }
        private void activarIT()
        {
            btSucursales.Visibility = Visibility.Collapsed;
            btPlanillas.Visibility = Visibility.Collapsed;
            btReportes.Visibility = Visibility.Collapsed;
            btAgregarEmpleados.Visibility = Visibility.Collapsed;
            btPlanillasHE.Visibility = Visibility.Collapsed;
            btConfigurar.Visibility = Visibility.Collapsed;
            frPpal.Navigate(configuracion);
        }

        private async void frPpal_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (frPpal.Content == null) { contador = 0; return; }
            if (frPpal.Content.GetType() == typeof(Planillas) && contador < 1)
            {
                contador += 1;
                await empleados.Edit();
                Empleados.edit = true;
                empleados.tbApellido.Focus();
                frPpal.Navigate(empleados);
                Empleados.cargarEdit = false;
                contador = 0;
            }
        }

        private void btConfigurarHorasExtra_Click(object sender, RoutedEventArgs e)
        {
            if (frPpal.Content.GetType() != typeof(UsuariosDispositivo))
            {
                frPpal.Navigate(UsuariosDispositivo);
            }
        }

        private void btPlanillasHE_Click(object sender, RoutedEventArgs e)
        {
            if (frPpal.Content.GetType() != typeof(HorasExtra))
            {
                frPpal.Navigate(HorasExtra);
            }
        }
    }
}
