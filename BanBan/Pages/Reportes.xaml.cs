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
        PlanillasControl pc = new PlanillasControl();

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
        }
        
        private void btnEmpleados_Click(object sender, RoutedEventArgs e)
        {
            List<empleado> lista = new List<empleado>();
            Reporte.Reset();
            List<empleado> empleados = (from em in sb.empleado.Include("sistemapension")
                                        join tb in sb.trabajo on em.idEmpleado equals tb.idEmpleado
                                        join sc in sb.sucursal on tb.idSucursal equals sc.idSucursal
                                        where em.idEmpleado >= 0 
                                        select em
                                        ).ToList();

            DataTable dt = new DataTable();
            dt = ConvertToDataTable(getPlanillaModels(empleados));
            ReportDataSource ds = new ReportDataSource("DataSet1", dt);
            Reporte.LocalReport.DataSources.Add(ds);
            Reporte.LocalReport.ReportEmbeddedResource = "BanBan.ReportEmp.rdlc";
            Reporte.RefreshReport();
        }

        private List<PlanillaModel> getPlanillaModels(List<empleado> empleados)
        {
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
                    List<DateTime?> Inicio = (from pln in sb.planillahorario where pln.idEmpleado.Equals(pm.IdEmpleado) select pln.entrada).ToList();
                    List<DateTime?> Fin = (from pln in sb.planillahorario where pln.idEmpleado.Equals(pm.IdEmpleado) select pln.salida).ToList();
                    pm.Horas = pc.getHorasTrabajadas(Inicio, Fin);
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

    }
}
