using Yubico.YubiKey;
using OnboardYK;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OnboardYK
{

    public class YKKeyCollector
    {
        private readonly Form1 _form;

        public YKKeyCollector(Form1 form)
        {
            _form = form ?? throw new ArgumentNullException(nameof(form));
        }

        public bool YKKeyCollectorDelegate(KeyEntryData keyEntryData)
        {
            if (keyEntryData is null)
            {
                return false;
            }

            if (keyEntryData.IsRetry)
            {
                switch (keyEntryData.Request)
                {
                    default:
                        throw new Exception("Unknown request. (Update YKKeyCollector)");

                    case KeyEntryRequest.AuthenticatePivManagementKey:
                        _form.UpdateStatusLabel($"Incorrect PIV Managemwent Key", Color.Red);
                        break;
                    case KeyEntryRequest.VerifyPivPin:
                        if (!(keyEntryData.RetriesRemaining is null))
                        {
                            if (keyEntryData.RetriesRemaining == 0)
                            {
                                _form.UpdateStatusLabel($"Incorrect PIN, {keyEntryData.RetriesRemaining} retires left.");
                            }
                            else
                            {
                                _form.UpdateStatusLabel($"Incorrect PIN, {keyEntryData.RetriesRemaining} retires left.");
                            }
                        }
                        return false;
                }
            }

            byte[] currentValue;
            byte[] newValue; // = null;

            switch (keyEntryData.Request)
            {
                default:
                    return false;

                case KeyEntryRequest.Release:
                    break;

                case KeyEntryRequest.TouchRequest:
                    _form.UpdateStatusLabel("Please touch the YubiKey");
                    return false;

                case KeyEntryRequest.VerifyPivPin:
                    currentValue = System.Text.Encoding.UTF8.GetBytes(Marshal.PtrToStringUni(Marshal.SecureStringToGlobalAllocUnicode(_form._currentPIN!))!);
                    keyEntryData.SubmitValue(currentValue);
                    break;

                case KeyEntryRequest.AuthenticatePivManagementKey:
                    keyEntryData.SubmitValue(_form._pivManagementKey);
                    break;

                case KeyEntryRequest.ChangePivPin:
                    currentValue = System.Text.Encoding.UTF8.GetBytes(Marshal.PtrToStringUni(Marshal.SecureStringToGlobalAllocUnicode(_form._currentPIN!))!);
                    newValue = System.Text.Encoding.UTF8.GetBytes(Marshal.PtrToStringUni(Marshal.SecureStringToGlobalAllocUnicode(_form._newPIN!))!);
                    keyEntryData.SubmitValues(currentValue, newValue);
                    break;
            }

            return true;
        }
    }
}
