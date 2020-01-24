using BanBan.Controls;
using BanBan.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System;

namespace BanBan.Pages
{
    /// <summary>
    /// Interaction logic for HorasExtra.xaml
    /// </summary>
    public partial class HorasExtra : Page
    {
        private BindingList<HorasExtraModel> he;
        public HorasExtraOfflineControl heoc;
        public HorasExtraControl hec;
        private const string filename = "he.xml";
        public HorasExtra()
        {
            InitializeComponent();

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
            cbEmpleado.SelectedIndex = cbEmpleado.HasItems ? 0 : -1;

            dpDesde.SelectedDate = DateTime.Now.AddDays(-15);
            dpHasta.SelectedDate = DateTime.Now;

            he = File.Exists(filename) ? hec.CargarDatos() : new BindingList<HorasExtraModel>();
            dgvPlanilla.ItemsSource = he;
        }

        private void btCancelar_Click(object sender, RoutedEventArgs e)
        {
            //if (MessageBoxResult.Yes == MessageBox.Show("Test", "", MessageBoxButton.YesNo, MessageBoxImage.Question))
            //{
            //    he.Clear();
            //}
            dgvPlanilla.ItemsSource = he;
        }

        private void btAgregar_Click(object sender, RoutedEventArgs e)
        {
            he.Add(new HorasExtraModel
            {
                IdHe = he.Count + 1,
                Nombre = cbEmpleado.Text,
                Apellido = "",
                HoraInicio = System.DateTime.Now,
                HoraFinal = System.DateTime.Now.AddHours(2),
            });
        }
        private void ActualizarPadre(object sender, PropertyChangedEventArgs args)
        {
            HorasExtraModel model = (HorasExtraModel)sender;
            he = (BindingList<HorasExtraModel>)he.Where(x => x.IdHe != model.IdHe);
            he.Add(model);
        }
        private void btEnviarDatos_Click(object sender, RoutedEventArgs e)
        {
            //heoc = new HorasExtraOfflineControl();
            //heoc.CrearBDOffline();
        }

        private void btObtenerDatos_Click(object sender, RoutedEventArgs e)
        {
            heoc = new HorasExtraOfflineControl();
            heoc.CargarDB();
        }

        private void btGuardar_Click(object sender, RoutedEventArgs e)
        {
            hec.GuardarDatos(he);
        }

        private void btfiltrar_Click(object sender, RoutedEventArgs e)
        {
            BindingList<HorasExtraModel> hem = new BindingList<HorasExtraModel>(he.Where(x => x.Comentario != "abc").ToList());
            foreach (var hex in hem)
            {
                hex.PropertyChanged += ActualizarPadre;
            }
            dgvPlanilla.ItemsSource = hem;
        }

        private void btDatosDispositivo_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
