using BanBan.Controls;
using BanBan.Model;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Input;

namespace BanBan.Pages
{

    public partial class HorasExtra : Page
    {
        private DispositivoControl dc;
        private BindingList<HorasExtraModel> he;
        private BindingList<DatosDispositivoModel> ddm;
        private DatosSucursalModel dsm;
        public HorasExtraOfflineControl heoc;
        public HorasExtraControl hec;
        private const string filename = "ds.xml";

        public Action<object, string> RaiseDeviceEvent;
        public HorasExtra()
        {
            InitializeComponent();

            if (LoginControl.offline)
            {
                heoc = new HorasExtraOfflineControl();
                hec = new HorasExtraControl(heoc.CargarBDOffline());
            }
            else
            {
                hec = new HorasExtraControl();
            }
            dsm = File.Exists(filename) ? hec.CargarDatos() : new DatosSucursalModel();
            he = new BindingList<HorasExtraModel>(dsm.HorasExtra);
            ddm = new BindingList<DatosDispositivoModel>(dsm.DatosMarcacion);

            dgvPlanilla.ItemsSource = he;
            dgvHorasDispositivo.ItemsSource = ddm;

            cbEmpleado.ItemsSource = hec.GetCBEmplados();
            cbEmpleado.SelectedIndex = cbEmpleado.HasItems ? 0 : -1;

            cbSucursal.ItemsSource = hec.GetCBSucursales();
            cbSucursal.SelectedIndex = cbSucursal.HasItems ? 0 : -1;

            dpDesde.SelectedDate = DateTime.Now.AddDays(-15);
            dpHasta.SelectedDate = DateTime.Now;
            dpDesdeHorasExtra.SelectedDate = DateTime.Now.AddDays(-15);
            dpHastaHorasExtra.SelectedDate = DateTime.Now;
            dpAgregar.SelectedDate = DateTime.Now;

            dc = new DispositivoControl(RaiseDeviceEvent);
        }

        private void btCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxResult.Yes == MessageBox.Show("Se descartaran todos los cambios realizados, Continuar?", "Borrar datos", MessageBoxButton.YesNo, MessageBoxImage.Question))
            {
                he.Clear();
                ddm.Clear();
            }
            //dgvPlanilla.ItemsSource = he;
        }

        private void btAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (!dpAgregar.SelectedDate.HasValue || cbEmpleado.SelectedItem == null)
            {
                MessageBox.Show("Fecha a agregar vacia o empleado no selecionado", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            HorasExtraModel emp = new HorasExtraModel
            {
                Nombre = cbEmpleado.Text,
                HoraInicio = dpAgregar.SelectedDate.Value.AddHours(DateTime.Now.Hour),
                HoraFinal = dpAgregar.SelectedDate.Value.AddHours(DateTime.Now.Hour + 2)
            };
            if (he.Count == 0)
            {
                emp.IdHe = 1;
            }
            else
            {
                emp.IdHe = he.Select(x => x.IdHe).Last() + 1;
            }
            emp.IdEmpleado = hec.GetIdEmpleadoByNombre(cbEmpleado.Text);
            emp.Sucursal = hec.GetSucursalbyIdEmpleado(emp.IdEmpleado);
            he.Add(emp);
            dgvPlanilla.ItemsSource = he;
        }
        private void ActualizarPadre(object sender, PropertyChangedEventArgs args)
        {
            HorasExtraModel model = (HorasExtraModel)sender;
            he = (BindingList<HorasExtraModel>)he.Where(x => x.IdHe != model.IdHe);
            he.Add(model);
        }
        private void ActualizarPadreDDM(object sender, PropertyChangedEventArgs args)
        {
            DatosDispositivoModel model = (DatosDispositivoModel)sender;
            ddm = (BindingList<DatosDispositivoModel>)ddm.Where(x => x.idDDM != model.idDDM);
            ddm.Add(model);
        }
        private void btObtenerDatos_Click(object sender, RoutedEventArgs e)
        {
            heoc = new HorasExtraOfflineControl();
            heoc.CargarDB();
        }

        private void btGuardar_Click(object sender, RoutedEventArgs e)
        {
            dsm.DatosMarcacion = new List<DatosDispositivoModel>(ddm);
            dsm.HorasExtra = new List<HorasExtraModel>(he);
            hec.GuardarDatos(dsm);
        }

        private void EnviarDatos(object sender, RoutedEventArgs e)
        {
            if (!dpHasta.SelectedDate.HasValue || !dpDesde.SelectedDate.HasValue || !dpDesdeHorasExtra.SelectedDate.HasValue || !dpHastaHorasExtra.SelectedDate.HasValue)
            {
                MessageBox.Show("Una de las fechas se encuentra vacía, porfavor asegurese que se todas las fechas se encuentran seleccionadas", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (dpDesde.SelectedDate <= dpHasta.SelectedDate)
            {
                //if (string.IsNullOrWhiteSpace(Properties.Settings.Default.SucursalActual))
                //{
                //    MessageBox.Show("Sucursal actual no se encuentra configurada, porfavor configurar.","Alerta",MessageBoxButton.OK,MessageBoxImage.Warning);
                //}
                string[] fechas = {
                    dpDesdeHorasExtra.SelectedDate.Value.ToString("dd/MMMM/yy"),
                    dpHastaHorasExtra.SelectedDate.Value.ToString("dd/MMMM/yy"),
                    dpDesde.SelectedDate.Value.ToString("dd/MMMM/yy"),
                    dpHasta.SelectedDate.Value.ToString("dd/MMMM/yy")
                };
                var alerta = string.Format("Se enviaran los datos de horas extra de {0} a {1} y datos de dispositivo de {2} a {3}, " +
                    "se enviaran y borraran los datos en los rangos seleccionados y se conservaran los que no. Desea continuar?", fechas);
                if (MessageBoxResult.Yes == MessageBox.Show(alerta, "Enviar datos", MessageBoxButton.YesNo, MessageBoxImage.Information))
                {
                    ConsolidarDatosDispositivo(dpDesde.SelectedDate, dpHasta.SelectedDate.Value.AddHours(23.9999), dpDesdeHorasExtra.SelectedDate.Value, dpHastaHorasExtra.SelectedDate.Value.AddHours(23.9999));
                }
            }
            else { MessageBox.Show("La fechas \"Desde\" no puede ser mayor que \"Hasta\"", "Error!", 0, MessageBoxImage.Exclamation); }
        }

        public bool Connect_Net(string IPAdd, int Port)
        {
            if (dc.objCZKEM.Connect_Net(IPAdd, Port))
            {
                //65535, 32767
                if (dc.objCZKEM.RegEvent(1, 32767))
                {
                    // [ Register your events here ]
                    // [ Go through the _IZKEMEvents_Event class for a complete list of events
                    dc.objCZKEM.OnConnected += dc.ObjCZKEM_OnConnected;
                    dc.objCZKEM.OnDisConnected += dc.objCZKEM_OnDisConnected;
                    //objCZKEM.OnEnrollFinger += ObjCZKEM_OnEnrollFinger;
                    //objCZKEM.OnFinger += ObjCZKEM_OnFinger;
                    //objCZKEM.OnAttTransactionEx += new _IZKEMEvents_OnAttTransactionExEventHandler(zkemClient_OnAttTransactionEx);
                }
                return true;
            }
            return false;
        }


        public ICollection<MachineInfo> GetLogData(int machineNumber)
        {
            string dwEnrollNumber1 = "";
            int dwVerifyMode = 0;
            int dwInOutMode = 0;
            int dwYear = 0;
            int dwMonth = 0;
            int dwDay = 0;
            int dwHour = 0;
            int dwMinute = 0;
            int dwSecond = 0;
            int dwWorkCode = 0;

            ICollection<MachineInfo> lstEnrollData = new List<MachineInfo>();

            dc.objCZKEM.ReadAllGLogData(machineNumber);

            while (dc.objCZKEM.SSR_GetGeneralLogData(machineNumber, out dwEnrollNumber1, out dwVerifyMode, out dwInOutMode, out dwYear, out dwMonth, out dwDay, out dwHour, out dwMinute, out dwSecond, ref dwWorkCode))

            {
                DateTime inputDate = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);

                MachineInfo objInfo = new MachineInfo();
                objInfo.MachineNumber = machineNumber;
                objInfo.EnrollNumber = int.Parse(dwEnrollNumber1);
                objInfo.DateAndTime = inputDate;
                objInfo.InOutMode = dwInOutMode;

                lstEnrollData.Add(objInfo);
            }

            return lstEnrollData;
        }

        public void ConsolidarDatosDispositivo(DateTime? Desde, DateTime? Hasta, DateTime? DesdeHE, DateTime? HastaHE)
        {

            //List<int> idEmpleados = DatosDispositivo.GroupBy(x => x.idEmpleado).OrderBy(g => g.Key).Select(f => f.Key).ToList();
            DatosSucursalModel ds = new DatosSucursalModel
            {
                DatosMarcacion = ddm.ToList(),
                HorasExtra = he.ToList().Where(x => Between(x.HoraInicio, DesdeHE, HastaHE)).ToList()
            };
            hec.GuardarDatos(ds, "Fabrica", Desde.Value, Hasta.Value);
        }

        public List<DatosDispositivoModel> GetMarcaciones(ICollection<MachineInfo> Marcaciones, DateTime? Desde, DateTime? Hasta)
        {
            List<MachineInfo> se = new List<MachineInfo>();
            se = Marcaciones.Where(x => x.InOutMode == 0 && Between(x.DateAndTime, Desde, Hasta))
                .GroupBy(x => new { x.EnrollNumber, x.DateAndTime.Day, x.InOutMode })
                .Select(g => g.First()).ToList();

            //List<MachineInfo> ssd = new List<MachineInfo>();
            //ssd = Marcaciones.Where(x => x.InOutMode == 2 && Between(x.DateAndTime, Desde, Hasta))
            //    .GroupBy(x => new { x.EnrollNumber, x.DateAndTime.Day, x.InOutMode })
            //    .Select(g => g.Last()).ToList();

            //List<MachineInfo> sed = new List<MachineInfo>();
            //sed = Marcaciones.Where(x => x.InOutMode == 3 && Between(x.DateAndTime, Desde, Hasta))
            //    .GroupBy(x => new { x.EnrollNumber, x.DateAndTime.Day, x.InOutMode })
            //    .Select(g => g.Last()).ToList();

            List<MachineInfo> ss = new List<MachineInfo>();
            ss = Marcaciones.Where(x => x.InOutMode == 1 && Between(x.DateAndTime, Desde, Hasta))
                .GroupBy(x => new { x.EnrollNumber, x.DateAndTime.Day, x.InOutMode })
                .Select(g => g.Last()).ToList();

            List<DatosDispositivoModel> phs = new List<DatosDispositivoModel>();
            foreach (var soloEntrada in se)
            {
                foreach (var soloSalida in ss)
                {
                    bool dia = soloEntrada.DateAndTime.Day == soloSalida.DateAndTime.Day;
                    bool mes = soloEntrada.DateAndTime.Month == soloSalida.DateAndTime.Month;
                    bool anio = soloEntrada.DateAndTime.Year == soloSalida.DateAndTime.Year;
                    bool enroll = soloEntrada.EnrollNumber == soloSalida.EnrollNumber;
                    if (dia && mes && anio && enroll)
                    {
                        DatosDispositivoModel ph = new DatosDispositivoModel
                        {
                            Entrada = soloEntrada.DateAndTime,
                            Salida = soloSalida.DateAndTime,
                            idEmpleado = soloEntrada.EnrollNumber,
                        };
                        phs.Add(ph);
                    }
                }
            }
            foreach (var soloEntrada in se)
            {
                if (!phs.Where(x => x.Entrada == soloEntrada.DateAndTime).Any())
                {
                    DatosDispositivoModel ph = new DatosDispositivoModel
                    {
                        Entrada = soloEntrada.DateAndTime,
                        idEmpleado = soloEntrada.EnrollNumber
                    };
                    phs.Add(ph);
                }
            }
            foreach (var soloSalida in ss)
            {
                if (!phs.Where(x => x.Salida == soloSalida.DateAndTime).Any())
                {
                    DatosDispositivoModel ph = new DatosDispositivoModel
                    {
                        Salida = soloSalida.DateAndTime,
                        idEmpleado = soloSalida.EnrollNumber
                    };
                    phs.Add(ph);
                }
            }
            int x = 1;
            foreach (var ph in phs)
            {
                ph._NombreEmpleado = hec.GetNombreEmpleadoById(ph.idEmpleado);
                ph.idDDM = x;
                x++;
            }
            return phs.OrderBy(x => x.NombreEmpleado).ToList();
        }
        private bool Between(DateTime FechaAComparar, DateTime? FechaIncial, DateTime? FechaFinal)
        {
            return (FechaAComparar >= FechaIncial && FechaAComparar <= FechaFinal);
        }

        private void cbSucursal_KeyUp(object sender, KeyEventArgs e)
        {
            if (he.Count >= 1)
            {
                FiltroSucursal(cbSucursal.Text);
            }
        }

        private void cbSucursal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSucursal.SelectedIndex != -1 && he.Count >= 1)
            {
                FiltroSucursal(cbSucursal.Text);
            }
        }
        private void FiltroSucursal(string sucursal)
        {
            if (!string.IsNullOrWhiteSpace(sucursal))
            {
                BindingList<HorasExtraModel> hem = new BindingList<HorasExtraModel>(he.Where(x => x.Sucursal.Contains(sucursal)).ToList());
                foreach (var hex in hem)
                {
                    hex.PropertyChanged += ActualizarPadre;
                }
                dgvPlanilla.ItemsSource = hem;
            }
            else dgvPlanilla.ItemsSource = he;
        }
        private void FiltroEmpleado(string Nombre)
        {
            if (!string.IsNullOrWhiteSpace(Nombre))
            {
                BindingList<HorasExtraModel> hem = new BindingList<HorasExtraModel>(he.Where(x => x.NombreCompleto.ToLower().Contains(Nombre.ToLower())).ToList());
                foreach (var hex in hem)
                {
                    hex.PropertyChanged += ActualizarPadre;
                }
                dgvPlanilla.ItemsSource = hem;
            }
            else dgvPlanilla.ItemsSource = he;
        }
        private void FiltroEmpleadoDDM(string Nombre)
        {
            if (!string.IsNullOrWhiteSpace(Nombre))
            {
                BindingList<DatosDispositivoModel> ddms = new BindingList<DatosDispositivoModel>(ddm.Where(x => x.NombreEmpleado.ToLower().Contains(Nombre.ToLower())).ToList());
                foreach (var dm in ddms)
                {
                    dm.PropertyChanged += ActualizarPadreDDM;
                }
                dgvHorasDispositivo.ItemsSource = ddms;
            }
            else dgvHorasDispositivo.ItemsSource = ddm;
        }
        private void FiltroFecha(DateTime? fecha)
        {
            if (fecha.HasValue)
            {
                BindingList<HorasExtraModel> hem = new BindingList<HorasExtraModel>(he.Where(x => x.HoraInicio.DayOfYear == fecha.Value.Date.DayOfYear).ToList());
                foreach (var hex in hem)
                {
                    hex.PropertyChanged += ActualizarPadre;
                }
                dgvPlanilla.ItemsSource = hem;
            }
            else dgvPlanilla.ItemsSource = he;
        }
        private void FiltroFechaDDM(DateTime? fecha)
        {
            if (fecha.HasValue)
            {
                BindingList<DatosDispositivoModel> ddms = new BindingList<DatosDispositivoModel>(ddm.Where(x => x.Entrada.DayOfYear == fecha.Value.Date.DayOfYear).ToList());
                foreach (var dm in ddms)
                {
                    dm.PropertyChanged += ActualizarPadreDDM;
                }
                dgvHorasDispositivo.ItemsSource = ddms;
            }
            else dgvHorasDispositivo.ItemsSource = ddm;
        }
        private void dgvPlanilla_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Delete == e.Key)
            {
                if (MessageBoxResult.Yes == MessageBox.Show("Se borrara(n) la(s) fila(s) seleccionada(s), Continuar?", "Borrar datos", MessageBoxButton.YesNo, MessageBoxImage.Question))
                {
                    List<HorasExtraModel> heBorrar = new List<HorasExtraModel>();
                    foreach (HorasExtraModel dg in dgvPlanilla.SelectedItems)
                    {
                        heBorrar.Add(dg);
                    }
                    foreach (var borrar in heBorrar)
                    {
                        he.Remove(borrar);
                    }
                    dgvPlanilla.ItemsSource = he;
                }
            }
        }

        private void tbBuscar_KeyUp(object sender, KeyEventArgs e)
        {
            if (he.Count >= 1)
            {
                FiltroEmpleado(tbBuscar.Text);
            }
            if (ddm.Count >= 1)
            {
                FiltroEmpleadoDDM(tbBuscar.Text);
            }
        }

        private void btFiltrarFecha_Click(object sender, RoutedEventArgs e)
        {
            if (he.Count >= 1)
            {
                if (!dpAgregar.SelectedDate.HasValue)
                {
                    MessageBox.Show("Fecha de filtro vacia", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                FiltroFecha(dpAgregar.SelectedDate);
            }
            if (ddm.Count >= 1)
            {
                if (!dpAgregar.SelectedDate.HasValue)
                {
                    MessageBox.Show("Fecha de filtro vacia", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                FiltroFechaDDM(dpAgregar.SelectedDate);
            }
        }

        private void btObtenterDatos_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (!dpHasta.SelectedDate.HasValue || !dpDesde.SelectedDate.HasValue)
            {
                return;
            }
            try
            {
                var x = Connect_Net("192.168.0.50", 4370);
                if (!x)
                {
                    MessageBox.Show("La conexion con el dispositivo ha fallado", "Atencion"); return;
                }
                ICollection<MachineInfo> Horario = GetLogData(1);
                if (dpDesde.SelectedDate <= dpHasta.SelectedDate)
                {
                    List<DatosDispositivoModel> DatosDispositivo = GetMarcaciones(Horario, dpDesde.SelectedDate, dpHasta.SelectedDate.Value.AddHours(23.9999));
                    ddm = AgregarRegistros(DatosDispositivo, new List<DatosDispositivoModel>(ddm));
                    ddm = new BindingList<DatosDispositivoModel>(ddm.Where(x => !string.IsNullOrWhiteSpace(x.NombreEmpleado)).ToList());
                    dgvHorasDispositivo.ItemsSource = ddm;
                    HorasDispositivo.Focus();
                    dc.objCZKEM.Disconnect();
                }
                else { MessageBox.Show("La fechas \"Desde\" no puede ser mayor que \"Hasta\"", "Error!", 0, MessageBoxImage.Exclamation); }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private BindingList<DatosDispositivoModel> AgregarRegistros(List<DatosDispositivoModel> datosDispositivo, List<DatosDispositivoModel> ddm)
        {
            List<DatosDispositivoModel> dm = new List<DatosDispositivoModel>(ddm);
            foreach (var dd in datosDispositivo)
            {
                if (dd.Entrada.DayOfYear == dd.Salida.DayOfYear)
                {
                    if (!ddm.Where(x => x.Entrada.DayOfYear == dd.Entrada.DayOfYear && x.idEmpleado == dd.idEmpleado).Any())
                    {
                        dm.Add(dd);
                        continue;
                    }
                }
                if (dd.Entrada == default)
                {
                    if (!ddm.Where(x => x.Salida.DayOfYear == dd.Salida.DayOfYear && x.idEmpleado == dd.idEmpleado).Any())
                    {
                        dm.Add(dd);
                    }
                }
                else if (dd.Salida == default)
                {
                    if (!ddm.Where(x => x.Entrada.DayOfYear == dd.Entrada.DayOfYear && x.idEmpleado == dd.idEmpleado).Any())
                    {
                        dm.Add(dd);
                    }
                }
            }
            return new BindingList<DatosDispositivoModel>(dm);
        }

        private void HorasExtraa_GotFocus(object sender, RoutedEventArgs e)
        {
            lbFiltro.Content = "Fecha a agregar o filtro:";
            btAgregar.Visibility = Visibility.Visible;
        }

        private void HorasDispositivo_GotFocus(object sender, RoutedEventArgs e)
        {
            lbFiltro.Content = "Filtro de fecha:";
            btAgregar.Visibility = Visibility.Hidden;
        }
    }
}