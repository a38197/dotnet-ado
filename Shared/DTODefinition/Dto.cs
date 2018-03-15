using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidSoftware.Shared.DTODefinition
{
    public interface IDtoObject : ICloneable, IRecordView
    {
        string ObjectName { get; }
        DatabaseTable Table { get; }
    }

    public interface IRecordView
    {
        
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DtoTableAttribute : Attribute
    {
        public DtoTableAttribute(string tableName)
        {
            this.tableName = tableName;
        }

        private readonly string tableName;
        public string TableName { get { return tableName; } }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false, Inherited=false)]
    public class DtoDefAttribute : Attribute
    {
        public DtoDefAttribute(string label, FieldType type, bool allowInsert, bool allowUpdate)
        {
            this.Label = label;
            this.Type = type;
            this.AllowInsert = allowInsert;
            this.AllowUpdate = allowUpdate;
        }

        public bool AllowInsert { get; private set; }

        public string Label { get; private set; }

        public FieldType Type { get; private set; }

        public bool AllowUpdate { get; private set; }

        public object GetNullDefault(object value)
        {
            var numberChars = from character 
                              in value.ToString().ToCharArray()
                              where character != ' '
                              select character;

            string numberStr = new String(numberChars.ToArray());
            switch (Type)
            {
                case FieldType.Boolean: return isEmptyOrNull(value) ? false : value;
                case FieldType.Date: return isEmptyOrNull(value) ? DateTime.Now.Date : value;
                case FieldType.String: return isEmptyOrNull(value) ? "" : value;
                case FieldType.Integer: return isEmptyOrNull(numberStr) ? "0" : numberStr;
                case FieldType.Money: return getValidMoney(numberStr);
                default: throw new ArgumentException("Non valid definition");
            }
        }

        private bool isEmptyOrNull(object obj)
        {
            return obj == null || obj.ToString().Trim().Length == 0;
        }

        private string getValidMoney(string obj)
        {
            if (isEmptyOrNull(obj))
                return Decimal.Zero.ToString();

            char separator = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            var split = obj
                .Replace('.', separator)
                .Replace(',', separator)
                .Split(separator);
            if (split.Length == 1)
                split = new string[] { split[0], "0" };

            decimal temp;
            if (split.Length > 2 || !Decimal.TryParse(obj, out temp))
                throw new ArgumentException("Invalid decimal format: " + obj);

            string retObj = split[0]
                + separator
                + split[1];

            return retObj;
        }
    }

    public enum FieldType
    {
        String, Integer, Date, Boolean, Money
    }
}
