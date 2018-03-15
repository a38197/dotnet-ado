namespace BidSoftware.Shared
{
    partial class BidForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvAuctions = new System.Windows.Forms.DataGridView();
            this.colStartDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nudRefresh = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.txtInteger = new System.Windows.Forms.MaskedTextBox();
            this.txtDecimal = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBid = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuctions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRefresh)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvAuctions
            // 
            this.dgvAuctions.AllowUserToAddRows = false;
            this.dgvAuctions.AllowUserToDeleteRows = false;
            this.dgvAuctions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAuctions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAuctions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colStartDate,
            this.colEndDate,
            this.colDesc,
            this.colValue,
            this.colEmail});
            this.dgvAuctions.Location = new System.Drawing.Point(12, 12);
            this.dgvAuctions.MultiSelect = false;
            this.dgvAuctions.Name = "dgvAuctions";
            this.dgvAuctions.ReadOnly = true;
            this.dgvAuctions.Size = new System.Drawing.Size(399, 190);
            this.dgvAuctions.TabIndex = 0;
            // 
            // colStartDate
            // 
            this.colStartDate.HeaderText = "Início";
            this.colStartDate.Name = "colStartDate";
            this.colStartDate.ReadOnly = true;
            // 
            // colEndDate
            // 
            this.colEndDate.HeaderText = "Fim";
            this.colEndDate.Name = "colEndDate";
            this.colEndDate.ReadOnly = true;
            // 
            // colDesc
            // 
            this.colDesc.HeaderText = "Descrição";
            this.colDesc.Name = "colDesc";
            this.colDesc.ReadOnly = true;
            // 
            // colValue
            // 
            this.colValue.HeaderText = "Valor";
            this.colValue.Name = "colValue";
            this.colValue.ReadOnly = true;
            // 
            // colEmail
            // 
            this.colEmail.HeaderText = "Email";
            this.colEmail.Name = "colEmail";
            this.colEmail.ReadOnly = true;
            // 
            // nudRefresh
            // 
            this.nudRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nudRefresh.Location = new System.Drawing.Point(325, 208);
            this.nudRefresh.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRefresh.Name = "nudRefresh";
            this.nudRefresh.Size = new System.Drawing.Size(86, 20);
            this.nudRefresh.TabIndex = 1;
            this.nudRefresh.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nudRefresh.ValueChanged += new System.EventHandler(this.nudRefresh_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(204, 210);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Taxa refrescamento (s)";
            // 
            // txtInteger
            // 
            this.txtInteger.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInteger.Location = new System.Drawing.Point(286, 232);
            this.txtInteger.Mask = "000000000000";
            this.txtInteger.Name = "txtInteger";
            this.txtInteger.Size = new System.Drawing.Size(81, 20);
            this.txtInteger.TabIndex = 3;
            // 
            // txtDecimal
            // 
            this.txtDecimal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDecimal.Location = new System.Drawing.Point(386, 232);
            this.txtDecimal.Mask = "00";
            this.txtDecimal.Name = "txtDecimal";
            this.txtDecimal.Size = new System.Drawing.Size(25, 20);
            this.txtDecimal.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(373, 237);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = ".";
            // 
            // btnBid
            // 
            this.btnBid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBid.Location = new System.Drawing.Point(205, 232);
            this.btnBid.Name = "btnBid";
            this.btnBid.Size = new System.Drawing.Size(75, 23);
            this.btnBid.TabIndex = 6;
            this.btnBid.Text = "Licitar";
            this.btnBid.UseVisualStyleBackColor = true;
            this.btnBid.Click += new System.EventHandler(this.btnBid_Click);
            // 
            // BidForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 273);
            this.Controls.Add(this.btnBid);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDecimal);
            this.Controls.Add(this.txtInteger);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudRefresh);
            this.Controls.Add(this.dgvAuctions);
            this.Name = "BidForm";
            this.Text = "Licitar";
            this.Load += new System.EventHandler(this.BidForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuctions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRefresh)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvAuctions;
        private System.Windows.Forms.NumericUpDown nudRefresh;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox txtInteger;
        private System.Windows.Forms.MaskedTextBox txtDecimal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEndDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmail;
    }
}