﻿using System;

namespace VLSM_Calc
{
    public class IPAddress
    {
        public byte Byte1 { get; set; }
        public byte Byte2 { get; set; }
        public byte Byte3 { get; set; }
        public byte Byte4 { get; set; }

        /// <summary>
        /// Create ip from 4 separate bytes
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
        public uint ToUint32()
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
        /// Convert to CIDR (e.g. 255.255.255.0 -> /24)
        /// </summary>
        /// <returns></returns>
        public int ToCidr()
        {
            return ToCidr(this);
        }

        /// <summary>
        /// Returns a string representation of this IP address
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Byte1}.{Byte2}.{Byte3}.{Byte4}";
        }

        /// <summary>
        /// Ip address which differs number places from given ip address
        /// </summary>
        /// <param name="ip">ip address to change</param>
        /// <param name="number">offset from given ip address</param>
        /// <returns></returns>
        public static IPAddress operator+ (IPAddress ip, uint number) 
        {
            return new IPAddress(ip.ToUint32() + number);
        }

        /// <summary>
        /// Convert to CIDR (e.g. 255.255.255.0 -> /24)
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static int ToCidr(IPAddress ipAddress)
        {
            return ToCidr(ipAddress.ToUint32());
        }

        /// <summary>
        /// Convert to CIDR (e.g. 255.255.255.0 -> /24)
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static int ToCidr(uint ipAddress)
        {
            // check if there's just one continuous line of 1's
            int testLength = 0;
            while (((ipAddress << testLength >> 31) & 0b1) == 1)
            {
                testLength++;
            }

            if (ipAddress << testLength > 0)
            {
                throw new FormatException("Invalid subnet mask");
            }

            int output = 0;

            while (ipAddress > 0)
            {
                ipAddress = ipAddress << 1;
                output++;
            }

            return output;
        }

        /// <summary>
        /// Convert CIDR to a subnet mask
        /// </summary>
        /// <param name="cidr"></param>
        /// <returns></returns>
        public static IPAddress FromCidr(int cidr)
        {
            // check input
            if (cidr < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cidr), "CIDR number can't be negative");
            }

            if (cidr > 32)
            {
                throw new ArgumentOutOfRangeException(nameof(cidr), "CIDR number is too big");
            }

            // prevent overflow error
            if (cidr == 0)
            {
                return new IPAddress(0);
            }

            return new IPAddress(uint.MaxValue << (32 - cidr));
        }
    }
}
