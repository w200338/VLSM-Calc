using System.Collections.Generic;

namespace VLSM_Calc
{
    public class SubnetCollection
    {
        /// <summary>
        /// subnet which is available
        /// </summary>
        public IPAddress Subnet { get; }

        /// <summary>
        /// Subnetmask for own subnet
        /// </summary>
        public IPAddress SubnetMask { get; }

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
            Subnet subnet = SubnetCreator.TryCreateSubnet(this, hostAmount);

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
        /// First available ip
        /// </summary>
        /// <returns></returns>
        public IPAddress FirstAvailableIP()
        {
            IPAddress output = Subnet;

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
            IPAddress output = Subnet;

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

            return null;
        }
    }
}
