using System;
using System.Text.RegularExpressions;
using System.Windows;

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

            // set title
            Title = subnet.ToString();
        }

        private void calculateButton_Click(object sender, RoutedEventArgs e)
        {
            // check inputs with regex
            if (!ipRegex.IsMatch(ipAddressBox.Text))
            {
                hostAddressResult.Text = "Invalid input format";
                return;
            }

            // split input and turn it into an ip address
            string[] ipBytes = ipAddressBox.Text.Split('.');
            IPAddress ip = new IPAddress(Convert.ToByte(ipBytes[0]), Convert.ToByte(ipBytes[1]), Convert.ToByte(ipBytes[2]), Convert.ToByte(ipBytes[3]));
            
            // check if it's a valid host in this subnet
            bool output = subnet.ContainsHost(ip);

            // output depends on what the ip is exactly
            if (output)
            {
                hostAddressResult.Text = "Yes";
            }
            else if (ip.ToUint32() == subnet.NetworkID)
            {
                hostAddressResult.Text = "No, this is the network ID";
            }
            else if (ip.ToUint32() == subnet.NetworkID)
            {
                hostAddressResult.Text = "No, this is the broadcast address";
            }
            else
            {
                hostAddressResult.Text = "No";
            }
        }
    }
}
