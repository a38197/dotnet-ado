using BidSoftware.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidSoftware.EntityFrameworkEnvironment.Tables
{
    class SaleTable
    {
        internal static IEnumerable<Shared.DTODefinition.IDtoObject> GetTableSalePage(int startRecord, int numRecords)
        {
            try
            {
                LinkedList<BidSoftware.Shared.DTODefinition.Sale> li = new LinkedList<BidSoftware.Shared.DTODefinition.Sale>();
                BidSoftware.Shared.DTODefinition.Sale s;
                using (var ctx = new SI2_Entities())
                {
                    foreach (var sale in ctx.vSales.Where(p => (p.ROW_NR >= startRecord) && (p.ROW_NR <= startRecord+numRecords)))
                    {
                        s = new BidSoftware.Shared.DTODefinition.Sale();
                        s.SaleId = sale.SaleId;
                        s.StartDate = sale.StartDate;
                        s.EndDate = sale.EndDate;
                        s.Location = sale.Location;
                        s.CountryNum = Convert.ToInt32(sale.CountryNum);
                        s.ItemId = sale.ItemId;
                        s.SaleValue = sale.SaleValue;
                        li.AddLast(s);
                    }
                }
                return li;
            }
            catch (Exception ex) { throw new DisconnectException(ex.Message, ex); }
        }

        internal static void AddSaleRecord(Shared.DTODefinition.IDtoObject record)
        {
            using (var ctx = new SI2_Entities())
            {
                BidSoftware.Shared.DTODefinition.Sale rec = (BidSoftware.Shared.DTODefinition.Sale)record;
                ctx.sp_addSale(rec.ItemId, rec.SaleValue, rec.StartDate, rec.EndDate, rec.Location, rec.CountryNum);
            }
        }

        internal static void UpdateSaleRecord(Shared.DTODefinition.IDtoObject record)
        {
            BidSoftware.Shared.DTODefinition.Sale s = (BidSoftware.Shared.DTODefinition.Sale)record;
            using (var ctx = new SI2_Entities())
            {
                s = (BidSoftware.Shared.DTODefinition.Sale)record;
                ctx.sp_editSale(s.SaleId, s.SaleValue, s.StartDate, s.EndDate, s.Location, s.CountryNum, s.ItemId);
            }
        }

        internal static void DeleteSaleRecord(Shared.DTODefinition.IDtoObject record)
        {
            using (var ctx = new SI2_Entities())
            {
                BidSoftware.Shared.DTODefinition.Sale s = (BidSoftware.Shared.DTODefinition.Sale)record;
                ctx.sp_deleteAuctionSale(s.SaleId);
            }
        }

        internal static Shared.DTODefinition.IDtoObject GetSaleRecord(object[] keys)
        {
            Shared.DTODefinition.Sale sale;
            using (var ctx = new SI2_Entities())
            {
                sale = new Shared.DTODefinition.Sale();
                int saleid = (int)keys[0];
                var auxsale = ctx.vSales.Where(p => p.SaleId == saleid).SingleOrDefault();
                sale.SaleId = auxsale.SaleId;
                sale.StartDate = auxsale.StartDate;
                sale.EndDate = auxsale.EndDate;
                sale.Location = auxsale.Location;
                sale.CountryNum = Convert.ToInt32(auxsale.CountryNum);
                sale.ItemId = auxsale.ItemId;
                sale.SaleValue = auxsale.SaleValue;
            }
            return sale;
        }
    }
}
