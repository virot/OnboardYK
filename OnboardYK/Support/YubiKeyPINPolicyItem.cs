using Yubico.YubiKey.Piv;

namespace OnboardYK.Support
{
    internal class YubiKeyPINPolicyItem
    {
        public string DisplayTitle { get; set; }
        public PivPinPolicy PivPinPolicy { get; set; }

        public YubiKeyPINPolicyItem(string displayTitle, PivPinPolicy? pivPinPolicy)
        {
            DisplayTitle = displayTitle;
            PivPinPolicy = pivPinPolicy ?? PivPinPolicy.Default;
        }

        public YubiKeyPINPolicyItem(PivPinPolicy? pivPinPolicy)
        {
            DisplayTitle = (pivPinPolicy ?? PivPinPolicy.Default).ToString();
            PivPinPolicy = pivPinPolicy ?? PivPinPolicy.Default;
        }

        // Override ToString to return the DisplayTitle
        public override string ToString()
        {
            return DisplayTitle;
        }
    }
}
