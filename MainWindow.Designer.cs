
namespace MsalExample
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            Button ExitButton;
            TableLayoutPanel tableLayoutPanel1;
            Label label2;
            Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            AccessTokenSourceLabel = new Label();
            SignOutButton = new Button();
            LookupUser = new Button();
            DeleteUser = new Button();
            ExpirePasswords = new Button();
            SignInCallToActionLabel = new Label();
            GraphResultsPanel = new Panel();
            GraphResultsTextBox = new TextBox();
            TenantID = new TextBox();
            ClientId = new TextBox();
            button3 = new Button();
            button4 = new Button();
            groupBox1 = new GroupBox();
            label7 = new Label();
            textBox3 = new TextBox();
            groupBox2 = new GroupBox();
            checkBox2 = new CheckBox();
            groupBox3 = new GroupBox();
            textBox5 = new TextBox();
            label8 = new Label();
            label3 = new Label();
            label4 = new Label();
            textBox4 = new TextBox();
            checkBox1 = new CheckBox();
            label5 = new Label();
            label6 = new Label();
            groupBox4 = new GroupBox();
            toolTip1 = new ToolTip(components);
            ExitButton = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            label2 = new Label();
            label1 = new Label();
            tableLayoutPanel1.SuspendLayout();
            GraphResultsPanel.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // ExitButton
            // 
            ExitButton.Anchor = AnchorStyles.Bottom;
            ExitButton.Location = new Point(302, 533);
            ExitButton.Margin = new Padding(2);
            ExitButton.Name = "ExitButton";
            ExitButton.Size = new Size(78, 20);
            ExitButton.TabIndex = 3;
            ExitButton.Text = "E&xit";
            ExitButton.UseVisualStyleBackColor = true;
            ExitButton.Click += ExitButton_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(AccessTokenSourceLabel, 1, 0);
            tableLayoutPanel1.Controls.Add(label2, 0, 0);
            tableLayoutPanel1.Controls.Add(SignOutButton, 2, 0);
            tableLayoutPanel1.Dock = DockStyle.Bottom;
            tableLayoutPanel1.Location = new Point(0, 276);
            tableLayoutPanel1.Margin = new Padding(2);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(668, 31);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // AccessTokenSourceLabel
            // 
            AccessTokenSourceLabel.Anchor = AnchorStyles.Left;
            AccessTokenSourceLabel.AutoSize = true;
            AccessTokenSourceLabel.Location = new Point(83, 8);
            AccessTokenSourceLabel.Margin = new Padding(0, 0, 2, 0);
            AccessTokenSourceLabel.Name = "AccessTokenSourceLabel";
            AccessTokenSourceLabel.Size = new Size(148, 15);
            AccessTokenSourceLabel.TabIndex = 1;
            AccessTokenSourceLabel.Text = "[Cached | Newly Acquired]";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new Point(2, 8);
            label2.Margin = new Padding(2, 0, 0, 0);
            label2.Name = "label2";
            label2.Size = new Size(81, 15);
            label2.TabIndex = 0;
            label2.Text = "Access Token:";
            // 
            // SignOutButton
            // 
            SignOutButton.Anchor = AnchorStyles.Right;
            SignOutButton.Location = new Point(588, 2);
            SignOutButton.Margin = new Padding(2);
            SignOutButton.Name = "SignOutButton";
            SignOutButton.Size = new Size(78, 27);
            SignOutButton.TabIndex = 2;
            SignOutButton.Text = "Sign &Out";
            SignOutButton.UseVisualStyleBackColor = true;
            SignOutButton.Click += SignOutButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Top;
            label1.Location = new Point(0, 0);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(149, 15);
            label1.TabIndex = 0;
            label1.Text = "Microsoft Graph Response:";
            // 
            // LookupUser
            // 
            LookupUser.Anchor = AnchorStyles.Top;
            LookupUser.Enabled = false;
            LookupUser.Location = new Point(17, 55);
            LookupUser.Margin = new Padding(2);
            LookupUser.Name = "LookupUser";
            LookupUser.Size = new Size(152, 26);
            LookupUser.TabIndex = 2;
            LookupUser.Text = "Lookup User(s)";
            LookupUser.UseVisualStyleBackColor = true;
            LookupUser.Click += FindUser_Click;
            // 
            // DeleteUser
            // 
            DeleteUser.Anchor = AnchorStyles.Top;
            DeleteUser.Enabled = false;
            DeleteUser.Location = new Point(17, 115);
            DeleteUser.Margin = new Padding(2);
            DeleteUser.Name = "DeleteUser";
            DeleteUser.Size = new Size(152, 26);
            DeleteUser.TabIndex = 4;
            DeleteUser.Text = "Delete User";
            DeleteUser.UseVisualStyleBackColor = true;
            DeleteUser.Click += Delete_click;
            // 
            // ExpirePasswords
            // 
            ExpirePasswords.Anchor = AnchorStyles.Top;
            ExpirePasswords.Enabled = false;
            ExpirePasswords.Location = new Point(17, 85);
            ExpirePasswords.Margin = new Padding(2);
            ExpirePasswords.Name = "ExpirePasswords";
            ExpirePasswords.Size = new Size(152, 26);
            ExpirePasswords.TabIndex = 0;
            ExpirePasswords.Text = "Expire Password(s)";
            ExpirePasswords.UseVisualStyleBackColor = true;
            ExpirePasswords.Click += ExpirePassword_Click;
            // 
            // SignInCallToActionLabel
            // 
            SignInCallToActionLabel.Anchor = AnchorStyles.Top;
            SignInCallToActionLabel.AutoSize = true;
            SignInCallToActionLabel.Location = new Point(142, 315);
            SignInCallToActionLabel.Margin = new Padding(2, 0, 2, 0);
            SignInCallToActionLabel.Name = "SignInCallToActionLabel";
            SignInCallToActionLabel.Size = new Size(385, 45);
            SignInCallToActionLabel.TabIndex = 2;
            SignInCallToActionLabel.Text = "This application will access Microsoft Graph, if you authorize it to do so.\r\n\r\nClick any button above to get started.";
            SignInCallToActionLabel.TextAlign = ContentAlignment.MiddleCenter;
            SignInCallToActionLabel.UseMnemonic = false;
            SignInCallToActionLabel.Click += SignInCallToActionLabel_Click;
            // 
            // GraphResultsPanel
            // 
            GraphResultsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            GraphResultsPanel.Controls.Add(label1);
            GraphResultsPanel.Controls.Add(GraphResultsTextBox);
            GraphResultsPanel.Controls.Add(tableLayoutPanel1);
            GraphResultsPanel.Location = new Point(8, 222);
            GraphResultsPanel.Margin = new Padding(2);
            GraphResultsPanel.Name = "GraphResultsPanel";
            GraphResultsPanel.Size = new Size(668, 307);
            GraphResultsPanel.TabIndex = 1;
            GraphResultsPanel.Visible = false;
            // 
            // GraphResultsTextBox
            // 
            GraphResultsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            GraphResultsTextBox.Location = new Point(9, 15);
            GraphResultsTextBox.Margin = new Padding(2);
            GraphResultsTextBox.Multiline = true;
            GraphResultsTextBox.Name = "GraphResultsTextBox";
            GraphResultsTextBox.ReadOnly = true;
            GraphResultsTextBox.ScrollBars = ScrollBars.Both;
            GraphResultsTextBox.Size = new Size(659, 257);
            GraphResultsTextBox.TabIndex = 1;
            GraphResultsTextBox.TextChanged += GraphResultsTextBox_TextChanged;
            // 
            // TenantID
            // 
            TenantID.Enabled = false;
            TenantID.Location = new Point(4, 13);
            TenantID.Name = "TenantID";
            TenantID.Size = new Size(396, 23);
            TenantID.TabIndex = 4;
            TenantID.Text = "Please Select a Tenant";
            TenantID.TextChanged += textBox1_TextChanged;
            // 
            // ClientId
            // 
            ClientId.Enabled = false;
            ClientId.Location = new Point(4, 37);
            ClientId.Name = "ClientId";
            ClientId.Size = new Size(396, 23);
            ClientId.TabIndex = 6;
            ClientId.TextChanged += textBox2_TextChanged;
            // 
            // button3
            // 
            button3.Location = new Point(25, 7);
            button3.Name = "button3";
            button3.Size = new Size(158, 23);
            button3.TabIndex = 8;
            button3.Text = "Set Tenant to Staging";
            button3.UseVisualStyleBackColor = true;
            button3.Click += Button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(25, 36);
            button4.Name = "button4";
            button4.Size = new Size(158, 23);
            button4.TabIndex = 9;
            button4.Text = "Set Tenant to Production";
            button4.UseVisualStyleBackColor = true;
            button4.Click += Button4_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(DeleteUser);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(LookupUser);
            groupBox1.Controls.Add(ExpirePasswords);
            groupBox1.Location = new Point(8, 65);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(186, 152);
            groupBox1.TabIndex = 10;
            groupBox1.TabStop = false;
            groupBox1.Text = "Commands";
            groupBox1.Enter += groupBox1_Enter;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(21, 26);
            label7.Name = "label7";
            label7.Size = new Size(144, 15);
            label7.TabIndex = 3;
            label7.Text = "Will open login if required";
            label7.Click += label7_Click;
            // 
            // textBox3
            // 
            textBox3.Enabled = false;
            textBox3.Location = new Point(68, 16);
            textBox3.Name = "textBox3";
            textBox3.RightToLeft = RightToLeft.No;
            textBox3.Size = new Size(374, 23);
            textBox3.TabIndex = 1;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(checkBox2);
            groupBox2.Controls.Add(groupBox3);
            groupBox2.Controls.Add(checkBox1);
            groupBox2.Location = new Point(200, 65);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(473, 152);
            groupBox2.TabIndex = 11;
            groupBox2.TabStop = false;
            groupBox2.Text = "Parameters";
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Checked = true;
            checkBox2.CheckState = CheckState.Checked;
            checkBox2.Location = new Point(187, 26);
            checkBox2.Name = "checkBox2";
            checkBox2.RightToLeft = RightToLeft.Yes;
            checkBox2.Size = new Size(140, 19);
            checkBox2.TabIndex = 13;
            checkBox2.Text = "Target Login Account";
            checkBox2.UseVisualStyleBackColor = true;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            checkBox2.CheckStateChanged += checkbox2_click;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(textBox5);
            groupBox3.Controls.Add(label8);
            groupBox3.Controls.Add(label3);
            groupBox3.Controls.Add(label4);
            groupBox3.Controls.Add(textBox3);
            groupBox3.Controls.Add(textBox4);
            groupBox3.Location = new Point(6, 47);
            groupBox3.Name = "groupBox3";
            groupBox3.RightToLeft = RightToLeft.Yes;
            groupBox3.Size = new Size(461, 99);
            groupBox3.TabIndex = 12;
            groupBox3.TabStop = false;
            groupBox3.Text = "Lookup / Reset User By Email or B2C ID";
            groupBox3.Enter += groupBox3_Enter;
            // 
            // textBox5
            // 
            textBox5.Enabled = false;
            textBox5.Location = new Point(68, 72);
            textBox5.Name = "textBox5";
            textBox5.RightToLeft = RightToLeft.No;
            textBox5.Size = new Size(374, 23);
            textBox5.TabIndex = 6;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(6, 75);
            label8.Name = "label8";
            label8.Size = new Size(49, 15);
            label8.TabIndex = 5;
            label8.Text = "Domain";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 19);
            label3.Name = "label3";
            label3.Size = new Size(36, 15);
            label3.TabIndex = 2;
            label3.Text = "Email";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 48);
            label4.Name = "label4";
            label4.Size = new Size(56, 15);
            label4.TabIndex = 4;
            label4.Text = "Object ID";
            // 
            // textBox4
            // 
            textBox4.Enabled = false;
            textBox4.Location = new Point(68, 45);
            textBox4.Name = "textBox4";
            textBox4.RightToLeft = RightToLeft.No;
            textBox4.Size = new Size(374, 23);
            textBox4.TabIndex = 3;
            textBox4.TextChanged += textBox4_TextChanged;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(6, 26);
            checkBox1.Name = "checkBox1";
            checkBox1.RightToLeft = RightToLeft.Yes;
            checkBox1.Size = new Size(159, 19);
            checkBox1.TabIndex = 5;
            checkBox1.Text = "Target All Users in Tenant";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            checkBox1.CheckStateChanged += checkbox_click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(406, 17);
            label5.Name = "label5";
            label5.Size = new Size(57, 15);
            label5.TabIndex = 12;
            label5.Text = "Tenant ID";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(406, 40);
            label6.Name = "label6";
            label6.Size = new Size(52, 15);
            label6.TabIndex = 13;
            label6.Text = "Client ID";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(label6);
            groupBox4.Controls.Add(TenantID);
            groupBox4.Controls.Add(label5);
            groupBox4.Controls.Add(ClientId);
            groupBox4.Location = new Point(192, -2);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(470, 66);
            groupBox4.TabIndex = 3;
            groupBox4.TabStop = false;
            // 
            // toolTip1
            // 
            toolTip1.ToolTipTitle = "Email Takes Prescedence";
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = ExitButton;
            ClientSize = new Size(685, 560);
            Controls.Add(groupBox4);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(GraphResultsPanel);
            Controls.Add(ExitButton);
            Controls.Add(SignInCallToActionLabel);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2);
            MaximizeBox = false;
            MinimumSize = new Size(565, 316);
            Name = "MainWindow";
            Text = "Refer-All Microsoft Graph Toolbox";
            Load += MainWindow_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            GraphResultsPanel.ResumeLayout(false);
            GraphResultsPanel.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private Label SignInCallToActionLabel;
        private Panel GraphResultsPanel;
        private Label AccessTokenSourceLabel;
        private Button SignOutButton;
        private TextBox GraphResultsTextBox;
        private TextBox textBox1;
        private TextBox ClientId;
        private Button button3;
        private Button button4;
        private GroupBox groupBox1;
        private TextBox textBox3;
        private GroupBox groupBox2;
        private Label label3;
        private CheckBox checkBox1;
        private Label label4;
        private TextBox textBox4;
        private GroupBox groupBox3;
        private Label label5;
        private Label label6;
        private Label label7;
        private GroupBox groupBox4;
        private ToolTip toolTip1;
        private CheckBox checkBox2;
        private TextBox textBox5;
        private Label label8;
        public Button ExpirePasswords;
        public TextBox TenantID;
        public Button LookupUser;
        public Button DeleteUser;
    }
}