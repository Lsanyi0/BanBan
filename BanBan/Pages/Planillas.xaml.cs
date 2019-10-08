using BanBan.Controls;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;

namespace BanBan.Pages
{
    /// <summary>
    /// Interaction logic for Planillas.xaml
    /// </summary>
    public partial class Planillas : Page
    {
        public Planillas()
        {
            string nada = "NADA";
            InitializeComponent();
            for (int i = 0; i < 50; i++)
            {
                List<string> Nombre = new List<string>
                {
                    nada
                };
                dgvPlanilla.Items.Add(Nombre);
            }
            lbNumero.Content = dgvPlanilla.Items.Count;

            var sc = from suc in Utilidades.sb.Sucursal select suc.sucursal1;
            if (sc != null)
            {
                cbSucursal.ItemsSource = sc.ToList();            
                cbSucursal.SelectedIndex = 0;
            }
        }
    }
}
