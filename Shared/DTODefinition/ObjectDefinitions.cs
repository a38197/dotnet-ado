using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidSoftware.Shared.DTODefinition
{

    [DtoTable("Item")]
    public class Item : IDtoObject
    {
        [DtoDef("Item Id", FieldType.Integer, false, false)]
        public int ItemId { get; set; }
        [DtoDef("Item Description", FieldType.String, true, true)]
        public string Description { get; set; }
        [DtoDef("Item Condition Id", FieldType.Integer, true, true)]
        public int ItemConditionId { get; set; }
        [DtoDef("User Email", FieldType.String, true, true)]
        public string UserEmail { get; set; }

        public virtual string ObjectName
        { 
            get{ 
                return "Item"; 
            }
        }

        public override string ToString()
        {
            return String.Format("[{0}:ItemId={1}, Description={2}, ItemConditionId={3}]",
                ObjectName, ItemId, Description, ItemConditionId);
        }

        public virtual object Clone()
        {
            return new Item { 
                ItemId = this.ItemId,
                Description = this.Description,
                ItemConditionId = this.ItemConditionId,
                UserEmail = this.UserEmail
            };
        }


        public DatabaseTable Table
        {
            get { return DatabaseTable.Item; }
        }
    }

    [DtoTable("ItemCondition")]
    public class ItemCondition : IDtoObject
    {
        public virtual string ObjectName
        {
            get
            {
                return "Item Condition";
            }
        }

        [DtoDef("Item Condition Id", FieldType.Integer, true, false)]
        public int ItemConditionId { get; set; }
        [DtoDef("Condition Description", FieldType.String, false, true)]
        public string Description { get; set; }

        public override string ToString()
        {
            return String.Format("[{0}:ItemConditionId={1}, Description={2}]",
                ObjectName, ItemConditionId, Description );
        }

        public virtual object Clone()
        {
            return new ItemCondition
            {
                Description = this.Description,
                ItemConditionId = this.ItemConditionId
            };
        }

        public DatabaseTable Table
        {
            get { return DatabaseTable.ItemCondition; }
        }
    }

    [DtoTable("User")]
    public class User : IDtoObject
    {
        [DtoDef("Email", FieldType.String, true, false)]
        public string Email { get; set; }
        [DtoDef("Name", FieldType.String, true, true)]
        public string Name { get; set; }
        [DtoDef("Address", FieldType.String, true, true)]
        public string Address { get; set; }
        [DtoDef("Password", FieldType.String, true, true)]
        public string Password { get; set; }
        public string OldPassword { get; set; }
        [DtoDef("Country Number", FieldType.Integer, true, true)]
        public int CountryNum { get; set; }

        public virtual string ObjectName
        {
            get
            {
                return "User";
            }
        }

        public override string ToString()
        {
            return String.Format("[{0}:Email={1}, Name={2}, Address={3}, Password={4}, CountryNum={5}]",
                ObjectName, Email, Name, Address, Password, CountryNum);
        }

        public virtual object Clone()
        {
            return new User
            {
                Email = this.Email,
                Name = this.Name,
                Address = this.Address,
                Password = this.Password,
                CountryNum = this.CountryNum,
                OldPassword = this.OldPassword
            };
        }

        public DatabaseTable Table
        {
            get { return DatabaseTable.User; }
        }
    }

    [DtoTable("Sale")]
    public class Sale : IDtoObject
    {
        [DtoDef("Sale Id", FieldType.Integer, false, false)]
        public int SaleId { get; set; }
        [DtoDef("Start Date", FieldType.Date, true, true)]
        public DateTime StartDate { get; set; }
        [DtoDef("End Date", FieldType.Date, true, true)]
        public DateTime EndDate { get; set; }
        [DtoDef("Location", FieldType.String, true, true)]
        public string Location { get; set; }
        [DtoDef("Country Number", FieldType.Integer, true, true)]
        public int CountryNum { get; set; }
        [DtoDef("Item Id", FieldType.Integer, true, true)]
        public int ItemId { get; set; }
        [DtoDef("Sale Value", FieldType.Money, true, true)]
        public decimal SaleValue { get; set; }

        public virtual string ObjectName
        {
            get
            {
                return "Sale";
            }
        }

        public override string ToString()
        {
            return String.Format("[{0}:SaleId={1}, StartDate={2}, EndDate={3}, Location={4}, CountryNum={5}, ItemId={6}, SaleValue={7}]",
                ObjectName, SaleId, StartDate, EndDate, Location, CountryNum, ItemId, SaleValue);
        }

        public virtual object Clone()
        {
            Sale sale = new Sale();
            fillClone(sale);
            return sale;
        }

        protected virtual void fillClone(Sale sale)
        {
            sale.SaleId = this.SaleId;
            sale.StartDate = this.StartDate;
            sale.EndDate = this.EndDate;
            sale.Location = this.Location;
            sale.CountryNum = this.CountryNum;
            sale.ItemId = this.ItemId;
            sale.SaleValue = this.SaleValue;
        }

        public virtual DatabaseTable Table
        {
            get { return DatabaseTable.Sale; }
        }
    }

    [DtoTable("AuctionSale")]
    public class Auction : Sale
    {
        [DtoDef("Minimum Increment", FieldType.Money, true, true)]
        public decimal MinIncrement { get; set; }

        public override string ObjectName
        {
            get
            {
                return "Auction";
            }
        }

        public override string ToString()
        {
            return String.Format("[{0}:SaleId={1}, StartDate={2}, EndDate={3}, Location={4}, CountryNum={5}, ItemId={6}, SaleValue={7}, IsAuction={8}, MinIncrement={9}]",
                ObjectName, SaleId, StartDate, EndDate, Location, CountryNum, ItemId, SaleValue, MinIncrement);
        }

        public override object Clone()
        {
            Auction auction = new Auction();
            fillClone(auction);
            return auction;
        }

        protected override void fillClone(Sale sale)
        {
            Auction auction = (Auction)sale;
            base.fillClone(sale);
            auction.MinIncrement = this.MinIncrement;
        }

        public override DatabaseTable Table
        {
            get { return DatabaseTable.Auction; }
        }
    }

    [DtoTable("Bid")]
    public class Bid : IDtoObject
    {
        [DtoDef("Bid Id", FieldType.Integer, false, false)]
        public int BidId { get; set; }
        [DtoDef("Stamp", FieldType.Date, true, true)]
        public DateTime Stamp { get; set; }
        [DtoDef("Sale Id", FieldType.Integer, true, true)]
        public int SaleId { get; set; }
        [DtoDef("User Email", FieldType.String, true, true)]
        public string UserEmail { get; set; }
        [DtoDef("Deleted", FieldType.Boolean, true, true)]
        public bool Deleted { get; set; }
        [DtoDef("Value", FieldType.Money, true, true)]
        public decimal Value { get; set; }

        public string ObjectName
        {
            get
            {
                return "Bid";
            }
        }

        public DatabaseTable Table
        {
            get
            {
                return DatabaseTable.Bid;
            }
        }

        public object Clone()
        {
            return new Bid() {
                BidId = this.BidId,
                Deleted = this.Deleted,
                SaleId = this.SaleId,
                Stamp = this.Stamp,
                UserEmail = this.UserEmail,
                Value = this.Value
            };
        }

        public override string ToString()
        {
            return String.Format("[{0}:BidId={1}, Deleted={2}, SaleId={3}, Stamp={4}, UserEmail={5}, Value={6}]",
                ObjectName, BidId, Deleted, SaleId, Stamp, UserEmail, Value);
        }
    }

    public class SimpleInt : IDtoObject
    {
        [DtoDef("Qual o id leilão ?", FieldType.Integer, true, false)]
        public int Value { get; set; }

        public string ObjectName
        {
            get
            {
                return "Introduza um inteiro";
            }
        }

        public DatabaseTable Table
        {
            get
            {
                return DatabaseTable.None;
            }
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }

    public class ActiveAuction : IRecordView
    {
        public int SaleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string UserEmail { get; set; }
        public decimal Value { get; set; }
    }
}
