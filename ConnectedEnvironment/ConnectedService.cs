using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using BidSoftware.Shared;
using BidSoftware.Shared.DTODefinition;
using BidSoftware.Shared.Export;

namespace BidSoftware.ConnectedEnvironment
{
    public class ConnectedService : IServiceOperations
    {
        private SqlConnection connection;
        public ConnectedService()
        {
            Credentials creds = new Credentials() { 
                Username = Configuration.GetConfigValue(Configuration.ConfigurationKey.DbUser),
                Password = Configuration.GetConfigValue(Configuration.ConfigurationKey.DbPassword)
            };
            string connectionString = getConnectionString(creds);
            connection = new SqlConnection(connectionString);
        }

        public string getConnectionString(Credentials creds)
        {
            var server = Configuration.GetConfigValue(Configuration.ConfigurationKey.Server);
            var db = Configuration.GetConfigValue(Configuration.ConfigurationKey.Database);

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = server;
            builder.InitialCatalog = db;
            builder.UserID = creds.Username;
            builder.Password = creds.Password;
            return builder.ToString();
        }

        public void Open()
        {
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
            
        }


        public void Close()
        {
            if (connection != null && connection.State != System.Data.ConnectionState.Closed)
            {
                connection.Close();
            }
        }

        public IEnumerable<IDtoObject> GetTablePage(DatabaseTable table, int startRecord, int numRecords)
        {
            try
            {
                var adapter = Tables.TableFactory.GetTableAdapter(table, connection);
                var data = adapter.GetTablePage(startRecord, numRecords);
                return data;
            }
            catch (Exception ex) { throw new ConnectException(ex.Message, ex); }
            
        }

        public void Dispose()
        {
            try
            {
                if (connection != null)
                {
                    Close();
                    connection.Dispose();
                    connection = null;
                }
            }
            catch (Exception ex) { throw new ConnectException(ex.Message, ex); }
            
        }

        public void AddRecord(DatabaseTable table, IDtoObject record)
        {
            try
            {
                var adapter = Tables.TableFactory.GetTableAdapter(table, connection);
                adapter.AddRecord(record);
            }
            catch (Exception ex) { throw new ConnectException(ex.Message, ex); }
            
        }

        public bool IsValidUser(Credentials creds)
        {
            try
            {
                var adapter = new Tables.UserTableAdapter(connection);
                var result = adapter.ValidateUser(creds.Username, creds.Password);
                return result;
            }
            catch (Exception ex) { throw new ConnectException(ex.Message, ex); }
            
        }

        public void TestConnection()
        {
            try { 
                Open(); 
            }
            catch (Exception ex) { throw new ConnectException(ex.Message, ex); }
            
        }


        public void UpdateRecord(DatabaseTable table, IDtoObject record)
        {
            try
            {
                var adapter = Tables.TableFactory.GetTableAdapter(table, connection);
                adapter.UpdateRecord(record);
            }
            catch (Exception ex) { throw new ConnectException(ex.Message, ex); }
            
        }

        public void DeleteRecord(DatabaseTable table, IDtoObject record)
        {
            try {
                var adapter = Tables.TableFactory.GetTableAdapter(table, connection);
                adapter.DeleteRecord(record);
            }
            catch (Exception ex) { throw new ConnectException(ex.Message, ex); }
            
        }

        public IEnumerable<IRecordView> GetTable(DatabaseTableOrView table)
        {
            try
            {
                var adapter = Tables.TableFactory.GetTableAdapter(table, connection);
                var records = adapter.GetTable();
                return records;
            }
            catch (Exception ex) { throw new ConnectException(ex.Message, ex); }
        }

        public void AddBid(int saleId, string userName, decimal value)
        {
            try
            {
                Shared.DTODefinition.Bid bid = new Shared.DTODefinition.Bid() {
                    SaleId = saleId,
                    UserEmail = userName,
                    Value = value
                };
                var adapter = Tables.TableFactory.GetTableAdapter(DatabaseTable.Bid, connection);
                adapter.AddRecord(bid);
            }
            catch (Exception ex) { throw new ConnectException(ex.Message, ex); }
        }

        public Shared.Export.Auction ExportAuctionData(int auctionId)
        {
            var auction = (Shared.DTODefinition.Auction)Tables.TableFactory.GetTableAdapter(DatabaseTable.Auction, connection).GetRecord(auctionId);
            if (auction == null)
                throw new ArgumentException("Não existe o leilão com id " + auctionId);


            var bids = Tables.TableFactory.GetTableAdapter(DatabaseTable.Bid, connection).GetTable(
                new ConditionGroup(new Condition(new NameValuePair("SaleId", auction.SaleId.ToString(), NameValuePair.Operator.Equals))),
                new SqlParameter[0]
            ).Cast<Shared.DTODefinition.Bid>();

            var bidsToExport =  from bid in bids
                                select new Shared.Export.Bid() {
                                    Datetime = bid.Stamp.ToString(),
                                    UserId = bid.UserEmail
                                };

            Shared.Export.Auction auctionExport = new Shared.Export.Auction() {
                Id = "cenas",
                Info = new Info() { InitialDate = auction.StartDate.ToString(), MinimumBid = auction.MinIncrement.ToString(), ReservationPrice = auction.SaleValue.ToString()  },
                Bids = new Bids()
                {
                    Num = bidsToExport.Count().ToString(),
                    ArrayBid = bidsToExport.ToArray()
                }
            };

            return auctionExport;
        }

        public IDtoObject GetRecord(DatabaseTable table, params object[] keys)
        {
            try
            {
                var adapter = Tables.TableFactory.GetTableAdapter(table, connection);
                var record = adapter.GetRecord(keys);
                return record;
            }
            catch (Exception ex) { throw new ConnectException(ex.Message, ex); }
        }
    }
}

