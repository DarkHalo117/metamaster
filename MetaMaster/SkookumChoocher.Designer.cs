
namespace MetaMaster
{
    partial class SkookumChoocher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SkookumChoocher));
            this.action = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.exec_chooch = new System.Windows.Forms.Button();
            this.application = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.user_email = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.user_password = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // action
            // 
            this.action.AutoSize = true;
            this.action.Location = new System.Drawing.Point(12, 16);
            this.action.Name = "action";
            this.action.Size = new System.Drawing.Size(43, 13);
            this.action.TabIndex = 2;
            this.action.Text = "Action: ";
            // 
            // comboBox2
            // 
            this.comboBox2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBox2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "search",
            "everest_call",
            "fetch_keys",
            "create",
            "read",
            "update",
            "delete"});
            this.comboBox2.Location = new System.Drawing.Point(63, 13);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(13, 40);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.exec_chooch);
            this.splitContainer1.Panel1.Controls.Add(this.application);
            this.splitContainer1.Panel1.Controls.Add(this.listBox1);
            this.splitContainer1.Panel1.Tag = "";
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(775, 365);
            this.splitContainer1.SplitterDistance = 175;
            this.splitContainer1.TabIndex = 4;
            // 
            // exec_chooch
            // 
            this.exec_chooch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exec_chooch.Location = new System.Drawing.Point(7, 316);
            this.exec_chooch.Name = "exec_chooch";
            this.exec_chooch.Size = new System.Drawing.Size(164, 39);
            this.exec_chooch.TabIndex = 4;
            this.exec_chooch.Text = "Chooch";
            this.exec_chooch.UseVisualStyleBackColor = true;
            this.exec_chooch.Click += new System.EventHandler(this.exec_chooch_Click);
            // 
            // application
            // 
            this.application.AutoSize = true;
            this.application.Location = new System.Drawing.Point(4, 4);
            this.application.Name = "application";
            this.application.Size = new System.Drawing.Size(62, 13);
            this.application.TabIndex = 1;
            this.application.Text = "Application:";
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Items.AddRange(new object[] {
            "software_license_fetch",
            "computer_evaluation",
            "users",
            "customer_request_order",
            "everest"});
            this.listBox1.Location = new System.Drawing.Point(7, 20);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(164, 290);
            this.listBox1.TabIndex = 3;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AccessibleName = "";
            this.dataGridView1.AccessibleRole = System.Windows.Forms.AccessibleRole.List;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(596, 365);
            this.dataGridView1.TabIndex = 0;
            // 
            // user_email
            // 
            this.user_email.AutoSize = true;
            this.user_email.Location = new System.Drawing.Point(191, 16);
            this.user_email.Name = "user_email";
            this.user_email.Size = new System.Drawing.Size(32, 13);
            this.user_email.TabIndex = 5;
            this.user_email.Text = "User:";
            // 
            // comboBox1
            // 
            this.comboBox1.AllowDrop = true;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(230, 13);
            this.comboBox1.MaxLength = 128;
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(237, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // user_password
            // 
            this.user_password.AutoSize = true;
            this.user_password.Location = new System.Drawing.Point(473, 16);
            this.user_password.Name = "user_password";
            this.user_password.Size = new System.Drawing.Size(56, 13);
            this.user_password.TabIndex = 7;
            this.user_password.Text = "Password:";
            // 
            // textBox1
            // 
            this.textBox1.AllowDrop = true;
            this.textBox1.HideSelection = false;
            this.textBox1.Location = new System.Drawing.Point(535, 14);
            this.textBox1.MaxLength = 64;
            this.textBox1.Name = "textBox1";
            this.textBox1.PasswordChar = '*';
            this.textBox1.Size = new System.Drawing.Size(214, 20);
            this.textBox1.TabIndex = 2;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 395);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // SkookumChoocher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 417);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.user_password);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.user_email);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.action);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SkookumChoocher";
            this.Text = "SkookumChoocher";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label action;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label application;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label user_email;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label user_password;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button exec_chooch;
        private System.Windows.Forms.StatusStrip statusStrip1;
    }
}