﻿namespace VLSM_Calc
{
    public class IPAddress
    {
        public byte Byte1 { get; set; }
        public byte Byte2 { get; set; }
        public byte Byte3 { get; set; }
        public byte Byte4 { get; set; }

        /// <summary>
        /// Create ip from 4 seperate bytes
        /// </summary>
        /// <param name="b1">First byte</param>
        /// <param name="b2">Second byte</param>
        /// <param name="b3">Third byte</param>
        /// <param name="b4">Fourth byte</param>
        public IPAddress(byte b1, byte b2, byte b3, byte b4)
        {
            Byte1 = b1;
            Byte2 = b2;
            Byte3 = b3;
            Byte4 = b4;
        }

        /// <summary>
        /// Create IP address from a uint
        /// </summary>
        /// <param name="ip">uint representation of IP address</param>
        public IPAddress(uint ip)
        {
            Byte4 = (byte)(ip & 0b1111_1111);
            ip = ip >> 8;
            Byte3 = (byte)(ip & 0b1111_1111);
            ip = ip >> 8;
            Byte2 = (byte)(ip & 0b1111_1111);
            ip = ip >> 8;
            Byte1 = (byte)(ip & 0b1111_1111);
        }

        /// <summary>
        /// Convert ip address to a 32 bit integer
        /// </summary>
        /// <returns></returns>
        public uint ToUINT32()
        {
            uint output = 0;

            output += Byte1;
            output = output << 8;
            output += Byte2;
            output = output << 8;
            output += Byte3;
            output = output << 8;
            output += Byte4;

            return output;
        }

        /// <summary>
        /// Returns a string representation of this IP address
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Byte1}:{Byte2}:{Byte3}:{Byte4}";
        }
    }
}
