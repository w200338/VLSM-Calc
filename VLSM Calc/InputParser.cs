using System.Text.RegularExpressions;

namespace VLSM_Calc
{
    public static class InputParser
    {
        /// <summary>
        /// Regex to check if an ip is valid (x.x.x.x)
        /// </summary>
        private static Regex ipRegex = new Regex(@"^(\d{1,3}\.){3}\d{1,3}$");

        /// <summary>
        /// Regex to check if a subnet mask is valid (/xx or x.x.x.x)
        /// </summary>
        private static Regex subnetMaskRegex = new Regex(@"^(((\d{1,3}\.){3}\d{1,3})|(\/\d{1,2}))$");

        /// <summary>
        /// Regex to check if a cidr number is valid (/xx)
        /// </summary>
        private static Regex cidrRegex = new Regex(@"^\/\d{1,2}$");

        /// <summary>
        /// Try to parse a possible IP Address
        /// </summary>
        /// <param name="userInput">IP address string which user put into box</param>
        /// <param name="result">Result of parsing if it was successful</param>
        /// <param name="errorMessage">Error message if it failed</param>
        /// <returns></returns>
        public static bool TryParseIPAddress(string userInput, out IPAddress result, out string errorMessage)
        {
            errorMessage = null;
            result = null;

            // check if it passes a regex check
            if (!ipRegex.IsMatch(userInput))
            {
                errorMessage = "Invalid IP address format (should be x.x.x.x)";
                return false;
            }

            // get 4 bytes out of string
            string[] stringBytes = userInput.Split('.');
            byte[] bytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                if (byte.TryParse(stringBytes[i], out byte b))
                {
                    bytes[i] = b;
                }
                else
                {
                    errorMessage = "IP address contains a value higher than 255";
                    return false;
                }
            }

            result = new IPAddress(bytes[0], bytes[1], bytes[2], bytes[3]);
            return true;
        }

        /// <summary>
        /// Try to parse a possible CIDR number
        /// </summary>
        /// <param name="userInput">CIDR number string which user put into box</param>
        /// <param name="result">Result of parsing if it was successful</param>
        /// <param name="errorMessage">Error message if it failed</param>
        /// <returns></returns>
        public static bool TryParseCidrNumber(string userInput, out int result, out string errorMessage)
        {
            errorMessage = null;
            result = -1;

            // check if it passes a regex check
            if (!cidrRegex.IsMatch(userInput))
            {
                errorMessage = "Invalid CIDR number format (should be /xx where xx is a number between 0 and 31)";
                return false;
            }

            // check if number is within bounds and put it into the out parameter
            if (int.TryParse(userInput.Remove(0, 1), out result))
            {
                if (result > 31)
                {
                    errorMessage = "Invalid CIDR number, should be between 0 and 31";
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Try to parse a possible subnet mask
        /// </summary>
        /// <param name="userInput">subnet mask string which user put into box</param>
        /// <param name="result">Result of parsing if it was successful</param>
        /// <param name="errorMessage">Error message if it failed</param>
        /// <returns></returns>
        public static bool TryParseSubnetMask(string userInput, out IPAddress result, out string errorMessage)
        {
            errorMessage = null;
            result = null;

            // check if it passes a regex check
            if (!subnetMaskRegex.IsMatch(userInput))
            {
                errorMessage = "Invalid subnet mask format (should be x.x.x.x or /xx)";
                return false;
            }

            // is it CIDR or IP address notation?
            if (userInput[0] == '/')
            {
                // try to get a valid CIDRNumber from input
                if (TryParseCidrNumber(userInput, out int cidrNumber, out errorMessage))
                {
                    result = IPAddress.FromCidr(cidrNumber);
                    return true;
                }

                return false;
            }

            // parse like an ip address
            if (TryParseIPAddress(userInput, out IPAddress possibleResult, out _))
            {
                // find first 0 and check for 1's following it
                uint subnetBits = possibleResult.ToUint32();
                int testLength = 0;
                while (((subnetBits << testLength >> 31) & 0b1) == 1)
                {
                    testLength++;
                }

                if (subnetBits << testLength > 0)
                {
                    errorMessage = "Invalid subnet mask";
                    return false;
                }

                result = possibleResult;
                return true;
            }

            // only reason this one can fail
            errorMessage = "Subnet mask contains a value higher than 255";
            return false;
        }

        /// <summary>
        /// Try to parse a possible wildcard mask
        /// </summary>
        /// <param name="userInput">wildcard mask string which user put into box</param>
        /// <param name="result">Result of parsing if it was successful</param>
        /// <param name="errorMessage">Error message if it failed</param>
        /// <returns></returns>
        public static bool TryParseWildcardMask(string userInput, out IPAddress result, out string errorMessage)
        {
            errorMessage = null;
            result = null;

            // check if it passes a regex check
            if (!ipRegex.IsMatch(userInput))
            {
                errorMessage = "Invalid wildcard mask format (should be x.x.x.x)";
                return false;
            }

            // parse like an ip address
            if (TryParseIPAddress(userInput, out IPAddress possibleResult, out _))
            {
                // invert UInt32 representation of input and then do the same check as the subnet mask
                // find first 0 and check for 1's following it
                uint subnetBits = ~possibleResult.ToUint32();

                // edge case which causes an infinite loop
                if (subnetBits == uint.MaxValue)
                {
                    result = possibleResult;
                    return true;
                }

                int testLength = 0;
                while (((subnetBits << testLength >> 31) & 0b1) == 1)
                {
                    testLength++;
                }

                if (subnetBits << testLength > 0)
                {
                    errorMessage = "Invalid wildcard mask";
                    return false;
                }

                result = possibleResult;
                return true;
            }

            // only reason this one can fail
            errorMessage = "Wildcard mask contains a value higher than 255";
            return false;
        }
    }
}
