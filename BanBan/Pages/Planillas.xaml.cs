﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BanBan.Pages
{
    /// <summary>
    /// Interaction logic for Planillas.xaml
    /// </summary>
    public partial class Planillas : Page
    {
        public Planillas()
        {
            string xD = "xD";
            InitializeComponent();
            for (int i = 0; i < 50; i++)
            {
                xD += "D";
                List<string> Nombre = new List<string>
                {
                    xD
                };
                dgvPlanilla.Items.Add(Nombre);
            }
        }
    }
}