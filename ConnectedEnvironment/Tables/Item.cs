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
    internal class ItemTableAdapter : TableAdapterBase<Item>
    {
        public ItemTableAdapter(SqlConnection connection) : base(connection) { }

        public override string TableName
        {
            get
            {
                return "vItem";
            }
        }

        protected override void addRecord(Item record)
        {
            using(SqlCommand command = new SqlCommand())
            {
                formatCommandForProcedure(
                    command,
                    Connection,
                    "sp_addItem",

                    getParameter("desc", System.Data.SqlDbType.VarChar, record.Description),
                    getParameter("condition", System.Data.SqlDbType.VarChar, record.ItemConditionId),
                    getParameter("email", System.Data.SqlDbType.VarChar, record.UserEmail)
                );
                command.ExecuteNonQuery();
            }
        }

        

        protected override Item mapItem(SqlDataReader reader)
        {
            Item record = new Item();
            record.Description = (string)reader["Description"];
            record.ItemConditionId = (int)reader["ItemConditionId"];
            record.UserEmail = (string)reader["UserEmail"];
            record.ItemId = (int)reader["ItemId"];
            return record;
        }

        protected override void updateRecord(Item record)
        {
            using (SqlCommand command = new SqlCommand())
            {
                formatCommandForProcedure(
                    command,
                    Connection,
                    "sp_editItem",

                    getParameter("id", System.Data.SqlDbType.VarChar, record.ItemId),
                    getParameter("desc", System.Data.SqlDbType.VarChar, record.Description),
                    getParameter("condition", System.Data.SqlDbType.VarChar, record.ItemConditionId),
                    getParameter("email", System.Data.SqlDbType.VarChar, record.UserEmail)
                );
                command.ExecuteNonQuery();
            }
        }

        protected override void deleteRecord(Item record)
        {
            using (SqlCommand command = new SqlCommand())
            {
                formatCommandForProcedure(
                    command,
                    Connection,
                    "sp_deleteItem",

                    getParameter("id", System.Data.SqlDbType.VarChar, record.ItemId)
                );
                command.ExecuteNonQuery();
            }
        }

        protected override ConditionGroup getKeysConditionGroup(object[] keys)
        {
            return new ConditionGroup(
                    new Condition(
                        new NameValuePair("ItemId", keys[0].ToString(), NameValuePair.Operator.Equals))
                    );
        }
    }
}
