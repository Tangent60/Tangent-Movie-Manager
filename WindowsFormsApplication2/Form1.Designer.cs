namespace WindowsFormsApplication2
{
    partial class frmMain
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
            this.btnLoad = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDeleteDuplicates = new System.Windows.Forms.Button();
            this.mbtnRefresh = new MetroFramework.Controls.MetroButton();
            this.mtxtSearch = new MetroFramework.Controls.MetroTextBox();
            this.mlblInfo = new MetroFramework.Controls.MetroLabel();
            this.Play = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(126, 12);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 39);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Play});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(516, 275);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Location = new System.Drawing.Point(0, 126);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(516, 275);
            this.panel1.TabIndex = 2;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(54, 60);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(66, 13);
            this.lblSearch.TabIndex = 4;
            this.lblSearch.Text = "Search Filter";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(207, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 39);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDeleteDuplicates
            // 
            this.btnDeleteDuplicates.Location = new System.Drawing.Point(300, 12);
            this.btnDeleteDuplicates.Name = "btnDeleteDuplicates";
            this.btnDeleteDuplicates.Size = new System.Drawing.Size(75, 39);
            this.btnDeleteDuplicates.TabIndex = 7;
            this.btnDeleteDuplicates.Text = "Del Dup";
            this.btnDeleteDuplicates.UseVisualStyleBackColor = true;
            this.btnDeleteDuplicates.Click += new System.EventHandler(this.btnDeleteDuplicates_Click);
            // 
            // mbtnRefresh
            // 
            this.mbtnRefresh.Highlight = true;
            this.mbtnRefresh.Location = new System.Drawing.Point(403, 47);
            this.mbtnRefresh.Name = "mbtnRefresh";
            this.mbtnRefresh.Size = new System.Drawing.Size(88, 46);
            this.mbtnRefresh.TabIndex = 10;
            this.mbtnRefresh.Text = "Refresh";
            this.mbtnRefresh.Click += new System.EventHandler(this.mbtnRefresh_Click);
            // 
            // mtxtSearch
            // 
            this.mtxtSearch.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.mtxtSearch.Location = new System.Drawing.Point(126, 57);
            this.mtxtSearch.Name = "mtxtSearch";
            this.mtxtSearch.Size = new System.Drawing.Size(249, 25);
            this.mtxtSearch.TabIndex = 11;
            // 
            // mlblInfo
            // 
            this.mlblInfo.AutoSize = true;
            this.mlblInfo.Location = new System.Drawing.Point(126, 95);
            this.mlblInfo.Name = "mlblInfo";
            this.mlblInfo.Size = new System.Drawing.Size(80, 19);
            this.mlblInfo.TabIndex = 12;
            this.mlblInfo.Text = "Metro Label";
            // 
            // Play
            // 
            this.Play.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Play.Frozen = true;
            this.Play.HeaderText = "Play";
            this.Play.Name = "Play";
            this.Play.Text = "Play";
            this.Play.UseColumnTextForButtonValue = true;
            this.Play.Width = 50;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 401);
            this.Controls.Add(this.mlblInfo);
            this.Controls.Add(this.mtxtSearch);
            this.Controls.Add(this.mbtnRefresh);
            this.Controls.Add(this.btnDeleteDuplicates);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnLoad);
            this.Name = "frmMain";
            this.Text = "Tangent Movie Manager";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDeleteDuplicates;
        private MetroFramework.Controls.MetroButton mbtnRefresh;
        private MetroFramework.Controls.MetroTextBox mtxtSearch;
        private MetroFramework.Controls.MetroLabel mlblInfo;
        private System.Windows.Forms.DataGridViewButtonColumn Play;
    }
}

