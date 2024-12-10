namespace OnboardYK.Support
{
    internal class YubiKeyDeviceItem
        {
        public string DisplayTitle { get; set; }
        public int SerialNumber { get; set; }

        public YubiKeyDeviceItem(string displayTitle, int? serialNumber)
        {
            DisplayTitle = displayTitle;
            SerialNumber = serialNumber ?? 0;
        }

        // Override ToString to return the DisplayTitle
        public override string ToString()
        {
            return DisplayTitle;
        }
    }
}
