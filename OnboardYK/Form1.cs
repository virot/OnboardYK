using OnboardYK.Models;
using OnboardYK.Support;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using Yubico.YubiKey;
using Yubico.YubiKey.Piv;

namespace OnboardYK
{
    public partial class Form1 : Form
    {
        public YubiKeyDevice? _yubiKeyDevice;
        public SecureString? _currentPIN;
        public SecureString? _newPIN;
        public YKKeyCollector _ykKeyCollector;
        public ProfileModel _profileModel;
        public byte[]? _pivManagementKey = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };

        public Form1()
        {
            InitializeComponent();
            _ykKeyCollector = new YKKeyCollector(form: this);

            // Load the Yubikey stuff
            comboBoxTouchPolicy.DataSource = Enum.GetValues(typeof(PivTouchPolicy));
            comboBoxPinPolicy.DataSource = Enum.GetValues(typeof(PivPinPolicy));
            comboBoxAlgorithm.DataSource = Enum.GetValues(typeof(PivAlgorithm));
            // Load the profiles
            string fileName = "OnboardYK.xml";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file {fileName} was not found in the application directory.");
            }

            _profileModel = ProfileModel.LoadFromFile(filePath);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxYubikey_updateList(sender, e);
            comboBoxProfiles_updateList(sender, e);
        }

        private void advancedMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            unlockProfilesMenuItem.Visible = (Control.ModifierKeys & (Keys.Shift | Keys.Control)) == (Keys.Shift | Keys.Control);
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to quit?", "Quit", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void comboBoxYubikey_updateList(object sender, EventArgs e)
        {
            IEnumerable<IYubiKeyDevice> allYubiKeys = YubiKeyDevice.FindAll();
            List<int> allSerialNumbers;
            if (allYubiKeys.Count() > 0)
            {
                allSerialNumbers = allYubiKeys.Select(yubiKey => yubiKey.SerialNumber ?? 0).ToList();
            }
            else
            {
                allSerialNumbers = new List<int> { 0 };
            }
            // Go through the list of the current YubiKeys and remove if they are no longer present.
            foreach (YubiKeyDeviceItem yubiKeyItem in comboBoxYubikey.Items)
            {
                if (allYubiKeys.Any(yubiKey => yubiKey.SerialNumber == yubiKeyItem.SerialNumber) == false)
                {
                    comboBoxYubikey.Items.Remove(yubiKeyItem);
                }
            }
            // Add any new YubiKeys to the list
            foreach (IYubiKeyDevice yubiKey in allYubiKeys)
            {
                if (comboBoxYubikey.Items.Cast<YubiKeyDeviceItem>().Any(yubiKeyItem => yubiKeyItem.SerialNumber == yubiKey.SerialNumber) == false)
                {
                    comboBoxYubikey.Items.Add(new YubiKeyDeviceItem($"{yubiKey.FormFactor} [{yubiKey.SerialNumber}]", yubiKey.SerialNumber));
                }
            }
            if (comboBoxYubikey.Items.Count == 0)
            {
                comboBoxYubikey.Items.Add(new YubiKeyDeviceItem("No YubiKey found", 0));
                UpdateStatusLabel("No YubiKey found", Color.Red);
                comboBoxYubikey.SelectedIndex = 0;
                return;
            }
            if (comboBoxYubikey.SelectedIndex == -1)
            {
                comboBoxYubikey.SelectedIndex = 0;
            }
        }
        private void comboBoxYubikey_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((YubiKeyDeviceItem)comboBoxYubikey.SelectedItem!).SerialNumber != 0)
            {
                _yubiKeyDevice = (YubiKeyDevice?)YubiKeyDevice.FindAll().First(yk => yk.SerialNumber == ((YubiKeyDeviceItem)comboBoxYubikey.SelectedItem!).SerialNumber);
                buttonValidatePIN.Enabled = true;
                _currentPIN = null;
                textBoxCurrentPIN.Text = "";
                textBoxCurrentPIN.Enabled = true;
            }
            else
            {
                _yubiKeyDevice = null;
                buttonValidatePIN.Enabled = false;
                textBoxCurrentPIN.Text = "";
                textBoxCurrentPIN.Enabled = false;
            }

        }
        private void comboBoxProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_profileModel.Profiles is not null && comboBoxProfiles.SelectedItem is not null)
            {
                ProfileModel.Profile selectedProfile = _profileModel.Profiles.First(profile => profile.Name == (string)comboBoxProfiles.SelectedItem);
                textBoxName.Text = selectedProfile.Name;
                textBoxTemplate.Text = selectedProfile.Template;
                textBoxCA.Text = selectedProfile.CA;
                comboBoxTouchPolicy.SelectedItem = selectedProfile.TouchPolicy;
                comboBoxPinPolicy.SelectedItem = selectedProfile.PinPolicy;
                comboBoxAlgorithm.SelectedItem = selectedProfile.Algorithm;
                comboBoxSlot.SelectedItem = selectedProfile.Slot.ToString();
            }
        }
        private void comboBoxProfiles_updateList(object sender, EventArgs e)
        {
            bool showAll = ((Control.ModifierKeys & Keys.Shift) == (Keys.Shift) || _profileModel.ShowAllProfiles == true);
            List<String> visibleProfiles = new List<String>();
            if (_profileModel.Profiles is not null)
            {
                visibleProfiles = _profileModel.Profiles.Where(visible => visible.AlwaysVisible == true || showAll).Select(profile => profile.Name).ToList();
            }

            foreach (String profile in visibleProfiles)
            {
                if (!comboBoxProfiles.Items.Contains(profile))
                {
                    comboBoxProfiles.Items.Add(profile);
                }
            }
            foreach (String profile in comboBoxProfiles.Items)
            {
                if (!visibleProfiles.Contains(profile))
                {
                    comboBoxProfiles.Items.Remove(profile);
                }
            }
            if (comboBoxProfiles.Items.Count == 1)
            {
                comboBoxProfiles.SelectedIndex = 0;
            }
        }

        public void UpdateStatusLabel(string message, Color? backgroundColor = null)
        {
            if (backgroundColor is not null)
            {
                toolStripStatusLabel1.BackColor = (Color)backgroundColor;
            }
            else
            {
                toolStripStatusLabel1.BackColor = Control.DefaultBackColor;
            }
            toolStripStatusLabel1.Text = message;
        }

        private void buttonValidatePIN_Click(object sender, EventArgs e)
        {
            if (textBoxCurrentPIN.TextLength >= 6 && textBoxCurrentPIN.TextLength <= 8)
            {
                _currentPIN = new NetworkCredential("", textBoxCurrentPIN.Text).SecurePassword;
            }
            else
            {
                toolStripStatusLabel1.Text = "PIN must be between 6 and 8 characters";
                return;
            }
            
            if (_currentPIN is not null)
            {
                try
                {
                    bool pinVerified = false;
                    int pinRetries = 0;
                    int pukRetries = 0;
                    using (var pivSession = new PivSession((YubiKeyDevice)_yubiKeyDevice!))
                    {
                        pivSession.KeyCollector = _ykKeyCollector.YKKeyCollectorDelegate;
                        pinVerified = pivSession.TryVerifyPin();
                        pinRetries = pivSession.GetMetadata(PivSlot.Pin).RetryCount;
                        pukRetries = pivSession.GetMetadata(PivSlot.Puk).RetriesRemaining;

                    }

                    if (pinVerified == true)
                    {
                        toolStripStatusLabel1.Text = "PIN verified";

                        // Success
                        buttonValidatePIN.Enabled = false;
                        textBoxCurrentPIN.Text = "";
                        textBoxCurrentPIN.Enabled = false;
                        tabYubikey.SelectTab(tabPage2);

                        textBoxPINPUKCount.Text = $"{pinRetries} / {pukRetries}";
                        checkIfYubiKeyFollowsRules(this);
                    }
                    else
                    {
                        textBoxCurrentPIN.Text = "";
                        UpdateStatusLabel("Incorrect PIN entered");
                        _currentPIN.Clear();
                    }
                }
                catch (Exception err)
                {
                    UpdateStatusLabel($"Failed to verify PIN:, '{err.Message}'", Color.Red);
                }
            }
        }
        private void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if ((e.TabPage == tabPage2 || e.TabPage == tabPage3) && _currentPIN is null)
            {
                e.Cancel = true; // Cancel the selection of tabPage2
                UpdateStatusLabel("Please verify PIN before switching pages.");
                return;
            }
            if ((e.TabPage == tabPage3) && _profileModel.RequireComplexPIN && isPINComplex.TestPIN(_currentPIN) == false)
            {
                e.Cancel = true; // Cancel the selection of tabPage2
                UpdateStatusLabel("Please update PIN before enrolling");
                return;
            }
            using (var pivSession = new PivSession((YubiKeyDevice)_yubiKeyDevice!))
            {
                if ((e.TabPage == tabPage3) && _profileModel.RequireBlockedPUK && pivSession.GetMetadata(PivSlot.Puk).RetriesRemaining != 0)
                {
                    e.Cancel = true; // Cancel the selection of tabPage2
                    UpdateStatusLabel("Please block PUK before enrolling");
                    return;
                }
            }

        }

        private void resetYubikeyPIVToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want reset the PIV on this YubiKey?", "Reset YubiKey PIV", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (var pivSession = new PivSession((YubiKeyDevice)_yubiKeyDevice!))
                {
                    pivSession.ResetApplication();
                    buttonValidatePIN.Enabled = true;
                    if (_currentPIN is not null)
                    {
                        _currentPIN = null;
                    }
                    textBoxCurrentPIN.Text = "";
                    textBoxCurrentPIN.Enabled = true;
                    tabYubikey.SelectTab(tabPage1);

                }
            }
        }

        private void buttonUpdatePINRetries_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want update the PIN to 8 retries, disable the PUK and PIN protect the management key?", "Lock YubiKey PIV", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (var pivSession = new PivSession((YubiKeyDevice)_yubiKeyDevice!))
                {
                    pivSession.KeyCollector = _ykKeyCollector.YKKeyCollectorDelegate;

                    if (pivSession.GetMetadata(PivSlot.Pin).RetryCount != 8)
                    {
                        pivSession.ChangePinAndPukRetryCounts(0x8, 0x3);
                        _newPIN = _currentPIN;
                        _currentPIN = new NetworkCredential("", "123456").SecurePassword;
                        pivSession.ChangePin();
                        _currentPIN = _newPIN;
                        _newPIN = null;
                    }
                    if (pivSession.GetPinOnlyMode() != PivPinOnlyMode.PinProtected)
                    {
                        PivAlgorithm mgmtKeyAlgorithm = ((YubiKeyDevice)_yubiKeyDevice!).HasFeature(YubiKeyFeature.PivAesManagementKey) ? PivAlgorithm.Aes256 : PivAlgorithm.TripleDes;
                        pivSession.SetPinOnlyMode(PivPinOnlyMode.PinProtected, mgmtKeyAlgorithm);
                    }
                    int pinRetries = pivSession.GetMetadata(PivSlot.Pin).RetryCount;
                    int pukRetries = pivSession.GetMetadata(PivSlot.Puk).RetriesRemaining;
                    textBoxPINPUKCount.Text = $"{pinRetries} / {pukRetries}";
                }
                checkIfYubiKeyFollowsRules(this);
            }
        }

     

        private void checkIfYubiKeyFollowsRules(Form form)
        {
            using (var pivSession = new PivSession((YubiKeyDevice)_yubiKeyDevice!))
            {
                pivSession.KeyCollector = _ykKeyCollector.YKKeyCollectorDelegate;

                if (_profileModel.RequireComplexPIN == true && isPINComplex.TestPIN(_currentPIN) == false)
                {
                    UpdateStatusLabel("PIN not complex enough", Color.Red);
                    label2.Text = "PIN not complex enough, please change";
                    return;
                }
                if (pivSession.GetMetadata(PivSlot.Pin).RetryCount != _profileModel.RetriesPIN)
                {
                    UpdateStatusLabel("PIN retries not 8, please correct", Color.Red);
                    label2.Text = "Please update number of possible retires";
                    return;
                }
                if (_profileModel.RequireBlockedPUK && pivSession.GetMetadata(PivSlot.Puk).RetriesRemaining != 0)
                {
                    UpdateStatusLabel("PUK must be blocked, please correct", Color.Red);
                    label2.Text = "PUK is not blocked, please block PUK.";
                    return;
                }

                label2.Text = "YubiKey is following requirements";
                UpdateStatusLabel("Proceed to enroll", null);

            }
        }

        private void buttonChangePIN_Click(object sender, EventArgs e)
        {
            if (textBoxPINNew.Text != textBoxPINConfirm.Text)
            {
                toolStripStatusLabel1.Text = "PIN New must match PIN confirm";
                return;
            }
            if (textBoxPINNew.TextLength >= 6 && textBoxPINNew.TextLength <= 8)
            {
                _newPIN = new NetworkCredential("", textBoxPINNew.Text).SecurePassword;
                using (var pivSession = new PivSession((YubiKeyDevice)_yubiKeyDevice!))
                {
                    pivSession.KeyCollector = _ykKeyCollector.YKKeyCollectorDelegate;
                    if (pivSession.TryChangePin() == true)
                    {
                        _currentPIN = _newPIN;
                        _newPIN = null;
                        toolStripStatusLabel1.Text = "PIN changed";
                        textBoxPINNew.Text = "";
                        textBoxPINConfirm.Text = "";
                        checkIfYubiKeyFollowsRules(this);
                    };
                }
            }
            else
            {
                toolStripStatusLabel1.Text = "PIN must be between 6 and 8 characters";
                return;
            }
        }

        private void unlockProfilesMenuItem_Click(object sender, EventArgs e)
        {
            this.Width = 1400;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ProfileModel profileModel = new ProfileModel();
            profileModel.Profiles = new List<ProfileModel.Profile>
            {
                new ProfileModel.Profile
                {
                    Slot = byte.Parse((string)(comboBoxSlot.SelectedItem ?? "0")),
                    Algorithm = (PivAlgorithm)comboBoxAlgorithm.SelectedItem!,
                    PinPolicy = (PivPinPolicy)comboBoxPinPolicy.SelectedItem!,
                    TouchPolicy = (PivTouchPolicy)comboBoxTouchPolicy.SelectedItem!,
                    Template = textBoxTemplate.Text,
                    CA = textBoxCA.Text,
                    Name = textBoxName.Text,
                    AlwaysVisible = true
                }
            };
            string xmloutput = profileModel.SaveToString();
            MessageBox.Show(xmloutput, "Profile", MessageBoxButtons.OKCancel);
        }

        private void buttonCertNew_Click(object sender, EventArgs e)
        {
            if (comboBoxSlot.Text.Length >= 2)
            { }
            else
            {
                MessageBox.Show("Slot must be selected", "No slot selected", MessageBoxButtons.OK);
                return;
            }
            using (var pivSession = new PivSession((YubiKeyDevice)_yubiKeyDevice!))
            {
                byte slot = byte.Parse((string)comboBoxSlot.SelectedItem!);
                pivSession.KeyCollector = _ykKeyCollector.YKKeyCollectorDelegate;

                try
                {
                    X509Certificate2 certificate = pivSession.GetCertificate(slot);
                    if (MessageBox.Show(text: $"There already exists a certificate in the slot 0x{slot.ToString("X2")} issued to {certificate.Subject}, do you want to overwrite it?", caption: $"Certificate exists in slot 0x{slot.ToString("X2")}", buttons: MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        UpdateStatusLabel("Certificate already exists aborting");
                        return;
                    }
                }
                catch
                {
                }
                UpdateStatusLabel("Starting to generate new private key");
                var publickey = pivSession.GenerateKeyPair(slotNumber:
                    byte.Parse((string)comboBoxSlot.SelectedItem!),
                    algorithm: (PivAlgorithm)comboBoxAlgorithm.SelectedItem!,
                    pinPolicy: (PivPinPolicy)comboBoxPinPolicy.SelectedItem!,
                    touchPolicy: (PivTouchPolicy)comboBoxTouchPolicy.SelectedItem!
                );
            }

            buttonCertRenew_Click(sender, e);
        }

        private void buttonCertRenew_Click(object sender, EventArgs e)
        {
            string csrPemData;
            X509Certificate2 certificate;


            if (comboBoxSlot.Text.Length >= 2)
            { }
            else
            {
                MessageBox.Show("Slot must be selected", "No slot selected", MessageBoxButtons.OK);
                return;
            }

            byte slot = byte.Parse((string)comboBoxSlot.SelectedItem!);
            using (var pivSession = new PivSession((YubiKeyDevice)_yubiKeyDevice!))
            {
                pivSession.KeyCollector = _ykKeyCollector.YKKeyCollectorDelegate;
                PivPublicKey? publicKey;
                try
                {
                    publicKey = pivSession.GetMetadata(byte.Parse((string)comboBoxSlot.SelectedItem!)).PublicKey;
                }
                catch
                {
                    MessageBox.Show($"No public key found in slot {slot.ToString("X2")}", "No public key", MessageBoxButtons.OK);
                    UpdateStatusLabel($"No public key found slot {slot.ToString("X2")}, unable to enroll", Control.DefaultBackColor);
                    return;
                }
            }

            try
            {
                UpdateStatusLabel("Please press YubiKey if it starts to blink", Color.Yellow);
                Thread.Sleep(15);
                csrPemData = CertificateManagement.GenerateCertificateSigningRequest(slot: slot, yubiKeyDevice: _yubiKeyDevice!, yKKeyCollector: _ykKeyCollector);
                UpdateStatusLabel("Certificate Signing Request created", Control.DefaultBackColor);
            }
            catch
            {
                UpdateStatusLabel("Failed to generate CSR", Color.Red);
                MessageBox.Show("Failed to generate CSR", "Failed to generate CSR", MessageBoxButtons.OK);
                return;
            }
            try
            {
                string certificateAuthority = textBoxCA.Text;
                string template = textBoxTemplate.Text;
                string response = CertificateManagement.SubmitRequest(caServer: certificateAuthority, csr: csrPemData, template: template);
                certificate = new X509Certificate2(Encoding.ASCII.GetBytes(response));
                UpdateStatusLabel("Certificate has been signed by the Certificate Authority", Control.DefaultBackColor);
            }
            catch
            {
                UpdateStatusLabel("Certificate Authority did not approve of the certificate request.", Color.Red);
                //MessageBox.Show("Failed to generate CSR", "Failed to generate CSR", MessageBoxButtons.OK);
                return;
            }
            try
            {
                CertificateManagement.InstallCertificate(slot: slot, certificate: certificate, yubiKeyDevice: _yubiKeyDevice!, yKKeyCollector: _ykKeyCollector);
                UpdateStatusLabel("Certificate has been installed into the YubiKey");
            }
            catch
            {
                UpdateStatusLabel("Failed install the Certificate", Color.Red);
                MessageBox.Show(text: "Failed to install the certificate", caption: "Failed to install the certificate", buttons: MessageBoxButtons.OK);
                return;
            }
        }
    }
}
