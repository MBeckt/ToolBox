
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
            Button ExitButton;
            TableLayoutPanel tableLayoutPanel1;
            Label label2;
            Button SignInButton;
            Label label1;
            Button button5;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            AccessTokenSourceLabel = new Label();
            SignOutButton = new Button();
            SignInCallToActionLabel = new Label();
            GraphResultsPanel = new Panel();
            GraphResultsTextBox = new TextBox();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            button3 = new Button();
            button4 = new Button();
            groupBox1 = new GroupBox();
            textBox3 = new TextBox();
            groupBox2 = new GroupBox();
            groupBox3 = new GroupBox();
            label3 = new Label();
            label4 = new Label();
            textBox4 = new TextBox();
            checkBox1 = new CheckBox();
            label5 = new Label();
            label6 = new Label();
            ExitButton = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            label2 = new Label();
            SignInButton = new Button();
            label1 = new Label();
            button5 = new Button();
            tableLayoutPanel1.SuspendLayout();
            GraphResultsPanel.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
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
            tableLayoutPanel1.Location = new Point(0, 288);
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
            AccessTokenSourceLabel.Location = new Point(82, 8);
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
            label2.Size = new Size(80, 15);
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
            // SignInButton
            // 
            SignInButton.Anchor = AnchorStyles.Top;
            SignInButton.Location = new Point(17, 83);
            SignInButton.Margin = new Padding(2);
            SignInButton.Name = "SignInButton";
            SignInButton.Size = new Size(152, 26);
            SignInButton.TabIndex = 0;
            SignInButton.Text = "Reset Password(s)";
            SignInButton.UseVisualStyleBackColor = true;
            SignInButton.Click += SignInButton_Click;
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
            // button5
            // 
            button5.Anchor = AnchorStyles.Top;
            button5.Location = new Point(17, 32);
            button5.Margin = new Padding(2);
            button5.Name = "button5";
            button5.Size = new Size(152, 26);
            button5.TabIndex = 2;
            button5.Text = "Lookup User";
            button5.UseVisualStyleBackColor = true;
            button5.Click += FindEmailButton_Click;
            // 
            // SignInCallToActionLabel
            // 
            SignInCallToActionLabel.Anchor = AnchorStyles.Top;
            SignInCallToActionLabel.AutoSize = true;
            SignInCallToActionLabel.Location = new Point(142, 222);
            SignInCallToActionLabel.Margin = new Padding(2, 0, 2, 0);
            SignInCallToActionLabel.Name = "SignInCallToActionLabel";
            SignInCallToActionLabel.Size = new Size(384, 45);
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
            GraphResultsPanel.Location = new Point(8, 210);
            GraphResultsPanel.Margin = new Padding(2);
            GraphResultsPanel.Name = "GraphResultsPanel";
            GraphResultsPanel.Size = new Size(668, 319);
            GraphResultsPanel.TabIndex = 1;
            GraphResultsPanel.Visible = false;
            // 
            // GraphResultsTextBox
            // 
            GraphResultsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            GraphResultsTextBox.Location = new Point(6, 17);
            GraphResultsTextBox.Margin = new Padding(2);
            GraphResultsTextBox.Multiline = true;
            GraphResultsTextBox.Name = "GraphResultsTextBox";
            GraphResultsTextBox.ReadOnly = true;
            GraphResultsTextBox.ScrollBars = ScrollBars.Both;
            GraphResultsTextBox.Size = new Size(659, 269);
            GraphResultsTextBox.TabIndex = 1;
            GraphResultsTextBox.TextChanged += GraphResultsTextBox_TextChanged;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(200, 8);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(386, 23);
            textBox1.TabIndex = 4;
            textBox1.Text = "2fdbaf70-405c-420d-81e6-0d5391cd6245";
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(200, 36);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(386, 23);
            textBox2.TabIndex = 6;
            textBox2.Text = "1be0f404-8ead-476c-bc75-72a6bd2ac06d";
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // button3
            // 
            button3.Location = new Point(25, 7);
            button3.Name = "button3";
            button3.Size = new Size(158, 23);
            button3.TabIndex = 8;
            button3.Text = "Set Tenant to Staging";
            button3.UseVisualStyleBackColor = true;
            button3.Click += Button4_Click;
            // 
            // button4
            // 
            button4.Location = new Point(25, 36);
            button4.Name = "button4";
            button4.Size = new Size(158, 23);
            button4.TabIndex = 9;
            button4.Text = "Set Tenant to Production";
            button4.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button5);
            groupBox1.Controls.Add(SignInButton);
            groupBox1.Location = new Point(8, 65);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(186, 130);
            groupBox1.TabIndex = 10;
            groupBox1.TabStop = false;
            groupBox1.Text = "Commands";
            groupBox1.Enter += groupBox1_Enter;
            // 
            // textBox3
            // 
            textBox3.Enabled = false;
            textBox3.Location = new Point(68, 16);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(374, 23);
            textBox3.TabIndex = 1;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(groupBox3);
            groupBox2.Controls.Add(checkBox1);
            groupBox2.Location = new Point(200, 65);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(473, 130);
            groupBox2.TabIndex = 11;
            groupBox2.TabStop = false;
            groupBox2.Text = "Parameters";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(label3);
            groupBox3.Controls.Add(label4);
            groupBox3.Controls.Add(textBox3);
            groupBox3.Controls.Add(textBox4);
            groupBox3.Location = new Point(6, 47);
            groupBox3.Name = "groupBox3";
            groupBox3.RightToLeft = RightToLeft.Yes;
            groupBox3.Size = new Size(461, 75);
            groupBox3.TabIndex = 12;
            groupBox3.TabStop = false;
            groupBox3.Text = "Lookup / Reset User By Email or B2C ID";
            groupBox3.Enter += groupBox3_Enter;
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
            textBox4.Size = new Size(374, 23);
            textBox4.TabIndex = 3;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Checked = true;
            checkBox1.CheckState = CheckState.Checked;
            checkBox1.Location = new Point(6, 22);
            checkBox1.Name = "checkBox1";
            checkBox1.RightToLeft = RightToLeft.Yes;
            checkBox1.Size = new Size(71, 19);
            checkBox1.TabIndex = 5;
            checkBox1.Text = "All Users";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckStateChanged += checkbox_click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(592, 11);
            label5.Name = "label5";
            label5.Size = new Size(56, 15);
            label5.TabIndex = 12;
            label5.Text = "Tenant ID";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(592, 39);
            label6.Name = "label6";
            label6.Size = new Size(52, 15);
            label6.TabIndex = 13;
            label6.Text = "Client ID";
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = ExitButton;
            ClientSize = new Size(685, 560);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
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
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
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
        private TextBox textBox2;
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
    }
}