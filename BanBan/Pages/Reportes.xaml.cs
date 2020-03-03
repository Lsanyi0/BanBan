using BanBan.Model;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Printing;
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
using BanBan.Controls;

namespace BanBan.Pages
{
    /// <summary>
    /// Interaction logic for Reportes.xaml
    /// </summary>
    public partial class Reportes : Page
    {
        sBanBan sb = new sBanBan();
        public int reporteV = 0;

        public Reportes()
        {
            InitializeComponent();
            System.Drawing.Printing.PageSettings pg = new System.Drawing.Printing.PageSettings();
            pg.Margins.Top = 0;
            pg.Margins.Bottom = 0;
            pg.Margins.Left = 0;
            pg.Margins.Right = 0;
            System.Drawing.Printing.PaperSize size = new System.Drawing.Printing.PaperSize();
            size.RawKind = (int)PaperKind.B5;
            System.Drawing.Printing.PageSettings orient = new System.Drawing.Printing.PageSettings();
            pg.PaperSize = size;
            Reporte.SetPageSettings(pg);
            this.Reporte.RefreshReport();
            Reporte.Reset();
            var suc = (from sc in sb.sucursal
                      select sc.sucursal1).ToList();
            foreach (var item in suc)
            {
                listBox.Items.Add(item);
            }
            var car = (from cr in sb.cargo
                       select cr.cargo1).ToList();
            
            cbCargo.Items.Add("<<ninguno>>");
            foreach (var item in car)
            {
                cbCargo.Items.Add(item);
            }
            cbCargo.SelectedIndex = 0;
            
        }
        
        private void btnEmpleados_Click(object sender, RoutedEventArgs e)
        {
            reporteV = 2;
            int cb = cbCargo.SelectedIndex;
            List<string> listaS = new List<string>();
            if (listBox.Items.Count > 0)
            {
                foreach (var item in listBox.SelectedItems)
                {
                    listaS.Add(item.ToString());
                }
            }

            reporte(llenadoL(listaS, cb));

        }

        private void btnDescuento_Click(object sender, RoutedEventArgs e)
        {
            reporteV = 3;
        }

        private void btnPlanilla_Click(object sender, RoutedEventArgs e)
        {
            reporteV = 1;
            int cb = cbCargo.SelectedIndex;
            List<string> listaS = new List<string>();
            if (listBox.Items.Count > 0)
            {
                foreach (var item in listBox.SelectedItems)
                {
                    listaS.Add(item.ToString());
                }
            }

            reporte(llenadoL(listaS, cb));
        }

        public List<empleado> llenadoL(List<string> listaS, int cb)
        {
            if (listaS.Count()>0)
            {
                List<empleado> lista = new List<empleado>();
                List<empleado> empleados = new List<empleado>();
                foreach (var item in listaS)
                {
                    if (cb==0)
                    {
                        empleados = (from em in sb.empleado.Include("sistemapension")
                                     join tb in sb.trabajo on em.idEmpleado equals tb.idEmpleado
                                     join sc in sb.sucursal on tb.idSucursal equals sc.idSucursal
                                     where em.idCargo != 2 && sc.sucursal1 == item
                                     select em
                             ).ToList();
                        lista.AddRange(empleados);
                    }
                    else
                    {
                        empleados = (from em in sb.empleado.Include("sistemapension")
                                     join tb in sb.trabajo on em.idEmpleado equals tb.idEmpleado
                                     join sc in sb.sucursal on tb.idSucursal equals sc.idSucursal
                                     where em.idCargo ==cb && em.idCargo != 2 && sc.sucursal1 == item
                                     select em
                            ).ToList();
                        lista.AddRange(empleados);
                    }
                    
                }
                return lista;
            }
            else
            {
                List<empleado> lista = new List<empleado>();
                List<empleado> empleados = new List<empleado>();
                foreach (var item in listaS)
                {
                    if (cb == 0)
                    {
                        empleados = (from em in sb.empleado.Include("sistemapension")
                                     join tb in sb.trabajo on em.idEmpleado equals tb.idEmpleado
                                     join sc in sb.sucursal on tb.idSucursal equals sc.idSucursal
                                     where em.idCargo != 2
                                     select em
                             ).ToList();
                        lista.AddRange(empleados);
                    }
                    else
                    {
                        empleados = (from em in sb.empleado.Include("sistemapension")
                                     join tb in sb.trabajo on em.idEmpleado equals tb.idEmpleado
                                     join sc in sb.sucursal on tb.idSucursal equals sc.idSucursal
                                     where em.idCargo == cb 
                                     select em
                            ).ToList();
                        lista.AddRange(empleados);
                    }
                }
                return lista;
            }
            
            
        }

        public void reporte(List<empleado> empleados)
        {
            string report = "";
            switch (reporteV)
            {
                case 1:
                    report = "BanBan.ReportEmp.rdlc";
                    break;
                case 2:
                    report = "BanBan.ReportAtenciones.rdlc";
                    break;
                case 3:
                    report = "BanBan.ReportEmp.rdlc";
                    break;
                default:
                    break;
            }
            Reporte.Reset();
            DataTable dt = new DataTable();
            dt = ConvertToDataTable(getPlanillaModels(empleados));
            ReportDataSource ds = new ReportDataSource("DataSet1", dt);
            Reporte.LocalReport.DataSources.Add(ds);
            Reporte.LocalReport.ReportEmbeddedResource = report;
            Reporte.RefreshReport();
        }
        private List<PlanillaModel> getPlanillaDModels(List<empleado> empleados)
        {
           
          

            return new List<PlanillaModel>();
        }
        private List<PlanillaModel> getPlanillaModels(List<empleado> empleados)
        {
            DateTime inicio=dpInicio.SelectedDate.Value.Date;
            DateTime fin=dpFinal.SelectedDate.Value.Date;
            PlanillasControl pc = new PlanillasControl();

            if (empleados != null)
            {
                List<PlanillaModel> planillaModels = new List<PlanillaModel>();
                foreach (empleado empleado in empleados)
                {
                    PlanillaModel pm = new PlanillaModel()
                    {
                        IdEmpleado = empleado.idEmpleado,
                        Nombre = empleado.nombre,
                        Apellido = empleado.apellido,
                        SueldoBase = empleado.sueldo ?? 0,
                        AFPEmpleado = empleado.sistemapension.descuento,
                        PorcentajeCargo = empleado.cargo.atenciones ?? 0,
                    };
                    List<DateTime> Inicio = (from pln in sb.planillahorario 
                                              join plnp in sb.planilla on pln.idPlanilla 
                                              equals plnp.idPlanilla 
                                              where (plnp.fecha >= inicio.Date && plnp.fecha <= fin.Date)
                                              && pln.idEmpleado.Equals(pm.IdEmpleado) 
                                              select pln.entrada).ToList();
                    List<DateTime> Fin = (from pln in sb.planillahorario
                                           join plnp in sb.planilla on pln.idPlanilla
                                           equals plnp.idPlanilla
                                           where (plnp.fecha >= inicio.Date && plnp.fecha <= fin.Date)
                                           && pln.idEmpleado.Equals(pm.IdEmpleado) 
                                           select pln.salida).ToList();
                    pm.Horas = pc.GetHorasTrabajadas(Inicio, Fin);
                    pm.Descuentos ="";
                    planillaModels.Add(pm);
                }
                return planillaModels;
            }
            return new List<PlanillaModel>();
        }


        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
        private bool Between(DateTime FechaAComparar, DateTime? FechaIncial, DateTime? FechaFinal)
        {
            return (FechaAComparar >= FechaIncial && FechaAComparar <= FechaFinal);
        }

        
    }
}
