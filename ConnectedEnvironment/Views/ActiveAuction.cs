using System;
using BidSoftware.Shared.DTODefinition;
using System.Data.SqlClient;

namespace BidSoftware.ConnectedEnvironment.Views
{
    internal class ActiveAuctionView : ViewAdapterBase<ActiveAuction>
    {
        public ActiveAuctionView(SqlConnection con) : base(con) { }


        public override string TableName
        {
            get
            {
                return "V_ActiveAuctions";
            }
        }

        protected override ConditionGroup getKeysConditionGroup(object[] keys)
        {
            return new ConditionGroup(
                    new Condition(
                        new NameValuePair("SaleId", keys[0].ToString(), NameValuePair.Operator.GreaterOrEquals))
                    );
        }

        protected override ActiveAuction mapItem(SqlDataReader reader)
        {
            ActiveAuction record = new ActiveAuction()
            {
                SaleId = (int)reader["SaleId"],
                StartDate = (DateTime)reader["StartDate"],
                EndDate = (DateTime)reader["EndDate"],
                Description = (string)reader["Description"],
                UserEmail = (string)reader["UserEmail"],
                Value = (decimal)reader["Value"]
            };
            return record;

        }
    }
}
