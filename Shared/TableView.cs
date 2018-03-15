using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BidSoftware.Shared.DTODefinition;

namespace BidSoftware.Shared
{
    public partial class TableView<T> : Form where T : IDtoObject
    {
        private readonly IServiceOperations daoOperations;
        private readonly DatabaseTable daoTable;
        private IEnumerable<T> recordPage;
        private int firstRecord, pageSize;

        public TableView(IServiceOperations dao) {
            daoOperations = dao;
            daoTable = Activator.CreateInstance<T>().Table;
            InitializeComponent();
        }

        public TableView(IServiceOperations dao, DatabaseTable table)
        {
            daoOperations = dao;
            daoTable = table;
            InitializeComponent();
        }

        private void TableView_Load(object sender, EventArgs e)
        {
            this.Text = "Tabela " + daoTable.ToString();
            configureDataTable(getDefinitions());
            pageSize = Int32.Parse(Configuration.GetConfigValue(Configuration.ConfigurationKey.TablePageSize));
            loadTablePage(0);
        }

        private void loadTablePage(int firstRecord)
        {
            try
            {
                dataTable.Rows.Clear();
                recordPage = Enumerable.Empty<T>();
                var records = daoOperations.GetTablePage(daoTable, firstRecord, pageSize);
                if (records.Count() == 0)
                {
                    MessageBox.Show("Sem registos para mostrar");
                    return;
                }

                loadDataTable(records);

                this.firstRecord = firstRecord;
                txtPage.Text = "Pag. " + (firstRecord < pageSize ? "1" : Convert.ToInt32(firstRecord / pageSize).ToString());
                recordPage = records.Cast<T>();
            }
            catch (Exception ex)
            {
                handleException(ex, default(T));
            }
        }

        private void configureDataTable(IEnumerable<DtoDefAttribute> attributes)
        {
            dataTable.Rows.Clear();
            dataTable.Columns.Clear();
            foreach(var attr in attributes)
            {
                DataGridViewColumn column = getAttributeColumn(attr);
                dataTable.Columns.Add(column);
            }
        }

        private DataGridViewColumn getAttributeColumn(DtoDefAttribute attr)
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.HeaderText = attr.Label;
            column.ValueType = typeof(string);

            return column;
        }

        private IEnumerable<DtoDefAttribute> getDefinitions()
        {
            Type typeDef = typeof(T);
            var objData = from property
                          in typeDef.GetProperties()
                          where property.GetCustomAttributes(typeof(DtoDefAttribute), false).Count() > 0
                          select property.GetCustomAttributes(false).OfType<DtoDefAttribute>().First();

            return objData;
        }

        private IEnumerable<System.Reflection.PropertyInfo> getProperties()
        {
            Type typeDef = typeof(T);
            var objData = from property
                          in typeDef.GetProperties()
                          where property.GetCustomAttributes(typeof(DtoDefAttribute), false).Count() > 0
                          select property;

            return objData;
        }

        private void loadDataTable(IEnumerable<IDtoObject> records)
        {
            foreach(var record in records)
            {
                var properties = getProperties();
                List<string> list = new List<string>(properties.Count());
                foreach (var prop in properties)
                    list.Add(prop.GetValue(record).ToString());

                dataTable.Rows.Insert(dataTable.Rows.Count, list.ToArray());
            }
        }

        private void dataTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            T value = Activator.CreateInstance<T>();
            inputboxWithExceptionHandle(value, DtoInputBox<T>.InputType.Insert, () => daoOperations.AddRecord(daoTable, value));
        }

        private void inputboxWithExceptionHandle(T value, DtoInputBox<T>.InputType type, Action submitAction)
        {
            bool done = false;
            while (!done)
            {
                try
                {
                    DtoInputBox<T> inBox = new DtoInputBox<T>(value, type);
                    inBox.ShowDialog(this);
                    if (inBox.Submited)
                        submitAction();

                    done = true;
                }
                catch (Exception ex)
                {
                    handleException(ex, value);
                }
            }
            
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            loadTablePage(firstRecord);
        }

        private T getSelectedRecord()
        {
            if (dataTable.SelectedCells.Count == 0)
                return default(T);

            int index = dataTable.SelectedCells[0].RowIndex;
            return (T)recordPage.ElementAt(index).Clone();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            T value = getSelectedRecord();
            if(value != null)
                inputboxWithExceptionHandle(value, DtoInputBox<T>.InputType.Update, () => daoOperations.UpdateRecord(daoTable, value));
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            T value = getSelectedRecord();
            if (value != null)
                inputboxWithExceptionHandle(value, DtoInputBox<T>.InputType.Delete, () => daoOperations.DeleteRecord(daoTable, value));
        }

        private void handleException(Exception ex, T value)
        {
            if (ex is ConnectException)
                handleConnectedException((ConnectException)ex, value);
            else
                if (ex is DisconnectException)
                    handleDisconnectException((DisconnectException)ex, value);
                else
                    MessageBox.Show(this, ex.Message, "Exception");
        }

        private void handleDisconnectException(DisconnectException ex, T value)
        {
            MessageBox.Show(this, Utils.whatexception(ex).Message, "Exception");
        }

        private void handleConnectedException(ConnectException ex, T value)
        {
            MessageBox.Show(this, ex.Message, "Exception");
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            loadTablePage(firstRecord + pageSize);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if(firstRecord - pageSize < 0)
                loadTablePage(0);
            else
                loadTablePage(firstRecord - pageSize);
        }
    }
}
