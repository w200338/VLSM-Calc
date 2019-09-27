namespace VLSM_Calc
{
    // TODO: implement IComparable
    public class Subnet
    {
        /// <summary>
        /// Default subnet mask of /24
        /// </summary>
        private static uint DefaultSubnetMask = 0xFF_FF_FF_00;

        /// <summary>
        /// Subnet mask for this subnet
        /// </summary>
        public uint SubnetMask { get; }

        /// <summary>
        /// Subnet mask in CIDR notation (e.g. /24)
        /// </summary>
        public int SubnetMaskCIDR => IPAddress.ToCidr(SubnetMask);

        /// <summary>
        /// Network ID of this subnet
        /// </summary>
        public uint NetworkID { get; }

        /// <summary>
        /// Get last ip in range
        /// </summary>
        public uint BroadcastIP => NetworkID + ~SubnetMask;

        /// <summary>
        /// First available ip for a host
        /// </summary>
        public uint FirstIP => NetworkID + 1;

        /// <summary>
        /// Create a subnet with a network id and default subnet mask
        /// </summary>
        /// <param name="networkID">Network id of subnet</param>
        public Subnet(uint networkID)
        {
            NetworkID = networkID;
            SubnetMask = DefaultSubnetMask;
        }

        /// <summary>
        /// Create subnet with given id and subnet mask
        /// </summary>
        /// <param name="networkID">Network id of subnet</param>
        /// <param name="subnetMask">Subnet mask of subnet</param>
        public Subnet(uint networkID, uint subnetMask)
        {
            NetworkID = networkID;
            SubnetMask = subnetMask;
        }

        /// <summary>
        /// Check if this subnet contains this ip
        /// </summary>
        /// <param name="ip">IP to test</param>
        /// <returns></returns>
        public bool Contains(IPAddress ip)
        {
            uint ipUint = ip.ToUint32();

            return (ipUint >= NetworkID && ipUint <= BroadcastIP);
        }

        /// <summary>
        /// Check if this subnet contains this ip
        /// </summary>
        /// <param name="ip">IP to test</param>
        /// <returns></returns>
        public bool Contains(uint ip)
        {
            return (ip >= NetworkID && ip <= BroadcastIP);
        }

        /// <summary>
        /// String representation of subnet
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"network ID: {new IPAddress(NetworkID)} subnet: {new IPAddress(SubnetMask)} (/{SubnetMaskCIDR})";
        }
    }
}
