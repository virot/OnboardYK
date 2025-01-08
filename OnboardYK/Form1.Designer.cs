using OnboardYK.Support;
using System.Security;
using System.Windows.Forms;
using Yubico.YubiKey;
using OnboardYK;
using Yubico.YubiKey.Piv;

namespace OnboardYK
{
    partial class Form1
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
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            tabYubikey = new TabControl();
            tabPage1 = new TabPage();
            buttonValidatePIN = new Button();
            labelCurrentPIN = new Label();
            textBoxCurrentPIN = new TextBox();
            labelYubiKey = new Label();
            comboBoxYubikey = new ComboBox();
            tabPage2 = new TabPage();
            buttonUpdatePINRetries = new Button();
            buttonChangePIN = new Button();
            textBoxPINPUKCount = new TextBox();
            textBoxPINConfirm = new TextBox();
            textBoxPINNew = new TextBox();
            labelPINPUKRetries = new Label();
            labelPINConfirm = new Label();
            labelNewPIN = new Label();
            label2 = new Label();
            labelPINGood = new Label();
            tabPage3 = new TabPage();
            buttonCertRenew = new Button();
            buttonCertNew = new Button();
            comboBoxProfiles = new ComboBox();
            label1 = new Label();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            quitToolStripMenuItem = new ToolStripMenuItem();
            advancedMenuItem = new ToolStripMenuItem();
            resetYubikeyPIVToolStripMenuItem1 = new ToolStripMenuItem();
            unlockProfilesMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            panel1 = new Panel();
            textBoxTemplate = new TextBox();
            buttonGenerateXML = new Button();
            textBoxName = new TextBox();
            textBoxCA = new TextBox();
            comboBoxTouchPolicy = new ComboBox();
            comboBoxPinPolicy = new ComboBox();
            comboBoxAlgorithm = new ComboBox();
            comboBoxSlot = new ComboBox();
            profileModelBindingSource = new BindingSource(components);
            statusStrip1.SuspendLayout();
            tabYubikey.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            menuStrip1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)profileModelBindingSource).BeginInit();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(32, 32);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 294);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 8, 0);
            statusStrip1.Size = new Size(402, 22);
            statusStrip1.TabIndex = 3;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.AutoSize = false;
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Alignment = ToolStripItemAlignment.Left;
            toolStripStatusLabel1.TextAlign = ContentAlignment.MiddleLeft;
            toolStripStatusLabel1.Size = new Size(350, 17);
            toolStripStatusLabel1.Text = "Select YubiKey and validate PIN to proceed";
            // 
            // tabYubikey
            // 
            tabYubikey.Controls.Add(tabPage1);
            tabYubikey.Controls.Add(tabPage2);
            tabYubikey.Controls.Add(tabPage3);
            tabYubikey.Location = new Point(12, 27);
            tabYubikey.Name = "tabYubikey";
            tabYubikey.SelectedIndex = 0;
            tabYubikey.Size = new Size(376, 257);
            tabYubikey.TabIndex = 4;
            tabYubikey.Selecting += tabControl_Selecting;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = SystemColors.Control;
            tabPage1.Controls.Add(buttonValidatePIN);
            tabPage1.Controls.Add(labelCurrentPIN);
            tabPage1.Controls.Add(textBoxCurrentPIN);
            tabPage1.Controls.Add(labelYubiKey);
            tabPage1.Controls.Add(comboBoxYubikey);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(368, 229);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Select Yubikey";
            // 
            // buttonValidatePIN
            // 
            buttonValidatePIN.Location = new Point(120, 60);
            buttonValidatePIN.Name = "buttonValidatePIN";
            buttonValidatePIN.Size = new Size(75, 23);
            buttonValidatePIN.TabIndex = 4;
            buttonValidatePIN.Text = "Validate PIN";
            buttonValidatePIN.UseVisualStyleBackColor = true;
            buttonValidatePIN.Click += buttonValidatePIN_Click;
            // 
            // labelCurrentPIN
            // 
            labelCurrentPIN.AutoSize = true;
            labelCurrentPIN.Location = new Point(10, 35);
            labelCurrentPIN.Name = "labelCurrentPIN";
            labelCurrentPIN.Size = new Size(69, 15);
            labelCurrentPIN.TabIndex = 3;
            labelCurrentPIN.Text = "Current PIN";
            // 
            // textBoxCurrentPIN
            // 
            textBoxCurrentPIN.Location = new Point(120, 35);
            textBoxCurrentPIN.MaxLength = 8;
            textBoxCurrentPIN.Name = "textBoxCurrentPIN";
            textBoxCurrentPIN.PasswordChar = '*';
            textBoxCurrentPIN.Size = new Size(200, 23);
            textBoxCurrentPIN.TabIndex = 2;
            // 
            // labelYubiKey
            // 
            labelYubiKey.AutoSize = true;
            labelYubiKey.Location = new Point(10, 10);
            labelYubiKey.Name = "labelYubiKey";
            labelYubiKey.Size = new Size(53, 15);
            labelYubiKey.TabIndex = 1;
            labelYubiKey.Text = "YubiKey:";
            // 
            // comboBoxYubikey
            // 
            comboBoxYubikey.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxYubikey.FormattingEnabled = true;
            comboBoxYubikey.Location = new Point(120, 10);
            comboBoxYubikey.Name = "comboBoxYubikey";
            comboBoxYubikey.Size = new Size(200, 23);
            comboBoxYubikey.TabIndex = 0;
            comboBoxYubikey.DropDown += comboBoxYubikey_updateList;
            comboBoxYubikey.SelectedIndexChanged += comboBoxYubikey_SelectedIndexChanged;
            // 
            // tabPage2
            // 
            tabPage2.BackColor = SystemColors.Control;
            tabPage2.Controls.Add(buttonUpdatePINRetries);
            tabPage2.Controls.Add(buttonChangePIN);
            tabPage2.Controls.Add(textBoxPINPUKCount);
            tabPage2.Controls.Add(textBoxPINConfirm);
            tabPage2.Controls.Add(textBoxPINNew);
            tabPage2.Controls.Add(labelPINPUKRetries);
            tabPage2.Controls.Add(labelPINConfirm);
            tabPage2.Controls.Add(labelNewPIN);
            tabPage2.Controls.Add(label2);
            tabPage2.Controls.Add(labelPINGood);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(368, 229);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "PIN | PUK";
            // 
            // buttonUpdatePINRetries
            // 
            buttonUpdatePINRetries.Location = new Point(140, 158);
            buttonUpdatePINRetries.Name = "buttonUpdatePINRetries";
            buttonUpdatePINRetries.Size = new Size(156, 23);
            buttonUpdatePINRetries.TabIndex = 9;
            buttonUpdatePINRetries.Text = "Update PIN | Block PUK";
            buttonUpdatePINRetries.UseVisualStyleBackColor = true;
            buttonUpdatePINRetries.Click += buttonUpdatePINRetries_Click;
            // 
            // buttonChangePIN
            // 
            buttonChangePIN.Location = new Point(140, 105);
            buttonChangePIN.Name = "buttonChangePIN";
            buttonChangePIN.Size = new Size(156, 23);
            buttonChangePIN.TabIndex = 8;
            buttonChangePIN.Text = "Change PIN";
            buttonChangePIN.UseVisualStyleBackColor = true;
            buttonChangePIN.Click += buttonChangePIN_Click;
            // 
            // textBoxPINPUKCount
            // 
            textBoxPINPUKCount.Enabled = false;
            textBoxPINPUKCount.Location = new Point(140, 128);
            textBoxPINPUKCount.Name = "textBoxPINPUKCount";
            textBoxPINPUKCount.Size = new Size(96, 23);
            textBoxPINPUKCount.TabIndex = 7;
            // 
            // textBoxPINConfirm
            // 
            textBoxPINConfirm.Location = new Point(140, 75);
            textBoxPINConfirm.Name = "textBoxPINConfirm";
            textBoxPINConfirm.PasswordChar = '*';
            textBoxPINConfirm.Size = new Size(100, 23);
            textBoxPINConfirm.TabIndex = 6;
            // 
            // textBoxPINNew
            // 
            textBoxPINNew.Location = new Point(138, 49);
            textBoxPINNew.Name = "textBoxPINNew";
            textBoxPINNew.PasswordChar = '*';
            textBoxPINNew.Size = new Size(100, 23);
            textBoxPINNew.TabIndex = 5;
            // 
            // labelPINPUKRetries
            // 
            labelPINPUKRetries.AutoSize = true;
            labelPINPUKRetries.Location = new Point(21, 131);
            labelPINPUKRetries.Name = "labelPINPUKRetries";
            labelPINPUKRetries.Size = new Size(95, 15);
            labelPINPUKRetries.TabIndex = 4;
            labelPINPUKRetries.Text = "PIN  | PUK retries";
            // 
            // labelPINConfirm
            // 
            labelPINConfirm.AutoSize = true;
            labelPINConfirm.Location = new Point(21, 75);
            labelPINConfirm.Name = "labelPINConfirm";
            labelPINConfirm.Size = new Size(76, 15);
            labelPINConfirm.TabIndex = 3;
            labelPINConfirm.Text = "Confirm PIN:";
            // 
            // labelNewPIN
            // 
            labelNewPIN.AutoSize = true;
            labelNewPIN.Location = new Point(21, 51);
            labelNewPIN.Name = "labelNewPIN";
            labelNewPIN.Size = new Size(56, 15);
            labelNewPIN.TabIndex = 2;
            labelNewPIN.Text = "New PIN:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(138, 24);
            label2.Name = "label2";
            label2.Size = new Size(38, 15);
            label2.TabIndex = 1;
            label2.Text = "label2";
            // 
            // labelPINGood
            // 
            labelPINGood.AutoSize = true;
            labelPINGood.Location = new Point(22, 27);
            labelPINGood.Name = "labelPINGood";
            labelPINGood.Size = new Size(38, 15);
            labelPINGood.TabIndex = 0;
            labelPINGood.Text = "label1";
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(buttonCertRenew);
            tabPage3.Controls.Add(buttonCertNew);
            tabPage3.Controls.Add(comboBoxProfiles);
            tabPage3.Controls.Add(label1);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(368, 229);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Certificate onboarding";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // buttonCertRenew
            // 
            buttonCertRenew.Location = new Point(80, 92);
            buttonCertRenew.Name = "buttonCertRenew";
            buttonCertRenew.Size = new Size(245, 23);
            buttonCertRenew.TabIndex = 3;
            buttonCertRenew.Text = "Reuse private key and get new certificate";
            buttonCertRenew.UseVisualStyleBackColor = true;
            buttonCertRenew.Click += buttonCertRenew_Click;
            // 
            // buttonCertNew
            // 
            buttonCertNew.Location = new Point(80, 63);
            buttonCertNew.Name = "buttonCertNew";
            buttonCertNew.Size = new Size(230, 23);
            buttonCertNew.TabIndex = 2;
            buttonCertNew.Text = "New private key and certificate";
            buttonCertNew.UseVisualStyleBackColor = true;
            buttonCertNew.Click += buttonCertNew_Click;
            // 
            // comboBoxProfiles
            // 
            comboBoxProfiles.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxProfiles.FormattingEnabled = true;
            comboBoxProfiles.Location = new Point(80, 21);
            comboBoxProfiles.Name = "comboBoxProfiles";
            comboBoxProfiles.Size = new Size(121, 23);
            comboBoxProfiles.TabIndex = 1;
            comboBoxProfiles.DropDown += comboBoxProfiles_updateList;
            comboBoxProfiles.SelectedIndexChanged += comboBoxProfiles_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(23, 24);
            label1.Name = "label1";
            label1.Size = new Size(44, 15);
            label1.TabIndex = 0;
            label1.Text = "Profile:";
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, advancedMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(402, 24);
            menuStrip1.TabIndex = 5;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { quitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "&File";
            // 
            // quitToolStripMenuItem
            // 
            quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            quitToolStripMenuItem.Size = new Size(97, 22);
            quitToolStripMenuItem.Text = "&Quit";
            quitToolStripMenuItem.Click += quitToolStripMenuItem_Click;
            // 
            // advancedMenuItem
            // 
            advancedMenuItem.DropDownItems.AddRange(new ToolStripItem[] { resetYubikeyPIVToolStripMenuItem1, unlockProfilesMenuItem });
            advancedMenuItem.Name = "advancedMenuItem";
            advancedMenuItem.Size = new Size(72, 20);
            advancedMenuItem.Text = "&Advanced";
            advancedMenuItem.DropDownOpening += advancedMenuItem_DropDownOpening;
            // 
            // resetYubikeyPIVToolStripMenuItem1
            // 
            resetYubikeyPIVToolStripMenuItem1.Name = "resetYubikeyPIVToolStripMenuItem1";
            resetYubikeyPIVToolStripMenuItem1.Size = new Size(167, 22);
            resetYubikeyPIVToolStripMenuItem1.Text = "&Reset Yubikey PIV";
            resetYubikeyPIVToolStripMenuItem1.Click += resetYubikeyPIVToolStripMenuItem1_Click;
            // 
            // unlockProfilesMenuItem
            // 
            unlockProfilesMenuItem.Name = "unlockProfilesMenuItem";
            unlockProfilesMenuItem.Size = new Size(167, 22);
            unlockProfilesMenuItem.Text = "&Unlock profiles";
            unlockProfilesMenuItem.Click += unlockProfilesMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(107, 22);
            aboutToolStripMenuItem.Text = "&About";
            // 
            // panel1
            // 
            panel1.Controls.Add(textBoxTemplate);
            panel1.Controls.Add(buttonGenerateXML);
            panel1.Controls.Add(textBoxName);
            panel1.Controls.Add(textBoxCA);
            panel1.Controls.Add(comboBoxTouchPolicy);
            panel1.Controls.Add(comboBoxPinPolicy);
            panel1.Controls.Add(comboBoxAlgorithm);
            panel1.Controls.Add(comboBoxSlot);
            panel1.Location = new Point(417, 51);
            panel1.Name = "panel1";
            panel1.Size = new Size(323, 229);
            panel1.TabIndex = 6;
            // 
            // textBoxTemplate
            // 
            textBoxTemplate.Location = new Point(185, 76);
            textBoxTemplate.Name = "textBoxTemplate";
            textBoxTemplate.Size = new Size(100, 23);
            textBoxTemplate.TabIndex = 1;
            textBoxTemplate.Text = "Templatename";
            // 
            // buttonGenerateXML
            // 
            buttonGenerateXML.Location = new Point(245, 194);
            buttonGenerateXML.Name = "buttonGenerateXML";
            buttonGenerateXML.Size = new Size(75, 23);
            buttonGenerateXML.TabIndex = 2;
            buttonGenerateXML.Text = "Generate XML";
            buttonGenerateXML.UseVisualStyleBackColor = true;
            buttonGenerateXML.Click += button3_Click;
            // 
            // textBoxName
            // 
            textBoxName.Location = new Point(42, 76);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new Size(100, 23);
            textBoxName.TabIndex = 1;
            textBoxName.Text = "Policy Name";
            // 
            // textBoxCA
            // 
            textBoxCA.Location = new Point(42, 105);
            textBoxCA.Name = "textBoxCA";
            textBoxCA.Size = new Size(264, 23);
            textBoxCA.TabIndex = 1;
            textBoxCA.Text = "CA Server in \"<Servername>\\<CA name>\" format";
            // 
            // comboBoxTouchPolicy
            // 
            comboBoxTouchPolicy.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxTouchPolicy.FormattingEnabled = true;
            comboBoxTouchPolicy.Items.AddRange(new object[] { PivTouchPolicy.None, PivTouchPolicy.Never, PivTouchPolicy.Always, PivTouchPolicy.Cached, PivTouchPolicy.Default });
            comboBoxTouchPolicy.Location = new Point(185, 47);
            comboBoxTouchPolicy.Name = "comboBoxTouchPolicy";
            comboBoxTouchPolicy.Size = new Size(121, 23);
            comboBoxTouchPolicy.TabIndex = 0;
            // 
            // comboBoxPinPolicy
            // 
            comboBoxPinPolicy.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxPinPolicy.FormattingEnabled = true;
            comboBoxPinPolicy.Items.AddRange(new object[] { PivPinPolicy.None, PivPinPolicy.Never, PivPinPolicy.Once, PivPinPolicy.Always, PivPinPolicy.MatchOnce, PivPinPolicy.MatchAlways, PivPinPolicy.Default });
            comboBoxPinPolicy.Location = new Point(42, 47);
            comboBoxPinPolicy.Name = "comboBoxPinPolicy";
            comboBoxPinPolicy.Size = new Size(121, 23);
            comboBoxPinPolicy.TabIndex = 0;
            // 
            // comboBoxAlgorithm
            // 
            comboBoxAlgorithm.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxAlgorithm.FormattingEnabled = true;
            comboBoxAlgorithm.Location = new Point(185, 18);
            comboBoxAlgorithm.Name = "comboBoxAlgorithm";
            comboBoxAlgorithm.Size = new Size(121, 23);
            comboBoxAlgorithm.TabIndex = 0;
            // 
            // comboBoxSlot
            // 
            comboBoxSlot.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxSlot.FormattingEnabled = true;
            comboBoxSlot.Items.AddRange(new object[] { "154", "156", "157", "158" });
            comboBoxSlot.Location = new Point(42, 18);
            comboBoxSlot.Name = "comboBoxSlot";
            comboBoxSlot.Size = new Size(121, 23);
            comboBoxSlot.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(402, 316);
            Controls.Add(panel1);
            Controls.Add(tabYubikey);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip1;
            Margin = new Padding(2, 1, 2, 1);
            Name = "Form1";
            Text = "OnboardYK";
            Load += Form1_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            tabYubikey.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)profileModelBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private TabControl tabYubikey;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem quitToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private Label labelYubiKey;
        private ComboBox comboBoxYubikey;
        private Label labelCurrentPIN;
        private TextBox textBoxCurrentPIN;
        private Button buttonValidatePIN;
        private Button buttonUpdatePINRetries;
        private Button buttonChangePIN;
        private TextBox textBoxPINPUKCount;
        private TextBox textBoxPINConfirm;
        private TextBox textBoxPINNew;
        private Label labelPINPUKRetries;
        private Label labelPINConfirm;
        private Label labelNewPIN;
        private Label label2;
        private Label labelPINGood;
        private ToolStripMenuItem advancedMenuItem;
        private ToolStripMenuItem resetYubikeyPIVToolStripMenuItem1;
        private ToolStripMenuItem unlockProfilesMenuItem;
        private Button buttonCertRenew;
        private Button buttonCertNew;
        private ComboBox comboBoxProfiles;
        private Label label1;
        private Panel panel1;
        private TextBox textBoxTemplate;
        private TextBox textBoxCA;
        private ComboBox comboBoxTouchPolicy;
        private ComboBox comboBoxPinPolicy;
        private ComboBox comboBoxAlgorithm;
        private ComboBox comboBoxSlot;
        private Button buttonGenerateXML;
        private TextBox textBoxName;
        private BindingSource profileModelBindingSource;
    }



}
