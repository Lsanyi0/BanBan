//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BanBan
{
    using System;
    using System.Collections.Generic;
    
    public partial class descuentos
    {
        public int idDescuentos { get; set; }
        public string descuento { get; set; }
        public System.DateTime fecha { get; set; }
        public decimal monto { get; set; }
        public int idEmpleado { get; set; }
        public int idPlanilla { get; set; }
    
        public virtual empleado empleado { get; set; }
        public virtual planilla planilla { get; set; }
    }
}
