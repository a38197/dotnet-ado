using BidSoftware.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidSoftware.EntityFrameworkEnvironment.Tables
{
    class BidTable
    {
        internal static IEnumerable<Shared.DTODefinition.IDtoObject> GetTableBidPage(int startRecord, int numRecords)
        {
            try
            {
                LinkedList<BidSoftware.Shared.DTODefinition.Bid> li = new LinkedList<BidSoftware.Shared.DTODefinition.Bid>();
                BidSoftware.Shared.DTODefinition.Bid b;
                using (var ctx = new SI2_Entities())
                {
                    foreach (var bid in ctx.vBids.Where(p => (p.ROW_NR >= startRecord) && (p.ROW_NR <= startRecord+numRecords)))
                    {
                        b = new BidSoftware.Shared.DTODefinition.Bid();
                        b.BidId = bid.BidId;
                        b.Deleted = bid.Deleted;
                        b.SaleId = bid.SaleId;
                        b.Stamp = b.Stamp;
                        b.UserEmail = b.UserEmail;
                        b.Value = b.Value;
                        li.AddLast(b);
                    }
                }
                return li;
            }
            catch (Exception ex) { throw new DisconnectException(ex.Message, ex); }
        }

        internal static Shared.DTODefinition.IDtoObject GetBidRecord(object[] keys)
        {
            Shared.DTODefinition.Bid bid;
            using (var ctx = new SI2_Entities())
            {
                bid = new Shared.DTODefinition.Bid();
                int bidid = (int)keys[0];
                var auxbid = ctx.Bids.Where(p => p.BidId == bidid).SingleOrDefault();
                bid.BidId = auxbid.BidId;
                bid.Deleted = auxbid.Deleted;
                bid.SaleId = auxbid.SaleId;
                bid.Stamp = auxbid.Stamp;
                bid.UserEmail = auxbid.UserEmail;
                bid.Value = auxbid.Value;
            }
            return bid;
        }
    }
}
