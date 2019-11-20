using BanBan.Controls;
using System.Windows.Controls;
using System.Windows;
using BanBan.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System;

namespace BanBan.Pages
{
    /// <summary>
    /// Interaction logic for Planillas.xaml
    /// </summary>
    public partial class Planillas : Page
    {
        PlanillasControl pc = new PlanillasControl();
        BindingList<PlanillaModel> pm;

        public Planillas()
        {
            InitializeComponent();

            cbSucursal.ItemsSource = pc.getSucursales();
            cbSucursal.SelectedIndex = 0;

            pm = File.Exists("planilla.xml") ? CargarXML() : pc.getEmpleados() ;

            pm.ListChanged += Actualizar;

            dgvEditar.ItemsSource = pm;

            ActualizarModelo();

            lbNumero.Content = dgvPlanilla.Items.Count;
        }

        private BindingList<PlanillaModel> CargarXML()
        {
            XmlSerializer xml = new XmlSerializer(typeof(BindingList<PlanillaModel>));
            BindingList<PlanillaModel> planillaXML;
            using (FileStream fileStream = new FileStream("planilla.xml", FileMode.Open))
            {
                planillaXML = (BindingList<PlanillaModel>)xml.Deserialize(fileStream);
            }
            return planillaXML;
        }

        private void Actualizar(object sender, ListChangedEventArgs e)
        {
            ActualizarModelo();
        }

        private void cbSucursalKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                cbSucursalDropDownClosed(sender, e);
            }
        }

        private void cbSucursalDropDownClosed(object sender, System.EventArgs e)
        {
            if (!cbSucursal.Items.Contains(cbSucursal.Text)) return;
            if (!string.IsNullOrWhiteSpace(cbSucursal.Text)) pm = pc.getEmpleados(cbSucursal.Text);
            else pm = pc.getEmpleados();
            ActualizarModelo();
            lbNumero.Content = pm.Count;
        }

        private void miEditarEmpleadoClick(object sender, RoutedEventArgs e)
        {
            Empleados.idEdit = ((PlanillaModel)dgvPlanilla.SelectedItems[0]).IdEmpleado;
            Empleados.cargarEdit = true;
        }

        private void ActualizarModelo()
        {
            dgvPlanilla.ItemsSource = null;
            dgvAtenciones.ItemsSource = null;
            dgvPlanilla.ItemsSource = pm;
            dgvAtenciones.ItemsSource = pm;
        }

        private void btGuardar_Click(object sender, RoutedEventArgs e)
        {
           
            if (pm != null)
            { 
                XmlSerializer xml = new XmlSerializer(typeof(BindingList<PlanillaModel>));
                using (StreamWriter sw = new StreamWriter("planilla.xml"))
                {
                    using (XmlWriter writer = XmlWriter.Create(sw)) 
                    {
                        xml.Serialize(writer, pm);
                    }
                }
            }
        }
    }
}