using System.Collections.Generic;

namespace VLSM_Calc
{
    public class SubnetCollection
    {
        public IPAddress NetworkID { get; set; }

        public IPAddress BroadcastIP
        {
            get
            {
                return NetworkID + ~SubnetMask.ToUINT32();
            }
        }

        /// <summary>
        /// Subnetmask for own subnet
        /// </summary>
        public IPAddress SubnetMask { get; set; }

        /// <summary>
        /// All subnets in this collection
        /// </summary>
        public List<Subnet> Subnets { get; } = new List<Subnet>();
        
        /// <summary>
        /// Try to add a subnet with at least this many hosts
        /// </summary>
        /// <param name="hostAmount"></param>
        /// <returns></returns>
        public bool AddSubnet(int hostAmount)
        {
            Subnet subnet = TryCreateSubnet(hostAmount);

            if (subnet == null)
            {
                return false;
            }
            else
            {
                Subnets.Add(subnet);
                return true;
            }
        }

        /// <summary>
        /// try to create a subnet
        /// </summary>
        /// <param name="requestedHosts"></param>
        /// <returns></returns>
        private Subnet TryCreateSubnet(int requestedHosts)
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
            uint subnetMask = ~actualSize + 1;

            // find required space in subnetcollection and get its network id
            IPAddress firstIp = FirstAvailableIP(actualSize);

            if (firstIp == null)
            {
                return null;
            }

            return new Subnet(firstIp.ToUINT32(), subnetMask);
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

            for (int i = 1; i < b; i++)
            {
                output *= a;
            }

            return output;
        }

        /// <summary>
        /// First available ip
        /// </summary>
        /// <returns></returns>
        public IPAddress FirstAvailableIP()
        {
            IPAddress output = NetworkID;

            foreach (Subnet subnet in Subnets)
            {
                if (subnet.Contains(output))
                {
                    output = new IPAddress(subnet.BroadcastIP + 1);
                }
                else 
                {
                    return output;
                }
            }

            return null;
        }

        /// <summary>
        /// First available ip
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public IPAddress FirstAvailableIP(uint size)
        {
            IPAddress output = NetworkID;

            // return if there aren't any other subnets yet
            if (Subnets.Count == 0)
            {
                return output;
            }

            foreach (Subnet subnet in Subnets)
            {
                if (subnet.Contains(output))
                {
                    output = new IPAddress(subnet.BroadcastIP + 1);
                }
                else
                {
                    IPAddress last = new IPAddress(output.ToUINT32() + size);
                    bool conflict = false;

                    foreach (Subnet otherSubnet in Subnets)
                    {
                        if (subnet == otherSubnet)
                        {
                            continue;
                        }

                        if (otherSubnet.Contains(last))
                        {
                            conflict = true;
                            break;
                        }
                    }

                    if (!conflict)
                    {
                        return output;
                    }
                }
            }

            // check if ip is still within bounds
            if (Contains(output))
            {
                return output;
            }

            return null;
        }

        public bool Contains(IPAddress ip)
        {
            uint ipUint = ip.ToUINT32();

            return (ipUint >= NetworkID.ToUINT32() && ipUint <= BroadcastIP.ToUINT32());
        }
    }
}
