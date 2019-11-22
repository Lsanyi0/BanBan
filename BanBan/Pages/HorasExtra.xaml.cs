using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using BanBan.Model;

namespace BanBan.Pages
{
    /// <summary>
    /// Interaction logic for HorasExtra.xaml
    /// </summary>
    public partial class HorasExtra : Page
    {
        BindingList<HorasExtraModel> he = new BindingList<HorasExtraModel>();
        public HorasExtra()
        {
            InitializeComponent();
            dgvPlanilla.ItemsSource = he;
        }

        private void btCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxResult.Yes == MessageBox.Show("Test","",MessageBoxButton.YesNo,MessageBoxImage.Question))
            {
                he.Clear();
            }
        }

        private void btAgregar_Click(object sender, RoutedEventArgs e)
        {
            he.Add(new HorasExtraModel { Nombre = "Name", Apellido = "Kusei" });
        }
    }
}
