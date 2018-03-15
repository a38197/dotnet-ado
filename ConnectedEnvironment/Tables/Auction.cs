using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BidSoftware.Shared.DTODefinition;
using System.Data.SqlClient;
using BidSoftware.Shared;

namespace BidSoftware.ConnectedEnvironment.Tables
{
    internal class AuctionTableAdapter : TableAdapterBase<Auction>
    {
        public AuctionTableAdapter(SqlConnection connection) : base(connection) { }

        public override string TableName
        {
            get
            {
                return "vAuction";
            }
        }

        protected override void addRecord(Auction record)
        {
            using(SqlCommand command = new SqlCommand())
            {
                formatCommandForProcedure(
                    command,
                    Connection,
                    "sp_addAuctionSale",

                    getParameter("itemId", System.Data.SqlDbType.Int, record.ItemId),
                    getParameter("value", System.Data.SqlDbType.Decimal, record.SaleValue),
                    getParameter("start", System.Data.SqlDbType.DateTime, record.StartDate),
                    getParameter("end", System.Data.SqlDbType.DateTime, record.EndDate),
                    getParameter("location", System.Data.SqlDbType.VarChar, record.Location),
                    getParameter("country", System.Data.SqlDbType.Int, record.CountryNum),
                    getParameter("minIncre", System.Data.SqlDbType.Money, record.MinIncrement)
                );
                command.ExecuteNonQuery();
            }
        }



        protected override Auction mapItem(SqlDataReader reader)
        {
            Auction record = new Auction();
            record.SaleId = (int)reader["SaleId"];
            record.ItemId = (int)reader["ItemId"];
            record.SaleValue = (decimal)reader["SaleValue"];
            record.StartDate = (DateTime)reader["StartDate"];
            record.EndDate = (DateTime)reader["EndDate"];
            record.Location = (string)reader["Location"];
            record.CountryNum = (int)reader["CountryNum"];
            record.MinIncrement = (decimal)reader["MinIncrement"];
            return record;            
        }

        protected override void updateRecord(Auction record)
        {
            using (SqlCommand command = new SqlCommand())
            {
                formatCommandForProcedure(
                    command,
                    Connection,
                    "sp_editAuctionSale",

                    getParameter("saleId", System.Data.SqlDbType.Int, record.SaleId),
                    getParameter("itemId", System.Data.SqlDbType.Int, record.ItemId),
                    getParameter("value", System.Data.SqlDbType.Decimal, record.SaleValue),
                    getParameter("start", System.Data.SqlDbType.DateTime, record.StartDate),
                    getParameter("end", System.Data.SqlDbType.DateTime, record.EndDate),
                    getParameter("location", System.Data.SqlDbType.VarChar, record.Location),
                    getParameter("country", System.Data.SqlDbType.Int, record.CountryNum),
                    getParameter("minIncre", System.Data.SqlDbType.Money, record.MinIncrement)
                );
                command.ExecuteNonQuery();
            }
        }

        protected override void deleteRecord(Auction record)
        {
            using (SqlCommand command = new SqlCommand())
            {
                formatCommandForProcedure(
                    command,
                    Connection,
                    "sp_deleteAuctionSale",

                    getParameter("saleId", System.Data.SqlDbType.Int, record.SaleId)
                );
                command.ExecuteNonQuery();
            }
        }

        protected override ConditionGroup getKeysConditionGroup(object[] keys)
        {
            return new ConditionGroup(
                    new Condition(
                        new NameValuePair("SaleId", keys[0].ToString(), NameValuePair.Operator.Equals))
                    );
        }
    }
}
