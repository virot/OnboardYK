using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace OnboardYK.Support
{
    internal class isPINComplex
    {
        public static bool TestPIN(SecureString? PIN)
        {
            if (PIN is null)
            {
                return false;
            }
            string? PINdecoded = Marshal.PtrToStringUni(Marshal.SecureStringToGlobalAllocUnicode(PIN));
            if (PINdecoded is null)
            { 
                return false;
            }
            // PIN must be atlast 4 different characters
            if (PINdecoded.Where(char.IsLetterOrDigit).Distinct().Count() <= 3)
            {
                return false;
            }
            // Check if PIN is in a sequence from a Qwerty keyboard
            if ("qwertyuiopasdfghjklzxcvbnm".Contains(PINdecoded, StringComparison.InvariantCultureIgnoreCase) ||
                "mnbvcxzlkjhgfdsapoiuytrewq".Contains(PINdecoded, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
            // Check if PIN is in a sequence from a numeric keyboard
            if ("1234567890".Contains(PINdecoded, StringComparison.InvariantCultureIgnoreCase) ||
                "0987654321".Contains(PINdecoded, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }
}
