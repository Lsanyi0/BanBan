﻿using BanBan.Controls;
using BanBan.Model;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

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

            if (LoginControl.offline)
            {
                heoc = new HorasExtraOfflineControl();
                hec = new HorasExtraControl(heoc.CargarBDOffline());
            }
            else
            {
                hec = new HorasExtraControl();
            }
            cbEmpleado.ItemsSource = hec.GetCBEmplados();
            cbEmpleado.SelectedIndex = cbEmpleado.HasItems ? 0 : -1;

            dpDesde.SelectedDate = DateTime.Now.AddDays(-15);
            dpHasta.SelectedDate = DateTime.Now;

            he = File.Exists(filename) ? hec.CargarDatos() : new BindingList<HorasExtraModel>();
            dgvPlanilla.ItemsSource = he;
            dc = new DispositivoControl(RaiseDeviceEvent);
        }

        private void btCancelar_Click(object sender, RoutedEventArgs e)
        {
            //if (MessageBoxResult.Yes == MessageBox.Show("Test", "", MessageBoxButton.YesNo, MessageBoxImage.Question))
            //{
            //    he.Clear();
            //}
            dgvPlanilla.ItemsSource = he;
        }

        private void btAgregar_Click(object sender, RoutedEventArgs e)
        {
            he.Add(new HorasExtraModel
            {
                IdHe = he.Count + 1,
                Nombre = cbEmpleado.Text,
                Apellido = "",
                HoraInicio = DateTime.Now,
                HoraFinal = DateTime.Now.AddHours(2),
            });
        }
        private void ActualizarPadre(object sender, PropertyChangedEventArgs args)
        {
            HorasExtraModel model = (HorasExtraModel)sender;
            he = (BindingList<HorasExtraModel>)he.Where(x => x.IdHe != model.IdHe);
            he.Add(model);
        }
        private void btEnviarDatos_Click(object sender, RoutedEventArgs e)
        {
            //heoc = new HorasExtraOfflineControl();
            //heoc.CrearBDOffline();
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

        private void btfiltrar_Click(object sender, RoutedEventArgs e)
        {
            BindingList<HorasExtraModel> hem = new BindingList<HorasExtraModel>(he.Where(x => x.Comentario != "abc").ToList());
            foreach (var hex in hem)
            {
                hex.PropertyChanged += ActualizarPadre;
            }
            dgvPlanilla.ItemsSource = hem;
        }

        private void btDatosDispositivo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var x = Connect_Net("192.168.0.50", 4370);
                if (!x)
                {
                    MessageBox.Show("La conexion con el dispositivo ha fallado", "Atencion"); return;
                }
                ICollection<UserInfo> Usuarios = GetAllUserInfo(dc, 1);
                ICollection<MachineInfo> Horario = GetLogData(1);
                ConsolidarDatosDispositivo(dpDesde.SelectedDate, dpHasta.SelectedDate, Usuarios, Horario);
            }
            catch (Exception)
            {
                throw;
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

        private void btGetDatos_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    ICollection<UserInfo> lstFingerPrintTemplates = GetAllUserInfo(this, 1);
            //    if (lstFingerPrintTemplates != null && lstFingerPrintTemplates.Count > 0)
            //    {
            //        tbDatos.ItemsSource = lstFingerPrintTemplates;
            //    }
            //    else
            //        lbConexion.Content = "fallo";
            //}
            //catch (Exception ex)
            //{
            //    lbConexion.Content = (ex.Message);
            //}
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
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
            string EnrollNumber = (int.Parse((from asd in lstFingerPrintTemplates select asd.EnrollNumber).Last()) + 1).ToString();
            string Name = "xDD";
            string Password = "1153";
            int Privilege = 1;
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

        public void ConsolidarDatosDispositivo(DateTime? Desde, DateTime? Hasta, ICollection<UserInfo> Usuarios, ICollection<MachineInfo> Marcaciones)
        {

            LimpiarDatos(Marcaciones);

            List<empleado> empleados = new List<empleado>();
            foreach (var usuario in Usuarios)
            {
                empleado emp = new empleado();
                foreach (var Marcacion in Marcaciones)
                {
                    if (Between(Marcacion.DateAndTime, Desde, Hasta))
                    {
                    }
                }
                empleados.Add(emp);
            }
        }

        public ICollection<MachineInfo> LimpiarDatos(ICollection<MachineInfo> Marcaciones)
        {
            List<MachineInfo> se = Marcaciones.Where(x => x.InOutMode == 0).GroupBy(x => new {x.EnrollNumber, x.DateAndTime.Day,x.InOutMode}).Select(g => g.First()).ToList();
            List<MachineInfo> ss = Marcaciones.Where(x => x.InOutMode == 1).GroupBy(x => new { x.EnrollNumber, x.DateAndTime.Day, x.InOutMode }).Select(g => g.Last()).ToList();
            return Marcaciones;
        }

        private bool Between(DateTime FechaAComparar, DateTime? FechaIncial, DateTime? FechaFinal)
        {
            return (FechaAComparar >= FechaIncial && FechaAComparar <= FechaFinal);
        }
    }
}