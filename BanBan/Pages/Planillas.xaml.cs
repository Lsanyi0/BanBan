﻿using BanBan.Controls;
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
using System;
using System.Windows.Input;

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
        private BindingList<PlanillaModel> pmFiltro;
        private const string file = "planilla.xml";
        private bool loading = true;
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
            loading = false;
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
            //dgvEditar.ItemsSource = planillas;
            lbNumero.Content = planillas.Count();
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
            if (DialogResult.No == System.Windows.Forms.MessageBox.Show("Los datos seran guardados en la base de datos, listos para ser utilizados en reportes y no podran ser modificados. ¿Desea continuar?", "Terminar revision", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                return;
            }
            int idPlanilla = pc.GetNewIdPlanilla();
            pc.GuardarPlanillas(pc.GetPlanillasAGuardar(idPlanilla, new List<PlanillaModel>(pm)));
            foreach (var planillaModel in pm)
            {
                pc.GuardarHorarioExtra(idPlanilla, planillaModel);
            }
            pc.SaveChanges();
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
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                List<EmpleadoModel> empleados = new List<EmpleadoModel>();
                foreach (var empleado in pc.ObtenerEmpleados().ToList())
                {
                    empleados.Add(new EmpleadoModel(empleado));
                }

                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                HorasExtraModel.Load = true;
                foreach (string file in ofd.FileNames)
                {
                    //Crypto.Encrypt(file, file.Substring(0,file.Length-1));
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
                                PlanillaEmpleadoModel planilla = new PlanillaEmpleadoModel();
                                planilla.entrada = Marcacion.Entrada;
                                planilla.salida = Marcacion.Salida;
                                empleado.planillasHorario.Add(planilla);
                            }
                        }
                        foreach (var HorasExtra in ds.HorasExtra)
                        {
                            if (empleado.idEmpleado == HorasExtra.IdEmpleado)
                            {
                                if (ObtenerTipoDeHora(HorasExtra) != 0)
                                {
                                    HorarioExtra hex = new HorarioExtra();

                                    hex.idTipoHora = pc.GetTipohora(ObtenerTipoDeHora(HorasExtra)).idTipoHora;
                                    hex.comentarios = HorasExtra.Comentario;
                                    DateTime Inicio = HorasExtra.HoraInicio;
                                    DateTime Fin = HorasExtra.HoraFinal;

                                    hex.horas = (Fin - Inicio).TotalHours;
                                    hex.fecha = new DateTime(Inicio.DayOfYear);
                                    empleado.horarioExtra.Add(hex);
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }
                empleados = empleados.Where(x => x.planillasHorario.Count > 0).OrderByDescending(x => x.nombre).ToList();
                ofd.Dispose();
                pm = pc.getPlanillaModels(empleados);
                foreach (var planilla in pm)
                {
                    planilla.PropertyChanged += ActualizarPadre;
                }
                pmInicial = pm;
                ActualizarModelo(pm);
                dgvEditar.ItemsSource = pm;
                pmInicial = pm;
                lbNumero.Content = pm.Count;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
            HorasExtraModel.Load = false;
        }
        private void ActualizarPadre(object sender, PropertyChangedEventArgs args)
        {
            PlanillaModel model = (PlanillaModel)sender;
            pm = new BindingList<PlanillaModel>((from pme in pm where pme.IdEmpleado != model.IdEmpleado select pme).ToList());
            pm.Add(model);
            //ActualizarModelo(pm);
        }
        private int ObtenerTipoDeHora(HorasExtraModel extraModel)
        {
            int tipohora = 0;
            if (extraModel.HoraExtra) tipohora = 2;
            if (extraModel.HoraExtraNocturna) tipohora = 3;
            if (extraModel.HoraAsueto) tipohora = 4;
            if (extraModel.HoraDescanso) tipohora = 5;
            return tipohora;
        }

        private void btDescartarCambios_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(file)) File.Delete(file);

            pm = pmInicial;
            dgvEditar.ItemsSource = pm;
            ActualizarModelo(pm);
        }

        private void tbBuscar_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (pm.Count > 1)
            {
                if (!string.IsNullOrWhiteSpace(tbBuscar.Text) && tbBuscar.Text.Length > 3)
                {
                    FiltrarEmpleado(tbBuscar.Text.ToLower());
                }
                else
                {
                    dgvEditar.ItemsSource = pm;
                    pmFiltro = pm;
                    ActualizarModelo(pm);
                }
            }
        }
        private void FiltrarEmpleado(string empleado)
        {
            if (!string.IsNullOrWhiteSpace(empleado))
            {
                pmFiltro = new BindingList<PlanillaModel>(pm.Where(x => x.NombreCompleto.ToLower().Contains(empleado)).ToList());
                foreach (var planilla in pmFiltro)
                {
                    planilla.PropertyChanged += ActualizarPadre;
                }
                dgvEditar.ItemsSource = pmFiltro;
                ActualizarModelo(pmFiltro);
            }
        }
        private void FiltroSucursal(string sucursal)
        {
            if (!string.IsNullOrWhiteSpace(sucursal) && !loading)
            {
                int idsucursal = pc.GetIdSucursalByNombre(sucursal);
                pmFiltro = new BindingList<PlanillaModel>();
                foreach (var planillaModel in pm)
                {
                    if (pc.EmpleadoInSucursal(planillaModel.IdEmpleado, idsucursal))
                    {
                        pmFiltro.Add(planillaModel);
                    }
                }
                foreach (var planilla in pmFiltro)
                {
                    planilla.PropertyChanged += ActualizarPadre;
                }
                dgvEditar.ItemsSource = pmFiltro;
                ActualizarModelo(pmFiltro);
            }
        }

        private void cbSucursal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!loading)
            {
                if (cbSucursal.SelectedValue != null)
                {
                    FiltroSucursal(cbSucursal.SelectedValue.ToString());
                }
                else
                {
                    dgvEditar.ItemsSource = pm;
                    pmFiltro = pm;
                    ActualizarModelo(pm);
                }
            }
        }

        private void miActualizar_Click(object sender, RoutedEventArgs e)
        {
            if (cbSucursal.SelectedValue != null)
            {
                FiltroSucursal(cbSucursal.SelectedValue.ToString());
            }
            else
            {
                dgvEditar.ItemsSource = pm;
                pmFiltro = pm;
                ActualizarModelo(pm);
            }
        }
    }
}