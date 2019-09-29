﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VLSM_Calc
{
    /// <summary>
    /// Interaction logic for Details.xaml
    /// </summary>
    public partial class Details : Window
    {
        private static Regex ipRegex = new Regex(@"^(\d{1,3}\.){3}\d{1,3}$");

        private Subnet subnet;

        public Details(Subnet subnet)
        {
            InitializeComponent();

            this.subnet = subnet;

            // populate boxes
            detailsNetworkAddress.Text = new IPAddress(subnet.NetworkID).ToString();
            detailsFirstHost.Text = new IPAddress(subnet.FirstIP).ToString();
            detailsLastHost.Text = new IPAddress(subnet.LastIP).ToString();
            detailsSubnetMask.Text = $"{new IPAddress(subnet.SubnetMask)} (/{subnet.SubnetMaskCIDR})";
            detailsBroadcast.Text = new IPAddress(subnet.BroadcastIP).ToString();
            detailsAvailableHosts.Text = (subnet.LastIP - subnet.FirstIP + 1).ToString();
        }

        private void calculateButton_Click(object sender, RoutedEventArgs e)
        {
            // check inputs with regex
            if (!ipRegex.IsMatch(ipAddressBox.Text))
            {
                hostAddressResult.Text = "Invalid input format";
                return;
            }

            string[] ipBytes = ipAddressBox.Text.Split('.');
            IPAddress ip = new IPAddress(Convert.ToByte(ipBytes[0]), Convert.ToByte(ipBytes[1]), Convert.ToByte(ipBytes[2]), Convert.ToByte(ipBytes[3]));
            
            bool output = subnet.Contains(ip);

            hostAddressResult.Text = output ? "Yes" : "No";
        }
    }
}