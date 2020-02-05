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
        public HorasExtraOfflineControl heoc;
        public HorasExtraControl hec;
        private const string filename = "he.xml";

        public Action<object, string> RaiseDeviceEvent;
        public HorasExtra()
        {
            InitializeComponent();
            //Language = System.Windows.Markup.XmlLanguage.GetLanguage("es-ES");

            if (LoginControl.offline)
            {
                heoc = new HorasExtraOfflineControl();
                hec = new HorasExtraControl(heoc.CargarBDOffline());
            }
            else
            {
                hec = new HorasExtraControl();
            }

            he = File.Exists(filename) ? hec.CargarDatos() : new BindingList<HorasExtraModel>();
            dgvPlanilla.ItemsSource = he;

            cbEmpleado.ItemsSource = hec.GetCBEmplados();
            cbEmpleado.SelectedIndex = cbEmpleado.HasItems ? 0 : -1;

            cbSucursal.ItemsSource = hec.GetCBSucursales();
            cbSucursal.SelectedIndex = cbSucursal.HasItems ? 0 : -1;

            dpDesde.SelectedDate = DateTime.Now.AddDays(-15);
            dpHasta.SelectedDate = DateTime.Now;
            dpAgregar.SelectedDate = DateTime.Now;



            dc = new DispositivoControl(RaiseDeviceEvent);
        }

        private void btCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxResult.Yes == MessageBox.Show("Se descartaran todos los cambios realizados, Continuar?", "Borrar datos", MessageBoxButton.YesNo, MessageBoxImage.Question))
            {
                he.Clear();
            }
            //dgvPlanilla.ItemsSource = he;
        }

        private void btAgregar_Click(object sender, RoutedEventArgs e)
        {
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
        private void btObtenerDatos_Click(object sender, RoutedEventArgs e)
        {
            heoc = new HorasExtraOfflineControl();
            heoc.CargarDB();
        }

        private void btGuardar_Click(object sender, RoutedEventArgs e)
        {
            hec.GuardarDatos(he);
        }

        private void EnviarDatos(object sender, RoutedEventArgs e)
        {
            try
            {
                var x = Connect_Net("192.168.0.50", 4370);
                if (!x)
                {
                    MessageBox.Show("La conexion con el dispositivo ha fallado", "Atencion"); return;
                }
                //ICollection<UserInfo> Usuarios = GetAllUserInfo(dc, 1);
                ICollection<MachineInfo> Horario = GetLogData(1);
                if (dpDesde.SelectedDate <= dpHasta.SelectedDate)
                {
                    ConsolidarDatosDispositivo(dpDesde.SelectedDate, dpHasta.SelectedDate.Value.AddHours(23.9999), Horario);
                }
                else { MessageBox.Show("La fechas \"Desde\" no puede ser mayor que \"Hasta\"", "Error!", 0, MessageBoxImage.Exclamation); }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        public ICollection<UserInfo> GetAllUserInfo(DispositivoControl objZkeeper, int machineNumber)
        {
            string sdwEnrollNumber = string.Empty, sName = string.Empty, sPassword = string.Empty, sTmpData = string.Empty;
            int iPrivilege = 0, iTmpLength = 0, iFlag = 0, idwFingerIndex;
            bool bEnabled = false;

            ICollection<UserInfo> lstFPTemplates = new List<UserInfo>();

            dc.ReadAllUserID(machineNumber);
            dc.ReadAllTemplate(machineNumber);

            while (dc.objCZKEM.SSR_GetAllUserInfo(machineNumber, out sdwEnrollNumber, out sName, out sPassword, out iPrivilege, out bEnabled))
            {
                for (idwFingerIndex = 0; idwFingerIndex < 10; idwFingerIndex++)
                {
                    if (dc.objCZKEM.GetUserTmpExStr(machineNumber, sdwEnrollNumber, idwFingerIndex, out iFlag, out sTmpData, out iTmpLength))
                    {
                        UserInfo fpInfo = new UserInfo();
                        fpInfo.MachineNumber = machineNumber;
                        fpInfo.EnrollNumber = sdwEnrollNumber;
                        fpInfo.Name = sName;
                        fpInfo.FingerIndex = idwFingerIndex;
                        fpInfo.TmpData = sTmpData;
                        fpInfo.Privelage = iPrivilege;
                        //fpInfo.Password = sPassword;
                        fpInfo.Enabled = bEnabled;
                        fpInfo.iFlag = iFlag.ToString();

                        lstFPTemplates.Add(fpInfo);
                    }
                }

            }
            return lstFPTemplates;
        }

        private void IngresarUsuarioADispositivo()
        {
            ICollection<UserInfo> lstFingerPrintTemplates;
            try
            {
                lstFingerPrintTemplates = GetAllUserInfo(dc, 1);
            }
            catch (Exception)
            {
                throw;
            }

            int MachineNumber = 1;
            //string EnrollNumber = (int.Parse((from asd in lstFingerPrintTemplates select asd.EnrollNumber).Last()) + 1).ToString();
            string EnrollNumber = "12";
            string Name = "Luis Albanes";
            string Password = "1153";
            int Privilege = 3;
            bool Enabled = true;

            dc.objCZKEM.SSR_SetUserInfo(MachineNumber, EnrollNumber, Name, Password, Privilege, Enabled);
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
                string inputDate = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond).ToString();

                MachineInfo objInfo = new MachineInfo();
                objInfo.MachineNumber = machineNumber;
                objInfo.EnrollNumber = int.Parse(dwEnrollNumber1);
                objInfo.DateTimeRecord = inputDate;
                objInfo.InOutMode = dwInOutMode;

                lstEnrollData.Add(objInfo);
            }

            return lstEnrollData;
        }

        public void ConsolidarDatosDispositivo(DateTime? Desde, DateTime? Hasta, ICollection<MachineInfo> Marcaciones)
        {
            List<DatosDispositivoModel> DatosDispositivo = LimpiarDatos(Marcaciones, Desde, Hasta);
            //List<int> idEmpleados = DatosDispositivo.GroupBy(x => x.idEmpleado).OrderBy(g => g.Key).Select(f => f.Key).ToList();
            DatosSucursalModel ds = new DatosSucursalModel
            {
                DatosMarcacion = DatosDispositivo,
                HorasExtra = he.ToList()
            };
            hec.GuardarDatos(ds, "Fabrica", Desde.Value, Hasta.Value);
        }

        public List<DatosDispositivoModel> LimpiarDatos(ICollection<MachineInfo> Marcaciones, DateTime? Desde, DateTime? Hasta)
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
                            idEmpleado = soloEntrada.EnrollNumber
                        };
                        phs.Add(ph);
                    }
                }
            }
            return phs;
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
        private void FiltroFecha(DateTime? fecha)
        {
            if (fecha.HasValue)
            {
                BindingList<HorasExtraModel> hem = new BindingList<HorasExtraModel>(he.Where(x => x.HoraInicio.Date.DayOfYear == fecha.Value.Date.DayOfYear).ToList());
                foreach (var hex in hem)
                {
                    hex.PropertyChanged += ActualizarPadre;
                }
                dgvPlanilla.ItemsSource = hem;
            }
            else dgvPlanilla.ItemsSource = he;
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
        }

        private void btFiltrarFecha_Click(object sender, RoutedEventArgs e)
        {
            if (he.Count >= 1)
            {
                FiltroFecha(dpAgregar.SelectedDate);
            }
        }
    }
}