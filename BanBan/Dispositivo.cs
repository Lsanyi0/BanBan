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
    
    public partial class dispositivo
    {
        public string idDispositivo { get; set; }
        public string ip { get; set; }
        public int idSucursal { get; set; }
    
        public virtual sucursal sucursal { get; set; }
    }
}
