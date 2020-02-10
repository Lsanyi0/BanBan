using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanBan.Model
{
    public class AtencionesPlanillaModel
    {
        // # de decimales para redondear
        protected const int Decimales = 2;
        public string Atenciones { get; set; }
        public List<decimal> Atencion { get; set; }
        public decimal TotalAtenciones { get { return decimal.Round(Atencion.Sum(), Decimales); } set { } }
        public decimal PorcentajeCargo { get; set; }
        public string Descuentos { get; set; }
        public List<decimal> Descuento { get; set; }
        public decimal TotalDescuento { get { return decimal.Round(Descuento.Sum(), Decimales); } set { } }
    }
}