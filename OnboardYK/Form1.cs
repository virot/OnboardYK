using OnboardYK.Models;
using OnboardYK.Support;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
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

            /*comboBoxTouchPolicy.Items.AddRange(new Object[]
            {
                new YubiKeyTouchPolicyItem(PivTouchPolicy.Never),
                new YubiKeyTouchPolicyItem(PivTouchPolicy.Always),
                new YubiKeyTouchPolicyItem(PivTouchPolicy.Cached),
            });
            
            comboBoxPinPolicy.Items.AddRange(new Object[]
            {
                new YubiKeyPINPolicyItem(PivPinPolicy.Always),
                new YubiKeyPINPolicyItem(PivPinPolicy.Never),
                new YubiKeyPINPolicyItem(PivPinPolicy.Once),
                new YubiKeyPINPolicyItem(PivPinPolicy.MatchAlways),
                new YubiKeyPINPolicyItem(PivPinPolicy.MatchOnce)
            });
            */

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

        private void advancedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Do allow manual editing of the configuration
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                MessageBox.Show("Going really advanced", "Advanced", MessageBoxButtons.OK);
            }
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
            using (var pivSession = new PivSession((YubiKeyDevice)_yubiKeyDevice!))
            {
                pivSession.KeyCollector = _ykKeyCollector.YKKeyCollectorDelegate;
                if (_currentPIN is not null)
                {
                    if (pivSession.TryVerifyPin() == true)
                    {
                        toolStripStatusLabel1.Text = "PIN verified";

                        // Success
                        buttonValidatePIN.Enabled = false;
                        textBoxCurrentPIN.Text = "";
                        textBoxCurrentPIN.Enabled = false;
                        tabYubikey.SelectTab(tabPage2);

                        int pinRetries = pivSession.GetMetadata(PivSlot.Pin).RetryCount;
                        int pukRetries = pivSession.GetMetadata(PivSlot.Puk).RetriesRemaining;
                        textBoxPINPUKCount.Text = $"{pinRetries} / {pukRetries}";
                    }
                    else
                    {
                        textBoxCurrentPIN.Text = "";
                        _currentPIN.Clear();
                    }
                }
            }
        }
        private void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if ((e.TabPage == tabPage2 || e.TabPage == tabPage3) && _currentPIN is null)
            {
                //e.Cancel = true; // Cancel the selection of tabPage2
                UpdateStatusLabel("Please verify PIN before switching pages.");
            }
        }

        private void resetYubikeyPIVToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want reset the PIV on this YubiKey?", "Reset YubiKey PIV", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                MessageBox.Show("Not implemented yet.", "Not implmented", MessageBoxButtons.OKCancel);
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
                        MessageBox.Show($"Not implemented yet. {Marshal.PtrToStringUni(Marshal.SecureStringToGlobalAllocUnicode(_currentPIN!))}", "CurrentPIN", MessageBoxButtons.OKCancel);
                        MessageBox.Show($"Not implemented yet. {Marshal.PtrToStringUni(Marshal.SecureStringToGlobalAllocUnicode(_newPIN!))}", "NewPIN", MessageBoxButtons.OKCancel);
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
                    Slot = 0x9A,
                    Algorithm = PivAlgorithm.Rsa2048,
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
    }
}
