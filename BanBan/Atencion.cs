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
    
    public partial class atencion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public atencion()
        {
            this.atenciondetalle = new HashSet<atenciondetalle>();
        }
    
        public int idAtencion { get; set; }
        public string atencion1 { get; set; }
        public decimal montoBase { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<atenciondetalle> atenciondetalle { get; set; }
    }
}
