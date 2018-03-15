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
    public partial class UserInterface : Form
    {
        private readonly IServiceOperations serviceOperations;

        public UserInterface(IServiceOperations operations)
        {
            serviceOperations = operations;
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (!isValidLogin())
                {
                    MessageBox.Show("Credênciais inválidas, por favor verifique.", "Credenciais");
                    return;
                }

                serviceOperations.TestConnection();
                bool validUser = false;
                if (!isAdmin())
                {
                    Credentials appCredentials = new Credentials()
                    {
                        Username = txtUser.Text.Trim(),
                        Password = txtPass.Text.Trim()
                    };
                    validUser = serviceOperations.IsValidUser(appCredentials);
                }
                else
                {
                    validUser = txtUser.Text.Equals(Configuration.GetConfigValue(Configuration.ConfigurationKey.AdminUser), StringComparison.InvariantCulture)
                        && txtPass.Text.Equals(Configuration.GetConfigValue(Configuration.ConfigurationKey.AdminPass), StringComparison.InvariantCulture);
                }

                if (!validUser)
                {
                    MessageBox.Show("Login inválido");
                    return;
                }

                loggedIn();
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro no login");
            }
        }

        private void loggedIn()
        {
            btnLogin.Text = "Logout";
            btnLogin.Click -= btnLogin_Click;
            btnLogin.Click += btnLogout_Click;

            btnUser.Enabled = true;
            btnItem.Enabled = true;
            btnSale.Enabled = true;
            btnAuction.Enabled = true;
            btnBid.Enabled = !isAdmin();
            btnExport.Enabled = true;

            txtPass.Enabled = false;
            txtUser.Enabled = false;
        }

        private void loggedOut()
        {
            btnLogin.Text = "Login";

            btnLogin.Click -= btnLogout_Click;
            btnLogin.Click += btnLogin_Click;
            txtUser.Enabled = true;
            txtPass.Enabled = true;

            btnUser.Enabled = false;
            btnItem.Enabled = false;
            btnSale.Enabled = false;
            btnAuction.Enabled = false;
            btnBid.Enabled = false;
            btnExport.Enabled = false;
        }

        private bool isAdmin()
        {
            return txtUser.Text.Trim().Equals(Configuration.GetConfigValue(Configuration.ConfigurationKey.AdminUser));
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            loggedOut();
        }

        private bool isValidLogin()
        {
            return txtUser.Text.Trim().Length > 0 && txtPass.Text.Trim().Length > 0;
        }

        private void UserInterface_Load(object sender, EventArgs e)
        {
            btnLogin.Click += btnLogin_Click;
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            new TableView<DTODefinition.User>(serviceOperations).ShowDialog(this);
        }

        private void btnItem_Click(object sender, EventArgs e)
        {
            new TableView<DTODefinition.Item>(serviceOperations).ShowDialog(this);
        }

        private void btnSale_Click(object sender, EventArgs e)
        {
            new TableView<DTODefinition.Sale>(serviceOperations).ShowDialog(this);
        }

        private void btnAuction_Click(object sender, EventArgs e)
        {
            new TableView<DTODefinition.Auction>(serviceOperations).ShowDialog(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            User user = new User {
                Email = txtUser.Text.Trim()
            };
            new BidForm(user, serviceOperations).ShowDialog(this);
        }

        private void UserInterface_Closing(object sender, FormClosingEventArgs e)
        {
            serviceOperations.Dispose();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SimpleInt integer = new SimpleInt();
                DtoInputBox<SimpleInt> inputBox = new DtoInputBox<SimpleInt>(integer, DtoInputBox<SimpleInt>.InputType.Insert);
                inputBox.ShowDialog(this);

                if (!inputBox.Submited)
                    return;

                var auction = serviceOperations.ExportAuctionData(integer.Value);

                string path = System.IO.Path.Combine(Environment.CurrentDirectory, "ExportTest.xml");
                Export.Exporter.Export(auction, path);
                MessageBox.Show("Exportação terminada", "Mensagem");
            }
            catch (ConnectException ex)
            {
                MessageBox.Show(ex.Message, "Erro");
            }
            catch (DisconnectException ex)
            {
                MessageBox.Show(ex.Message, "Erro");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro");
            }
            
        }
    }
}
