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
    
    public partial class PlanillaHorario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PlanillaHorario()
        {
            this.ComentariosPlanilla = new HashSet<ComentariosPlanilla>();
        }
    
        public int idPlanillaHorario { get; set; }
        public Nullable<System.TimeSpan> entrada { get; set; }
        public Nullable<System.TimeSpan> salida { get; set; }
        public System.DateTime fecha { get; set; }
        public string comentario { get; set; }
        public int idEmpleado { get; set; }
        public int idPlanilla { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ComentariosPlanilla> ComentariosPlanilla { get; set; }
        public virtual Empleado Empleado { get; set; }
        public virtual Planilla Planilla { get; set; }
    }
}
