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
    internal class BidTableAdapter : TableAdapterBase<Bid>
    {
        public BidTableAdapter(SqlConnection connection) : base(connection) { }

        public override string TableName
        {
            get
            {
                return "vBid";
            }
        }

        protected override void addRecord(Bid record)
        {
            using(SqlCommand command = new SqlCommand())
            {
                formatCommandForProcedure(
                    command,
                    Connection,
                    "sp_addBid",

                    getParameter("saleId", System.Data.SqlDbType.Int, record.SaleId),
                    getParameter("buyerEmail", System.Data.SqlDbType.VarChar, record.UserEmail),
                    getParameter("value", System.Data.SqlDbType.Decimal, record.Value)
                );
                command.ExecuteNonQuery();
            }
        }

        

        protected override Bid mapItem(SqlDataReader reader)
        {
            Bid record = new Bid();
            record.Value = (decimal)reader["Value"];
            record.UserEmail = (string)reader["UserEmail"];
            record.Stamp = (DateTime)reader["Stamp"];
            record.SaleId = (int)reader["SaleId"];
            record.Deleted = (bool)reader["Deleted"];
            record.BidId = (int)reader["BidId"];
            return record;
        }

        protected override void deleteRecord(Bid record)
        {
            using (SqlCommand command = new SqlCommand())
            {
                formatCommandForProcedure(
                    command,
                    Connection,
                    "sp_deleteBid",

                    getParameter("saleId", System.Data.SqlDbType.Int, record.SaleId),
                    getParameter("userMail", System.Data.SqlDbType.VarChar, record.UserEmail)
                );
                command.ExecuteNonQuery();
            }
        }

        protected override void updateRecord(Bid record)
        {
            throw new InvalidOperationException("Bid update not allowed");
        }

        protected override ConditionGroup getKeysConditionGroup(object[] keys)
        {
            return new ConditionGroup(
                    new Condition(
                        new NameValuePair("BidId", keys[0].ToString(), NameValuePair.Operator.Equals))
                    );
        }
    }
}
