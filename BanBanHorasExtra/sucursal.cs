//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BanBanHorasExtra
{
    using System;
    using System.Collections.Generic;
    
    public partial class sucursal
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sucursal()
        {
            this.trabajo = new HashSet<trabajo>();
        }
    
        public int idSucursal { get; set; }
        public string sucursal1 { get; set; }
        public string direccion { get; set; }
        public int idCiudad { get; set; }
        public int idHangar { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<trabajo> trabajo { get; set; }
    }
}
