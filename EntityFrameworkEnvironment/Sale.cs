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
    
    public partial class Sale
    {
        public Sale()
        {
            this.Bids = new HashSet<Bid>();
        }
    
        public int SaleId { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string Location { get; set; }
        public Nullable<int> CountryNum { get; set; }
        public int ItemId { get; set; }
        public decimal SaleValue { get; set; }
        public bool IsAuction { get; set; }
    
        public virtual AuctionSale AuctionSale { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
        public virtual Country Country { get; set; }
        public virtual Item Item { get; set; }
    }
}
