using System.Windows.Controls;
using System;
using System.Collections.Generic;
using zkemkeeper;

namespace BanBan.Pages
{

    public class UserInfo
    {

        public int MachineNumber { get; set; }
        public string EnrollNumber { get; set; }
        public string Name { get; set; }
        public int FingerIndex { get; set; }
        public string TmpData { get; set; }
        public int Privelage { get; set; }
        public string Password { get; set; }
        public bool Enabled { get; set; }
        public string iFlag { get; set; }

    }
    /// <summary>
    /// Interaction logic for pruebaDatos.xaml
    /// </summary>
    public partial class pruebaDatos : Page
    {
        public const string acx_Disconnect = "Disconnected";
        public const string acx_Connect = "Conncected";

        Action<object, string> RaiseDeviceEvent;
        public pruebaDatos(Action<object, string> RaiseDeviceEvent)
        { this.RaiseDeviceEvent = RaiseDeviceEvent; }

        CZKEM objCZKEM = new CZKEM();

        public pruebaDatos()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (Connect_Net(tbIP.Text, 4370))
                {
                    lbConexion.Content = acx_Connect;
                }
            }
            catch (Exception)
            {
                lbConexion.Content = acx_Disconnect;
                throw;
            }
        }
        public bool Connect_Net(string IPAdd, int Port)
        {
            if (objCZKEM.Connect_Net(IPAdd, Port))
            {
                //65535, 32767
                if (objCZKEM.RegEvent(1, 32767))
                {
                    // [ Register your events here ]
                    // [ Go through the _IZKEMEvents_Event class for a complete list of events
                    objCZKEM.OnConnected += ObjCZKEM_OnConnected;
                    objCZKEM.OnDisConnected += objCZKEM_OnDisConnected;
                    //objCZKEM.OnEnrollFinger += ObjCZKEM_OnEnrollFinger;
                    //objCZKEM.OnFinger += ObjCZKEM_OnFinger;
                    //objCZKEM.OnAttTransactionEx += new _IZKEMEvents_OnAttTransactionExEventHandler(zkemClient_OnAttTransactionEx);
                }
                return true;
            }
            return false;
        }
        private void ObjCZKEM_OnConnected()
        {
            RaiseDeviceEvent(this, acx_Connect);
        }

        void objCZKEM_OnDisConnected()
        {
            // Implementing the Event
            RaiseDeviceEvent(this, acx_Disconnect);
        }
        public ICollection<UserInfo> GetAllUserInfo(pruebaDatos objZkeeper, int machineNumber)
        {
            string sdwEnrollNumber = string.Empty, sName = string.Empty, sPassword = string.Empty, sTmpData = string.Empty;
            int iPrivilege = 0, iTmpLength = 0, iFlag = 0, idwFingerIndex;
            bool bEnabled = false;

            ICollection<UserInfo> lstFPTemplates = new List<UserInfo>();

            objZkeeper.ReadAllUserID(machineNumber);
            objZkeeper.ReadAllTemplate(machineNumber);

            while (objZkeeper.SSR_GetAllUserInfo(machineNumber, out sdwEnrollNumber, out sName, out sPassword, out iPrivilege, out bEnabled))
            {
                for (idwFingerIndex = 0; idwFingerIndex < 10; idwFingerIndex++)
                {
                    if (objZkeeper.GetUserTmpExStr(machineNumber, sdwEnrollNumber, idwFingerIndex, out iFlag, out sTmpData, out iTmpLength))
                    {
                        UserInfo fpInfo = new UserInfo();
                        fpInfo.MachineNumber = machineNumber;
                        fpInfo.EnrollNumber = sdwEnrollNumber;
                        fpInfo.Name = sName;
                        fpInfo.FingerIndex = idwFingerIndex;
                        fpInfo.TmpData = sTmpData;
                        fpInfo.Privelage = iPrivilege;
                        fpInfo.Password = sPassword;
                        fpInfo.Enabled = bEnabled;
                        fpInfo.iFlag = iFlag.ToString();

                        lstFPTemplates.Add(fpInfo);
                    }
                }

            }
            return lstFPTemplates;
        }

        public bool ReadAllUserID(int dwMachineNumber)
        {
            return objCZKEM.ReadAllUserID(dwMachineNumber);
        }
        public bool ReadAllTemplate(int dwMachineNumber)
        {
            return objCZKEM.ReadAllTemplate(dwMachineNumber);
        }
        public bool SSR_GetAllUserInfo(int dwMachineNumber, out string dwEnrollNumber, out string Name, out string Password, out int Privilege, out bool Enabled)
        {
            return objCZKEM.SSR_GetAllUserInfo(dwMachineNumber, out dwEnrollNumber, out Name, out Password, out Privilege, out Enabled);
        }
        public bool GetUserTmpExStr(int dwMachineNumber, string dwEnrollNumber, int dwFingerIndex, out int Flag, out string TmpData, out int TmpLength)
        {
            return objCZKEM.GetUserTmpExStr(dwMachineNumber, dwEnrollNumber, dwFingerIndex, out Flag, out TmpData, out TmpLength);
        }

        private void btGetDatos_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {               
                ICollection<UserInfo> lstFingerPrintTemplates = GetAllUserInfo(this, 1);
                if (lstFingerPrintTemplates != null && lstFingerPrintTemplates.Count > 0)
                {
                    tbDatos.ItemsSource = lstFingerPrintTemplates;
                }
                else
                    lbConexion.Content = "fallo";
            }
            catch (Exception ex)
            {
                lbConexion.Content = (ex.Message);
            }
        }
    }
}
