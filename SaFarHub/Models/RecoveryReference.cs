//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SaFarHub.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class RecoveryReference
    {
        public int recoveryID { get; set; }
        public string username { get; set; }
        public string recoveryEmail { get; set; }
        public string recoveryPhn { get; set; }
    
        public virtual User User { get; set; }
    }
}
