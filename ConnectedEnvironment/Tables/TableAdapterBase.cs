using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BidSoftware.Shared.DTODefinition;
using BidSoftware.Shared;
using BidSoftware.ConnectedEnvironment.Views;

namespace BidSoftware.ConnectedEnvironment.Tables
{
    internal interface ITableAdapter<out TOut> 
        where TOut : IRecordView
    {
        SqlConnection Connection { get; set; }
        IEnumerable<TOut> GetTablePage(int startRecord, int numRecords);
        IEnumerable<TOut> GetTable();
        IEnumerable<TOut> GetTable(ConditionGroup conditionGroup, params SqlParameter[] conditionParams);
        TOut GetRecord(params object[] keys);
        void AddRecord(IDtoObject record);
        void UpdateRecord(IDtoObject record);
        void DeleteRecord(IDtoObject record);
    }
    
    internal abstract class TableAdapterBase<T> : ViewAdapterBase<T> 
        where T : IDtoObject
    {
        public TableAdapterBase(SqlConnection con) : base(con) { }
        
        public override void AddRecord(IDtoObject record)
        {
            addRecord((T)record);
        }
        protected abstract void addRecord(T record);

        public override void UpdateRecord(IDtoObject record)
        {
            updateRecord((T)record);
        }
        protected abstract void updateRecord(T record);

        public override void DeleteRecord(IDtoObject record)
        {
            deleteRecord((T)record);
        }
        protected abstract void deleteRecord(T record);
        
        protected SqlCommand formatCommandForProcedure(SqlCommand command, SqlConnection con, string procedureName, params SqlParameter[] parameters)
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Connection = con;
            command.CommandText = String.Format("{0}.{1}", SchemaName, procedureName); ;
            foreach (var p in parameters)
                command.Parameters.Add(p);

            return command;
        }

        
    }

    internal static class TableFactory
    {
        public static ITableAdapter<IDtoObject> GetTableAdapter(DatabaseTable table, SqlConnection con)
        {
            switch (table)
            {
                case DatabaseTable.User: return new UserTableAdapter(con);
                case DatabaseTable.Item: return new ItemTableAdapter(con);
                case DatabaseTable.Sale: return new SaleTableAdapter(con);
                case DatabaseTable.Auction: return new AuctionTableAdapter(con);
                case DatabaseTable.Bid: return new BidTableAdapter(con);
                default: throw new ArgumentException("Unknown table");
            }
        }

        public static ITableAdapter<IRecordView> GetTableAdapter(DatabaseTableOrView table, SqlConnection con)
        {
            switch (table)
            {
                case DatabaseTableOrView.User: return new UserTableAdapter(con);
                case DatabaseTableOrView.Item: return new ItemTableAdapter(con);
                case DatabaseTableOrView.Sale: return new SaleTableAdapter(con);
                case DatabaseTableOrView.Auction: return new AuctionTableAdapter(con);
                case DatabaseTableOrView.Bid: return new BidTableAdapter(con);
                case DatabaseTableOrView.ActiveAuctionsView: return new ActiveAuctionView(con);
                default: throw new ArgumentException("Unknown table or view");
            }
        }
    }
}
