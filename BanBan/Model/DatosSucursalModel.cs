using System.Collections.Generic;

namespace BanBan.Model
{
    public class DatosSucursalModel
    {
        public DatosSucursalModel()
        {
            DatosMarcacion = new List<DatosDispositivoModel>();
            HorasExtra = new List<HorasExtraModel>();
        }
        public List<DatosDispositivoModel> DatosMarcacion { get; set; }
        public List<HorasExtraModel> HorasExtra { get; set; }
    }
}
