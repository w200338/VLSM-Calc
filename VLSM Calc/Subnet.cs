using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLSM_Calc
{
    // TODO: implement IComparable
    public class Subnet
    {
        /// <summary>
        /// Default subnetmask of /24
        /// </summary>
        private static uint DefaultSubnetMask = 0xFF_FF_FF_00;

        /// <summary>
        /// Subnet mask for this subnet
        /// </summary>
        public uint SubnetMask { get; }

        /// <summary>
        /// Subnetmask in CIDR notation (e.g. /24)
        /// </summary>
        public int SubnetMaskCIDR {
            get
            {
                return IPAddress.ToCIDR(SubnetMask);
            }
        }

        /// <summary>
        /// Network ID of this subnet
        /// </summary>
        public uint NetworkID { get; }

        /// <summary>
        /// Get last ip in range
        /// </summary>
        public uint BroadcastIP
        {
            get
            {
                return NetworkID + ~SubnetMask;
            }
        }

        /// <summary>
        /// First available ip for a host
        /// </summary>
        public uint FirstIP
        {
            get
            {
                return NetworkID + 1;
            }
        }

        /// <summary>
        /// Last available ip for a host
        /// </summary>
        public uint LastIP
        {
            get
            {
                return BroadcastIP - 1;
            }
        }

        /// <summary>
        /// Create a network with 
        /// </summary>
        /// <param name="networkID"></param>
        public Subnet(uint networkID)
        {
            NetworkID = networkID;
            SubnetMask = DefaultSubnetMask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="networkID"></param>
        /// <param name="subnetMask"></param>
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
            uint ipUint = ip.ToUINT32();

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
