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
    public partial class BidForm : Form
    {
        private readonly User user;
        private readonly IServiceOperations operations;
        public BidForm(User user, IServiceOperations operations)
        {
            this.operations = operations;
            this.user = user;
            InitializeComponent();
        }

        private void BidForm_Load(object sender, EventArgs e)
        {
            loadActiveAuctions();

            timer.Tick += Timer_Tick;
            setTimer();
        }

        private Timer timer = new Timer();
        private void setTimer()
        {
            timer.Stop();
            timer.Interval = Convert.ToInt32(nudRefresh.Value) * 1000;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            loadActiveAuctions();
        }

        private IEnumerable<ActiveAuction> auctions;
        private void loadActiveAuctions()
        {
            int rowIndex = -1, colIndex = -1;
            if(dgvAuctions.SelectedCells.Count > 0)
            {
                rowIndex = dgvAuctions.SelectedCells[0].RowIndex;
                colIndex = dgvAuctions.SelectedCells[0].ColumnIndex;
            }

            auctions = operations.GetTable(DatabaseTableOrView.ActiveAuctionsView).Cast<ActiveAuction>();
            dgvAuctions.Rows.Clear();
            foreach(var auction in auctions)
            {
                dgvAuctions.Rows.Add(auction.StartDate, auction.EndDate, auction.Description, auction.Value, auction.UserEmail);
            }
            
            if(rowIndex >= 0)
            {
                dgvAuctions.ClearSelection();
                dgvAuctions.Rows[rowIndex].Cells[colIndex].Selected = true;
            }
        }

        private void nudRefresh_ValueChanged(object sender, EventArgs e)
        {
            setTimer();
        }

        private void btnBid_Click(object sender, EventArgs e)
        {
            decimal value = getValue();
            ActiveAuction selected = getSelectedAuction();

            if (null == selected)
                return;

            if(value <= selected.Value)
            {
                MessageBox.Show("Valor inválido", "Erro");
                return;
            }

            try
            {
                operations.AddBid(selected.SaleId, user.Email, value);
                loadActiveAuctions();
                MessageBox.Show("Bid inserida", "Mensagem");
            } catch (ConnectException con)
            {
                handleConnectedException(con);
            } catch (DisconnectException disc)
            {
                handleDisconnectedException(disc);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Exception");
            }

        }

        private void handleDisconnectedException(DisconnectException disc)
        {
            MessageBox.Show(this, disc.Message, "DisconnectException");
        }

        private void handleConnectedException(ConnectException ex)
        {
            MessageBox.Show(this, ex.Message, "ConnectException");
        }

        private decimal getValue()
        {
            string val = "0" + txtInteger.Text.Trim()
                + System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator
                + txtDecimal.Text.Trim() + "0";

            decimal ret;
            if (!Decimal.TryParse(val, out ret))
                return -1;
            return ret;
        }

        private ActiveAuction getSelectedAuction()
        {
            if (dgvAuctions.SelectedCells.Count == 0) return null;
            return auctions.ElementAt(dgvAuctions.SelectedCells[0].RowIndex);
        }
    }
}
