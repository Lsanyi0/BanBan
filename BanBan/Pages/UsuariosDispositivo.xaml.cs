using BanBan.Controls;
using BanBan.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace BanBan.Pages
{
    /// <summary>
    /// Interaction logic for UsuariosDispositivo.xaml
    /// </summary>
    public partial class UsuariosDispositivo : Page
    {
        private string sucursal;
        private DispositivoControl dc;
        public HorasExtraOfflineControl heoc;
        public HorasExtraControl hec;

        public Action<object, string> RaiseDeviceEvent;
        public UsuariosDispositivo()
        {
            InitializeComponent();
            sucursal = Properties.Settings.Default.SucursalActual;
            if (string.IsNullOrWhiteSpace(sucursal))
            {
                lbSucursalActualB.Content = "No seleccionada";
            }
            else
            {
                lbSucursalActualB.Content = sucursal;
            }

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

            cbSucursal.ItemsSource = hec.GetCBSucursales();
            cbSucursal.SelectedIndex = cbSucursal.HasItems ? 0 : -1;

            cbNombreEnDispositivo.ItemsSource = cbEmpleado.Text.Split(' ');
            cbNombreEnDispositivo.SelectedIndex = cbNombreEnDispositivo.HasItems ? 0 : -1;

            cbNombreEnDispositivoB.ItemsSource = cbEmpleado.Text.Split(' ');
            cbNombreEnDispositivoB.SelectedIndex = cbNombreEnDispositivoB.HasItems ? 0 : -1;

            dc = new DispositivoControl(RaiseDeviceEvent);
        }
        private void btSeleccionarEmpleado_Click(object sender, RoutedEventArgs e)
        {
            if (cbEmpleado.SelectedItem != null)
            {
                IngresarUsuarioADispositivo();
            }
            else
            {
                System.Windows.MessageBox.Show("Porfavor seleccione un empleado de la lista.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void IngresarUsuarioADispositivo()
        {
            if ((cbNombreEnDispositivo.Text + " " + cbNombreEnDispositivoB.Text).Trim().Length >= 16)
            {
                System.Windows.MessageBox.Show("El nombre de usuario seleccionado debe contener un maximo de 16 caracteres, " +
                    "porfavor seleccione una nueva combinacion o abreviacion", "Error con nombre seleccionado", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            try
            {
                var x = Connect_Net("192.168.0.50", 4370);
                if (!x)
                {
                    System.Windows.MessageBox.Show("La conexion con el dispositivo ha fallado", "Atencion"); return;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            int MachineNumber = 1;
            string EnrollNumber = hec.GetIdEmpleadoByNombre(cbEmpleado.Text).ToString();
            string Name = (cbNombreEnDispositivo.Text + " " + cbNombreEnDispositivoB.Text).Trim();
            string Password = "";
            int Privilege = 0;
            bool Enabled = true;

            var res = dc.objCZKEM.SSR_SetUserInfo(MachineNumber, EnrollNumber, Name, Password, Privilege, Enabled);
            if (res)
            {
                System.Windows.MessageBox.Show("Usuario insertado/actualizado en dispositivo con el ID: " + EnrollNumber, "Insercion correcta", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show("Fallo en insercion de usuario" + EnrollNumber, "Fallo en la insercion", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        //nombre <= 16 char
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
        private void cbEmpleado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbNombreEnDispositivo.ItemsSource = cbEmpleado.Text.Split(' ');
            cbNombreEnDispositivo.SelectedIndex = cbNombreEnDispositivo.HasItems ? 0 : -1;

            cbNombreEnDispositivoB.ItemsSource = cbEmpleado.Text.Split(' ');
            cbNombreEnDispositivoB.SelectedIndex = cbNombreEnDispositivoB.HasItems ? 0 : -1;
        }

        private void btSeleccionarSucursal_Click(object sender, RoutedEventArgs e)
        {
            if (cbSucursal.SelectedItem != null)
            {
                Properties.Settings.Default.SucursalActual = cbSucursal.Text;
                Properties.Settings.Default.Save();
                lbSucursalActualB.Content = cbSucursal.Text;
                System.Windows.MessageBox.Show("Se ha guardado la sucursal seleccionada como sucursal actual", "Guardar", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show("Porfavor seleccione una sucursal de la lista", "Advertencia!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void btSeleccionarCarpeta_Click(object sender, RoutedEventArgs e)
        {
            string path = Properties.Settings.Default.Dropbox;
            FolderBrowserDialog fd = new FolderBrowserDialog();
            fd.ShowNewFolderButton = false;
            fd.Description = "Seleccione la carpeta de dropbox";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                path = fd.SelectedPath;
                Properties.Settings.Default.Dropbox = path;
                Properties.Settings.Default.Save();
                System.Windows.MessageBox.Show("Carpeta seleccionada correctamente");
            }
        }
    }
}
