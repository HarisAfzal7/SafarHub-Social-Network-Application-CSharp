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
    
    public partial class FriendRequest
    {
        public int friendRequestID { get; set; }
        public string friendUsername { get; set; }
        public string toWhomeRequestUsername { get; set; }
        public Nullable<System.DateTime> requestDateAndTime { get; set; }
    
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
