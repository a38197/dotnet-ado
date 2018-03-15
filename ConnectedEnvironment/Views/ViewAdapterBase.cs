using System;
using System.Collections.Generic;
using System.Linq;
using BidSoftware.ConnectedEnvironment.Tables;
using BidSoftware.Shared.DTODefinition;
using System.Data.SqlClient;

namespace BidSoftware.ConnectedEnvironment.Views
{
    internal abstract class ViewAdapterBase<T> : ITableAdapter<T> where T : IRecordView
    {
        public ViewAdapterBase(SqlConnection con) {
            Connection = con;
        }

        public SqlConnection Connection { get; set; }

        public abstract string TableName { get; }

        public string SchemaName { get { return "app"; } }

        public virtual IEnumerable<T> GetTablePage(int startRecord, int numRecords)
        {
            var condition = new ConditionGroup(
                    new Condition(
                        new NameValuePair("ROW_NR", "@start", NameValuePair.Operator.GreaterOrEquals))
                        .And(new NameValuePair("ROW_NR", "@end", NameValuePair.Operator.LessOrEquals))
                    );

            var parameters = new SqlParameter[] {
                getParameter("start", System.Data.SqlDbType.Int, startRecord),
                getParameter("end", System.Data.SqlDbType.Int, startRecord + numRecords)
            };

            return GetTable(condition, parameters);
        }

        public IEnumerable<T> GetTable(ConditionGroup conditionGroup, params SqlParameter[] conditionParams)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.CommandText = SqlCommandBuilder.GetDefaultSelectCommand(TableName)
                    .AddConditionGroup(conditionGroup)
                    .Build();

                command.Parameters.AddRange(conditionParams);

                command.Connection = Connection;
                return mapReader(command.ExecuteReader());
            }
        }

        public virtual IEnumerable<T> GetTable()
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.CommandText = SqlCommandBuilder.GetDefaultSelectCommand(TableName).Build();

                command.Connection = Connection;
                return mapReader(command.ExecuteReader());
            }
        }

        public T GetRecord(params object[] keys)
        {
            ConditionGroup condition = getKeysConditionGroup(keys);
            var records = GetTable(condition);
            return records.FirstOrDefault();
        }

        protected abstract ConditionGroup getKeysConditionGroup(object[] keys);

        protected SqlParameter getParameter(string name, System.Data.SqlDbType type, object value)
        {
            var param = new SqlParameter(name, type);
            param.Value = value;
            return param;
        }

        protected IEnumerable<T> mapReader(SqlDataReader reader)
        {
            using (reader)
            {
                LinkedList<T> list = new LinkedList<T>();
                while (reader.Read())
                {
                    T record = mapItem(reader);
                    list.AddLast(record);
                }
                return list;
            }
        }

        protected abstract T mapItem(SqlDataReader reader);

        public virtual void AddRecord(IDtoObject record)
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateRecord(IDtoObject record)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteRecord(IDtoObject record)
        {
            throw new NotImplementedException();
        }
    }

    
}
