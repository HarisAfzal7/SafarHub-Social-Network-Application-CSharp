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
    
    public partial class PicturesLike
    {
        public int likeID { get; set; }
        public int postID { get; set; }
        public string username { get; set; }
        public Nullable<System.DateTime> clickedLikeDateTime { get; set; }
    
        public virtual Picture Picture { get; set; }
        public virtual User User { get; set; }
    }
}
