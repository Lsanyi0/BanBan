using System;

namespace BanBan.Model
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
    public class MachineInfo
    {
        public int MachineNumber { get; set; }
        public int EnrollNumber { get; set; }
        public int InOutMode { get; set; }
        public DateTime DateAndTime {get; set;}
    }
}
