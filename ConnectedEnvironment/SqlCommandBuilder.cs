using System;
using System.Collections.Generic;
using System.Text;
using BidSoftware.Shared;

namespace BidSoftware.ConnectedEnvironment
{
    internal class SqlCommandBuilder
    {

        public static SqlCommandBuilder GetDefaultSelectCommand(string tableName)
        {
            return new SqlCommandBuilder(CommandType.Select)
                .SelectAllColumns()
                .SetSchema(Configuration.GetConfigValue(Configuration.ConfigurationKey.Schema))
                .SetTable(tableName);
        }

        public enum CommandType { Select, Update, Delete }

        CommandType commandType;
        public SqlCommandBuilder(CommandType type)
        {
            this.commandType = type;
        }

        private bool allColumns = true;
        public SqlCommandBuilder SelectAllColumns()
        {
            allColumns = true;
            columns.Clear();
            return this;
        }

        private List<string> columns = new List<string>();
        public SqlCommandBuilder SelectColumn(string columnName)
        {
            allColumns = false;
            columns.Add(columnName);
            return this;
        }

        private string schema = null;
        public SqlCommandBuilder SetSchema(string schemaName)
        {
            schema = schemaName;
            return this;
        }

        string table = null;
        public SqlCommandBuilder SetTable(string tableName)
        {
            table = tableName;
            return this;
        }

        private LinkedList<ConditionGroup> conditionList = new LinkedList<ConditionGroup>();
        public SqlCommandBuilder AddConditionGroup(ConditionGroup cond)
        {
            conditionList.AddLast(cond);
            return this;
        }

        public string Build()
        {
            switch (commandType)
            {
                case CommandType.Delete: return buildDelete();
                case CommandType.Select: return buildSelect();
                case CommandType.Update: return buildUpdate();
                default: throw new ArgumentException("Invalid command type");
            }
        }

        private string buildUpdate()
        {
            throw new NotImplementedException();
        }

        private string buildSelect()
        {
            StringBuilder sb = new StringBuilder("Select ");
            if (allColumns)
                sb.Append(" * ");
            else
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    sb.Append(columns[i]);
                    if (i < columns.Count - 1)
                        sb.Append(",");
                }
            }

            sb.AppendFormat(" from [{0}].[{1}] ", schema, table);

            if(conditionList.Count > 0)
            {
                sb.Append(" where ");
                foreach (var c in conditionList)
                    sb.AppendFormat(" {0} ", c.ToString());
            }
            

            return sb.ToString();
        }

        private string buildDelete()
        {
            throw new NotImplementedException();
        }
    }

    internal class NameValuePair
    {
        public enum Operator { Equals, NotEquals, Greater, Less, GreaterOrEquals, LessOrEquals }
        string name, value;
        private Operator oper;
        public NameValuePair(string name, string value, Operator oper)
        {
            this.name = name;
            this.value = value;
            this.oper = oper;
        }

        public string GetName { get { return name; } }
        public string GetValue { get { return value; } }
        public string OperatorString { get
            {
                switch (oper)
                {
                    case Operator.Equals: return "=";
                    case Operator.NotEquals: return "<>";
                    case Operator.Greater: return ">";
                    case Operator.GreaterOrEquals: return ">=";
                    case Operator.Less: return "<";
                    case Operator.LessOrEquals: return "<=";
                    default:throw new ArgumentException("Invalid Operator");
                }
            }
        }
    }

    internal class Condition
    {
        public enum ConditionType { And, Or }

        private readonly NameValuePair first;
        public Condition(NameValuePair pair)
        {
            first = pair;
        }

        private LinkedList<NameValuePair> andList = new LinkedList<NameValuePair>();
        public Condition And(NameValuePair pair)
        {
            andList.AddLast(pair);
            return this;
        }

        private LinkedList<NameValuePair> orList = new LinkedList<NameValuePair>();
        public Condition Or(NameValuePair pair)
        {
            orList.AddLast(pair);
            return this;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" {0}{1}{2} ", first.GetName, first.OperatorString, first.GetValue);

            foreach(NameValuePair nvp in andList)
                sb.AppendFormat(" and {0}{1}{2} ", nvp.GetName, nvp.OperatorString, nvp.GetValue);

            foreach (NameValuePair nvp in orList)
                sb.AppendFormat(" or {0}{1}{2} ", nvp.GetName, nvp.OperatorString, nvp.GetValue);

            return sb.ToString();
        }
    }

    internal class ConditionGroup
    {
        private readonly Condition first;
        public ConditionGroup(Condition cond)
        {
            first = cond;
        }

        private LinkedList<Condition> andList = new LinkedList<Condition>();
        public ConditionGroup And(Condition cond)
        {
            andList.AddLast(cond);
            return this;
        }

        private LinkedList<Condition> orList = new LinkedList<Condition>();
        public ConditionGroup Or(Condition cond)
        {
            orList.AddLast(cond);
            return this;
        }

        private LinkedList<string> closedGroups = new LinkedList<string>();
        public ConditionGroup CloseGroup(Condition.ConditionType nextGroupType)
        {
            string group = getGroupString();
            andList.Clear();
            orList.Clear();

            if (nextGroupType == Condition.ConditionType.And)
                closedGroups.AddLast(String.Format(" {0} and ", group));
            if (nextGroupType == Condition.ConditionType.Or)
                closedGroups.AddLast(String.Format(" {0} or ", group));

            return this;
        }

        private string getGroupString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" ( {0} ", first.ToString());

            foreach (Condition cond in andList)
                sb.AppendFormat(" and {0} ", cond.ToString());

            foreach (Condition cond in orList)
                sb.AppendFormat(" or {0} ", cond.ToString());

            sb.Append(" ) ");
            return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string group in closedGroups)
                sb.Append(group);

            sb.Append(getGroupString());
            return sb.ToString();
        }
    }
}
