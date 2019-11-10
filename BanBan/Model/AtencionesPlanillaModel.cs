using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanBan.Model
{
    class AtencionesPlanillaModel
    {
        public string Atenciones { get; set; }
        public List<decimal> Atencion { get; set; }
        public decimal TotalAtenciones { get { return Atencion.Sum(); } set { } }
        public decimal PorcentajeCargo { get; set; }
        public string Descuentos { get; set; }
        public List<decimal> Descuento { private get; set; }
        public decimal TotalDesuento { get { return Descuento.Sum(); } set { } }
        public decimal TotalNeto
        {
            get; //modificar
            set;
        }
    }
}