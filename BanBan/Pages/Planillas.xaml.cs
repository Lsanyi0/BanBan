using System.Collections.Generic;
using System.Windows.Controls;

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
        }
    }
}
