//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BidSoftware.EntityFrameworkEnvironment
{
    using System;
    using System.Collections.Generic;
    
    public partial class V_RakedBids
    {
        public int BidId { get; set; }
        public System.DateTime Stamp { get; set; }
        public int SaleId { get; set; }
        public string UserEmail { get; set; }
        public bool Deleted { get; set; }
        public decimal Value { get; set; }
        public Nullable<long> RNK_SALE { get; set; }
        public Nullable<long> RNK_DATE { get; set; }
    }
}