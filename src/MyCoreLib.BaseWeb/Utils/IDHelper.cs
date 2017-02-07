
namespace MyCoreLib.BaseWeb.Utils
{
    public static class IDHelper
    {
        private static int[] s_idNumberFactors = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
        private static char[] s_idNumberChecksumChars = new char[] { '1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2' };

        /// <summary>
        /// Validates whether this is a valid id number.
        /// </summary>
        public static bool ValidateIDNumber(string idNumber)
        {
            if (string.IsNullOrWhiteSpace(idNumber))
                return false;

            idNumber = idNumber.Trim();
            if (idNumber.Length == 18)
            {
                int sum = 0;
                for (int i = 0; i < 17; i++)
                {
                    sum += (idNumber[i] - '0') * s_idNumberFactors[i];
                }

                if (sum > 0)
                {
                    char expectedChecksumChar = s_idNumberChecksumChars[sum % 11];
                    char actual = idNumber[17];
                    if (expectedChecksumChar == actual || (expectedChecksumChar == 'X' && actual == 'x'))
                        return true;
                }
            }

            return false;
        }
    }
}
