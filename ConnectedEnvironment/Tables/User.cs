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
    internal class UserTableAdapter : TableAdapterBase<User>
    {
        public UserTableAdapter(SqlConnection connection) : base(connection) {}

        public override string TableName
        {
            get
            {
                return "vUser";
            }
        }
        
        protected override void addRecord(User record)
        {
            using(SqlCommand command = new SqlCommand())
            {
                formatCommandForProcedure(
                    command,
                    Connection,
                    "sp_addUser",

                    getParameter("email", System.Data.SqlDbType.VarChar, record.Email),
                    getParameter("name", System.Data.SqlDbType.VarChar, record.Name),
                    getParameter("address", System.Data.SqlDbType.VarChar, record.Address),
                    getParameter("password", System.Data.SqlDbType.VarChar, record.Password),
                    getParameter("country", System.Data.SqlDbType.Int, record.CountryNum)
                );
                command.ExecuteNonQuery();
            }
        }

        protected override User mapItem(SqlDataReader reader)
        {
            User record = new User();
            record.Address = (string) reader["Address"];
            record.CountryNum = (int) reader["CountryNum"];
            record.Email = (string) reader["Email"];
            record.Name = (string) reader["Name"];
            record.Password = ConvertUtils.GetByteString((byte[])reader["Password"]);
            record.OldPassword = record.Password;
            return record;
        }

        protected override void updateRecord(User record)
        {
            using (SqlCommand command = new SqlCommand())
            {
                formatCommandForProcedure(
                    command,
                    Connection,
                    "sp_editUser",

                    getParameter("email", System.Data.SqlDbType.VarChar, record.Email),
                    getParameter("name", System.Data.SqlDbType.VarChar, record.Name),
                    getParameter("address", System.Data.SqlDbType.VarChar, record.Address),
                    getParameter("country", System.Data.SqlDbType.Int, record.CountryNum)
                );
                if (record.Password != record.OldPassword)
                    command.Parameters.Add(getParameter("password", System.Data.SqlDbType.VarChar, record.Password));
                command.ExecuteNonQuery();
            }
        }

        protected override void deleteRecord(User record)
        {
            using (SqlCommand command = new SqlCommand())
            {
                formatCommandForProcedure(
                    command,
                    Connection,
                    "sp_deleteUser",

                    getParameter("email", System.Data.SqlDbType.VarChar, record.Email)
                );
                command.ExecuteNonQuery();
            }
        }

        public bool ValidateUser(string username, string password)
        {
            using (SqlCommand command = new SqlCommand())
            {
                formatCommandForProcedure(
                    command,
                    Connection,
                    "sp_validateUser",

                    getParameter("email", System.Data.SqlDbType.VarChar, username),
                    getParameter("password", System.Data.SqlDbType.VarChar, password)
                );

                SqlParameter par = new SqlParameter("result", System.Data.SqlDbType.Int);
                par.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(par);

                command.ExecuteNonQuery();

                return ((int)par.Value > 0);
            }
        }

        protected override ConditionGroup getKeysConditionGroup(object[] keys)
        {
            return new ConditionGroup(
                    new Condition(
                        new NameValuePair("Email", keys[0].ToString(), NameValuePair.Operator.Equals))
                    );
        }
    }
}
