using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BidSoftware.Shared;
using BidSoftware.Shared.DTODefinition;
using System.Data.Entity.Core.Objects;
using BidSoftware.ConnectedEnvironment.Tables;
using BidSoftware.Shared.Export;

namespace BidSoftware.EntityFrameworkEnvironment
{
    public class EEService : IServiceOperations
    { 

        public IEnumerable<IDtoObject> GetTablePage(DatabaseTable table, int startRecord, int numRecords)
        {
            try
            {
                switch (table)
                {
                    case DatabaseTable.User: return Tables.UserTable.GetTableUserPage(startRecord, numRecords);
                    case DatabaseTable.Item: return Tables.ItemTable.GetTableItemPage(startRecord, numRecords);
                    case DatabaseTable.Auction: return Tables.AuctionTable.GetTableAuctionPage(startRecord, numRecords);
                    case DatabaseTable.Sale: return Tables.SaleTable.GetTableSalePage(startRecord, numRecords);
                    case DatabaseTable.Bid: return Tables.BidTable.GetTableBidPage(startRecord, numRecords);
                    default: return null;
                }
            }
            catch (Exception ex) { throw new DisconnectException(ex.Message, ex); }
        }

        public void AddRecord(DatabaseTable table, IDtoObject record)
        {
            try
            {
                switch (table)
                {
                    case DatabaseTable.User:
                        Tables.UserTable.AddUserRecord(record);
                        break;
                    case DatabaseTable.Item:
                        Tables.ItemTable.AddItemRecord(record);
                        break;
                    case DatabaseTable.Sale:
                        Tables.SaleTable.AddSaleRecord(record);
                        break;
                    case DatabaseTable.Auction:
                        Tables.AuctionTable.AddAuctionRecord(record);
                        break;

                    default: break;
                }
            }
            catch (Exception ex) { throw new DisconnectException(ex.Message, ex); }
        }

        public void TestConnection()
        {
            //simplesmente para testar a ligação
            try
            {
                using (var ctx = new SI2_Entities())
                {
                    var cond = ctx.ItemConditions.Where(p => p.Id == 1);
                    if (cond==null)
                        throw new DisconnectException("Não foi detectado condições de itens");
                }
               
            }
            catch (Exception ex) { throw new DisconnectException(ex.Message, ex); }
        }

        public bool IsValidUser(Credentials creds)
        {
            try
            {
                //credentials = creds;

                using (var ctx = new SI2_Entities())
                {
                    ObjectParameter result = new ObjectParameter("result", typeof(int));

                    var a = ctx.sp_validateUser(creds.Username, creds.Password, result);
                    bool re = Convert.ToBoolean(result.Value);
                    return re;
                }


            }
            catch (Exception ex) { throw new DisconnectException(ex.Message, ex); }
        }


        public void UpdateRecord(DatabaseTable table, IDtoObject record)
        {
            try
            {
                switch (table)
                {
                    case DatabaseTable.User:
                        Tables.UserTable.UpdateUserRecord(record);
                        break;
                    case DatabaseTable.Item:
                        Tables.ItemTable.UpdateItemRecord(record);
                        break;
                    case DatabaseTable.Sale:
                        Tables.SaleTable.UpdateSaleRecord(record);
                        break;
                    case DatabaseTable.Auction:
                        Tables.AuctionTable.UpdateAuctionRecord(record);
                        break;

                    default: break;
                }
            }
            catch (Exception ex) { throw new DisconnectException(ex.Message, ex); }
        }

        public void DeleteRecord(DatabaseTable table, IDtoObject record)
        {
            try
            {
                switch (table)
                {
                    case DatabaseTable.User:
                        Tables.UserTable.DeleteUserRecord(record);
                        break;
                    case DatabaseTable.Item:
                        Tables.ItemTable.DeleteItemRecord(record);
                        break;
                    case DatabaseTable.Sale:
                        Tables.SaleTable.DeleteSaleRecord(record);
                        break;
                    case DatabaseTable.Auction:
                        Tables.AuctionTable.DeleteAuctionRecord(record);
                        break;
                    default: break;
                }
            }
            catch (Exception ex) { throw new DisconnectException(ex.Message, ex); }
        }

        public void Dispose()
        {
            try
            {
                //Nothing to do in EF
            }
            catch (Exception ex) { throw new DisconnectException(ex.Message, ex); }

        }

        public IEnumerable<IRecordView> GetTable(DatabaseTableOrView table)
        {
            try
            {
                switch (table)
                {
                    case DatabaseTableOrView.ActiveAuctionsView: return GetTableActiveAuctionsPage();

                    default: return null;
                }
            }
            catch (Exception ex) { throw new DisconnectException(ex.Message, ex); }
        }

        private IEnumerable<IRecordView> GetTableActiveAuctionsPage()
        {
            try
            {
                LinkedList<BidSoftware.Shared.DTODefinition.ActiveAuction> li = new LinkedList<BidSoftware.Shared.DTODefinition.ActiveAuction>();
                BidSoftware.Shared.DTODefinition.ActiveAuction a;
                using (var ctx = new SI2_Entities())
                {
                    foreach (var auc in ctx.V_ActiveAuctions)
                    {
                        a = new BidSoftware.Shared.DTODefinition.ActiveAuction();
                        a.SaleId = auc.SaleId;
                        a.StartDate = auc.StartDate;
                        a.EndDate = auc.EndDate;
                        a.Description = auc.Description;
                        a.UserEmail = auc.UserEmail;
                        a.Value = Convert.ToDecimal(auc.Value);

                        li.AddLast(a);
                    }
                }
                return li;
            }
            catch (Exception ex) { throw new DisconnectException(ex.Message, ex); }
        }

        public void AddBid(int saleId, string userName, decimal value)
        {
            try
            {
                using (var ctx = new SI2_Entities())
                {
                    ctx.sp_addBid(saleId, userName, value);
                }
            }
            catch (Exception ex) { throw new DisconnectException(Utils.whatexception(ex).Message, ex); }
        }

        public Shared.Export.Auction ExportAuctionData(int auctionId)
        {
            List<BidSoftware.Shared.Export.Bid> lb = new List<BidSoftware.Shared.Export.Bid>();
            try
            {
                using (var ctx = new SI2_Entities())
                {
                    var auc = ctx.vAuctions.Where(a => a.SaleId == auctionId).SingleOrDefault();
                    if (auc == null)
                        throw new ArgumentException("Não existe o leilão com id " + auctionId);
                    
                    var bids = ctx.vBids.Where(b => b.SaleId == auctionId);
                    foreach (var bi in bids)
                    {
                        BidSoftware.Shared.Export.Bid newbid = new BidSoftware.Shared.Export.Bid();
                        newbid.UserId = bi.UserEmail;
                        newbid.Datetime = bi.Stamp.ToString();
                        lb.Add(newbid);
                    }
                    
                    Shared.Export.Auction auctionExport = new Shared.Export.Auction()
                    {
                        Id = "cenas entity",
                        Info = new Info() { InitialDate = auc.StartDate.ToString(), MinimumBid = auc.MinIncrement.ToString(), ReservationPrice = auc.SaleValue.ToString() },
                        Bids = new Bids()
                        {
                            Num = lb.Count().ToString(),
                            ArrayBid = lb.ToArray()
                        }
                    };
                    return auctionExport;
                }
            }
            catch (Exception ex) { throw new DisconnectException(ex.Message, ex); }

        }

        public IDtoObject GetRecord(DatabaseTable table, params object[] keys)
        {
                try
                {
                    switch (table)
                    {
                        case DatabaseTable.User: return Tables.UserTable.GetUserRecord(keys);
                        case DatabaseTable.Item: return Tables.ItemTable.GetItemRecord(keys);
                        case DatabaseTable.Auction: return Tables.AuctionTable.GetAuctionRecord(keys);
                        case DatabaseTable.Sale: return Tables.SaleTable.GetSaleRecord(keys);
                        case DatabaseTable.Bid: return Tables.BidTable.GetBidRecord(keys);
                        default: return null;
                    }
                }
                catch (Exception ex) { throw new DisconnectException(ex.Message, ex); }
            }
    }
}
