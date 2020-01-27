using System;
using zkemkeeper;

namespace BanBan.Controls
{

    public class DispositivoControl
    {
        public Action<object, string> RaiseDeviceEvent;
        public DispositivoControl(Action<object, string> RaiseDeviceEvent)
        { this.RaiseDeviceEvent = RaiseDeviceEvent; }

        public const string acx_Disconnect = "Disconnected";
        public const string acx_Connect = "Conncected";

        public CZKEM objCZKEM = new CZKEM();

        private void SSR_SetUserInfo(int machineNumber, string enrollNumber, string name, string password, int privilege, bool enabled)
        {
            System.Windows.MessageBox.Show("" + objCZKEM.SSR_SetUserInfo(machineNumber, enrollNumber, name, password, privilege, enabled));
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
        public void ObjCZKEM_OnConnected()
        {
            RaiseDeviceEvent(this, acx_Connect);
        }
        public void objCZKEM_OnDisConnected()
        {
            // Implementing the Event
            RaiseDeviceEvent(this, acx_Disconnect);
        }
        public bool SSR_GetGeneralLogData(int dwMachineNumber, out string dwEnrollNumber, out int dwVerifyMode, out int dwInOutMode, out int dwYear, out int dwMonth, out int dwDay, out int dwHour, out int dwMinute, out int dwSecond, ref int dwWorkCode)
        {
            return objCZKEM.SSR_GetGeneralLogData(dwMachineNumber, out dwEnrollNumber, out dwVerifyMode, out dwInOutMode, out dwYear, out dwMonth, out dwDay, out dwHour, out dwMinute, out dwSecond, ref dwWorkCode);
        }
        public bool ReadAllGLogData(int dwMachineNumber)
        {
            return objCZKEM.ReadAllGLogData(dwMachineNumber);
        }
    }
}
