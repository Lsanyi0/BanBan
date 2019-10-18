using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BanBanHorasExtra
{
    public partial class BanBanHorasExtra : Form
    {
        sbanbanHE he = new sbanbanHE();
        public BanBanHorasExtra()
        {
            InitializeComponent();
            cbEmpleados.DataSource = (from suc in he.sucursal select suc.sucursal1).ToList();
        }
    }
}
