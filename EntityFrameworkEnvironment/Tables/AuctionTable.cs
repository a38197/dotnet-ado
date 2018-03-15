using BidSoftware.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidSoftware.EntityFrameworkEnvironment.Tables
{
    class AuctionTable
    {
        internal static IEnumerable<Shared.DTODefinition.IDtoObject> GetTableAuctionPage(int startRecord, int numRecords)
        {
            try
            {
                LinkedList<BidSoftware.Shared.DTODefinition.Auction> li = new LinkedList<BidSoftware.Shared.DTODefinition.Auction>();
                BidSoftware.Shared.DTODefinition.Auction auc;
                using (var ctx = new SI2_Entities())
                {
                    foreach (var auction in ctx.vAuctions.Where(p => (p.ROW_NR >= startRecord) && (p.ROW_NR <= startRecord+numRecords)))
                    {
                        auc = new BidSoftware.Shared.DTODefinition.Auction();
                        auc.SaleId = auction.SaleId;
                        auc.StartDate = auction.StartDate;
                        auc.EndDate = auction.EndDate;
                        auc.Location = auction.Location;
                        auc.ItemId = auction.ItemId;
                        auc.SaleValue = auction.SaleValue;
                        auc.MinIncrement = auction.MinIncrement;
                        li.AddLast(auc);
                    }
                }
                return li;
            }
            catch (Exception ex) { throw new DisconnectException(ex.Message, ex); }
        }

        internal static void AddAuctionRecord(Shared.DTODefinition.IDtoObject record)
        {
            using (var ctx = new SI2_Entities())
            {
                BidSoftware.Shared.DTODefinition.Auction rec = (BidSoftware.Shared.DTODefinition.Auction)record;
                ctx.sp_addAuctionSale(rec.ItemId, rec.SaleValue, rec.StartDate, rec.EndDate, rec.Location, rec.CountryNum, rec.MinIncrement);
            }  
        }

        internal static void UpdateAuctionRecord(Shared.DTODefinition.IDtoObject record)
        {
            BidSoftware.Shared.DTODefinition.Auction a = (BidSoftware.Shared.DTODefinition.Auction)record;
            using (var ctx = new SI2_Entities())
            {
                a = (BidSoftware.Shared.DTODefinition.Auction)record;
                ctx.sp_editAuctionSale(a.SaleId, a.SaleValue, a.StartDate, a.EndDate, a.Location, a.MinIncrement, a.ItemId, a.CountryNum);
            }
        }

        internal static void DeleteAuctionRecord(Shared.DTODefinition.IDtoObject record)
        {
            using (var ctx = new SI2_Entities())
            {
                BidSoftware.Shared.DTODefinition.Auction a = (BidSoftware.Shared.DTODefinition.Auction)record;
                ctx.sp_deleteAuctionSale(a.SaleId);
            }
        }

        internal static Shared.DTODefinition.IDtoObject GetAuctionRecord(object[] keys)
        {
            Shared.DTODefinition.Auction auction;
            using (var ctx = new SI2_Entities())
            {
                auction = new Shared.DTODefinition.Auction();
                int auctionid = (int)keys[0];
                var auxauction = ctx.vAuctions.Where(p => p.SaleId == auctionid).SingleOrDefault();
                auction.SaleId = auxauction.SaleId;
                auction.StartDate = auxauction.StartDate;
                auction.EndDate = auxauction.EndDate;
                auction.Location = auxauction.Location;
                auction.ItemId = auxauction.ItemId;
                auction.SaleValue = auxauction.SaleValue;
                auction.MinIncrement = auxauction.MinIncrement;
            }
            return auction;
        }
    }
}
