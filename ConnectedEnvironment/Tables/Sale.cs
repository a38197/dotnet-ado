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
    internal class SaleTableAdapter : TableAdapterBase<Sale>
    {
        public SaleTableAdapter(SqlConnection connection) : base(connection) { }

        public override string TableName
        {
            get
            {
                return "vSale";
            }
        }

        protected override void addRecord(Sale record)
        {
            using(SqlCommand command = new SqlCommand())
            {
                formatCommandForProcedure(
                    command,
                    Connection,
                    "sp_addSale",

                    getParameter("itemId", System.Data.SqlDbType.Int, record.ItemId),
                    getParameter("value", System.Data.SqlDbType.Decimal, record.SaleValue),
                    getParameter("start", System.Data.SqlDbType.DateTime, record.StartDate),
                    getParameter("end", System.Data.SqlDbType.DateTime, record.EndDate),
                    getParameter("location", System.Data.SqlDbType.VarChar, record.Location),
                    getParameter("country", System.Data.SqlDbType.Int, record.CountryNum)
                );
                command.ExecuteNonQuery();
            }
        }



        protected override Sale mapItem(SqlDataReader reader)
        {
            Sale record = new Sale();
            record.SaleId = (int)reader["SaleId"];
            record.ItemId = (int)reader["ItemId"];
            record.SaleValue = (decimal)reader["SaleValue"];
            record.StartDate = (DateTime)reader["StartDate"];
            record.EndDate = (DateTime)reader["EndDate"];
            record.Location = (string)reader["Location"];
            record.CountryNum = (int)reader["CountryNum"];
            return record;
        }

        protected override void updateRecord(Sale record)
        {
            using (SqlCommand command = new SqlCommand())
            {
                formatCommandForProcedure(
                    command,
                    Connection,
                    "sp_editSale",

                    getParameter("saleId", System.Data.SqlDbType.Int, record.SaleId),
                    getParameter("itemId", System.Data.SqlDbType.Int, record.ItemId),
                    getParameter("value", System.Data.SqlDbType.Decimal, record.SaleValue),
                    getParameter("start", System.Data.SqlDbType.DateTime, record.StartDate),
                    getParameter("end", System.Data.SqlDbType.DateTime, record.EndDate),
                    getParameter("location", System.Data.SqlDbType.VarChar, record.Location),
                    getParameter("country", System.Data.SqlDbType.Int, record.CountryNum)
                );
                command.ExecuteNonQuery();
            }
        }

        protected override void deleteRecord(Sale record)
        {
            using (SqlCommand command = new SqlCommand())
            {
                formatCommandForProcedure(
                    command,
                    Connection,
                    "sp_deleteSale",

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
