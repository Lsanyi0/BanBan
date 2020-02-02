using BanBan.Controls;
using System.Windows.Controls;
using System.Windows;
using BanBan.Model;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace BanBan.Pages
{
    /// <summary>
    /// Interaction logic for Planillas.xaml
    /// </summary>
    public partial class Planillas : Page
    {
        private PlanillasControl pc = new PlanillasControl();
        private BindingList<PlanillaModel> pm;
        private BindingList<PlanillaModel> pmInicial;
        private const string file = "planilla.xml";

        public Planillas()
        {
            InitializeComponent();

            cbSucursal.ItemsSource = pc.getSucursales();
            cbSucursal.SelectedIndex = 0;

            pm = File.Exists(file) ? pc.CargarXML(file) : new BindingList<PlanillaModel>();
            pmInicial = pm;

            dgvEditar.ItemsSource = pm;

            ActualizarModelo(pm);

            lbNumero.Content = dgvPlanilla.Items.Count;
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
            //if (!cbSucursal.Items.Contains(cbSucursal.Text)) return;
            //if (!string.IsNullOrWhiteSpace(cbSucursal.Text)) pm = pc.getEmpleados(cbSucursal.Text);
            //else pm = pc.getEmpleados();
            //ActualizarModelo();
            //lbNumero.Content = pm.Count;
        }

        private void miEditarEmpleadoClick(object sender, RoutedEventArgs e)
        {
            Empleados.idEdit = ((PlanillaModel)dgvPlanilla.SelectedItems[0]).IdEmpleado;
            Empleados.cargarEdit = true;
        }

        private void ActualizarModelo(BindingList<PlanillaModel> planillas)
        {
            dgvPlanilla.ItemsSource = planillas;
            dgvAtenciones.ItemsSource = planillas;
            dgvEditar.ItemsSource = planillas;
        }

        private void btGuardar_Click(object sender, RoutedEventArgs e)
        {

            if (pm != null)
            {
                XmlSerializer xml = new XmlSerializer(typeof(BindingList<PlanillaModel>));
                using (StreamWriter sw = new StreamWriter(file))
                {
                    using (XmlWriter writer = XmlWriter.Create(sw))
                    {
                        xml.Serialize(writer, pm);
                    }
                }
            }
        }

        private void btCerrarPlanilla_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btObtenerDatos_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Multiselect = true,
                Filter = "Sucursal (*.sc)|*.sc",
                Title = "Seleccionar archivos",
                InitialDirectory = HorasExtraOfflineControl.pathDB
            };
            List<empleado> empleados = pc.ObtenerEmpleados();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                HorasExtraModel.Load = true;
                foreach (string file in ofd.FileNames)
                {
                    Crypto.Decrypt(file, file + "x");
                    XmlSerializer xml = new XmlSerializer(typeof(DatosSucursalModel));
                    DatosSucursalModel ds = new DatosSucursalModel();

                    using (FileStream fileStream = new FileStream(file + "x", FileMode.Open))
                    {
                        ds = (DatosSucursalModel)xml.Deserialize(fileStream);
                    }
                    File.Delete(file + "x");
                    foreach (var empleado in empleados)
                    {
                        foreach (var Marcacion in ds.DatosMarcacion)
                        {
                            if (empleado.idEmpleado == Marcacion.idEmpleado)
                            {
                                empleado.planillahorario.Add(new planillahorario()
                                {
                                    entrada = Marcacion.Entrada,
                                    salida = Marcacion.Salida
                                });
                            }
                        }
                        foreach (var HorasExtra in ds.HorasExtra)
                        {
                            if (empleado.idEmpleado == HorasExtra.IdEmpleado)
                            {
                                empleado.horarioextra.Add(new horarioextra()
                                {
                                    horaInicio = HorasExtra.HoraInicio,
                                    horaFinal = HorasExtra.HoraFinal,
                                    tipohora = new tipohora()
                                    {
                                        idTipoHora = ObtenerTipoDeHora(HorasExtra)
                                    }
                                });
                            }
                        }
                    }
                }
                empleados = empleados.Where(x => x.planillahorario.Count > 0).Select(x => x).ToList();
            }
            ofd.Dispose();
            pm = pc.getPlanillaModels(empleados);
            foreach (var planilla in pm)
            {
                planilla.PropertyChanged += ActualizarPadre;
            }
            pmInicial = pm;
            ActualizarModelo(pm);
            HorasExtraModel.Load = false;
        }
        private void ActualizarPadre(object sender, PropertyChangedEventArgs args)
        {
            PlanillaModel model = (PlanillaModel)sender;
            pm = (BindingList<PlanillaModel>)pm.Where(x => x.IdEmpleado != model.IdEmpleado);
            pm.Add(model);
        }
        private int ObtenerTipoDeHora(HorasExtraModel extraModel)
        {
            int tipohora = 0;
            if (extraModel.HoraExtra) tipohora = 1;
            if (extraModel.HoraExtraNocturna) tipohora = 3;
            if (extraModel.HoraAsueto) tipohora = 4;
            if (extraModel.HoraDescanso) tipohora = 5;
            return tipohora;
        }

        private void btDescartarCambios_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(file)) File.Delete(file);

            pm = pc.getEmpleados();

            ActualizarModelo(pm);
        }

        private void tbBuscar_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (tbBuscar.Text.Length > 3)
            {

            }
        }
        private void FiltroSucursal(string sucursal)
        {
            if (!string.IsNullOrWhiteSpace(sucursal))
            {
                int idsucursal = pc.GetIdSucursalByNombre(sucursal);
                BindingList<PlanillaModel> fpm = new BindingList<PlanillaModel>(pm.Where(x => pc.EmpleadoInSucursal(x.IdEmpleado,idsucursal)).ToList());
                foreach (var planilla in fpm)
                {
                    planilla.PropertyChanged += ActualizarPadre;
                }
                dgvPlanilla.ItemsSource = fpm;
                ActualizarModelo(fpm);
            }
            else
            {
                dgvPlanilla.ItemsSource = pm;
                ActualizarModelo(pm);
            }
        }
    }
}