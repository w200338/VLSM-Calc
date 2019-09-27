using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLSM_Calc
{
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
        public int SubnetMaskCIDR { get; }

        /// <summary>
        /// Network ID of this subnet
        /// </summary>
        public uint NetworkID { get; }

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
    }
}
