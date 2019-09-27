namespace VLSM_Calc
{
    public static class SubnetCreator
    {
        /// <summary>
        /// try to create a subnet
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="requestedHosts"></param>
        /// <returns></returns>
        public static Subnet TryCreateSubnet(SubnetCollection parent, int requestedHosts)
        {
            // convert requested hosts to nearest 2^n - 2 and get n
            bool found = false;
            int requiredBits = 0;
            while (!found)
            {
                requiredBits++;
                int newHosts = (IntPow(2, requiredBits) - 2);
                found = (newHosts >= requestedHosts);
            }

            uint actualSize = (uint)IntPow(2, requiredBits);
            uint subnetMask = ~actualSize;

            // find required space in subnetcollection and get its network id
            return new Subnet(parent.FirstAvailableIP(actualSize).ToUINT32(), subnetMask);
        }

        /// <summary>
        /// Integer only version of a^b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b">Exponent</param>
        /// <returns></returns>
        private static int IntPow(int a, int b)
        {
            if (b == 0) return 1;
            if (b == 1) return a;

            int output = a;

            for (int i = 1; i <= b; i++)
            {
                a *= a;
            }

            return output;
        }
    }
}
