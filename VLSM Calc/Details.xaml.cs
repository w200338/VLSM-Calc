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

        /// <summary>
        /// Currently selected subnet
        /// </summary>
        private Subnet subnet;

        /// <summary>
        /// Index of subnet
        /// </summary>
        private int index = 0;

        /// <summary>
        /// reference to MainWindow
        /// </summary>
        private MainWindow mainWindow;

        public Details(MainWindow mainWindow, Subnet subnet, int subnetIndex)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;
            index = subnetIndex;

            UpdateSubnet(subnet);

            UpdateNextAndPreviousButtons();
        }

        /// <summary>
        /// Put text into textboxes
        /// </summary>
        private void UpdateSubnet(Subnet newSubnet)
        {
            // change subnet
            subnet = newSubnet;

            // populate boxes
            detailsNetworkAddress.Text = new IPAddress(newSubnet.NetworkID).ToString();
            detailsFirstHost.Text = new IPAddress(newSubnet.FirstIP).ToString();
            detailsLastHost.Text = new IPAddress(newSubnet.LastIP).ToString();
            detailsSubnetMask.Text = $"{new IPAddress(newSubnet.SubnetMask)} (/{newSubnet.SubnetMaskCIDR})";
            detailsBroadcast.Text = new IPAddress(newSubnet.BroadcastIP).ToString();
            detailsAvailableHosts.Text = (newSubnet.LastIP - newSubnet.FirstIP + 1).ToString();

            // set title
            Title = newSubnet.ToString();
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

        /// <summary>
        /// Select previous subnet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviousSubnet_Click(object sender, RoutedEventArgs e)
        {
            index--;

            UpdateSubnet((Subnet) mainWindow.resultList.Items[index]);

            UpdateNextAndPreviousButtons();
        }

        /// <summary>
        /// Select next subnet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextSubnet_Click(object sender, RoutedEventArgs e)
        {
            index++;

            UpdateSubnet((Subnet)mainWindow.resultList.Items[index]);

            UpdateNextAndPreviousButtons();
        }

        /// <summary>
        /// Update IsEnabled property of next and previous buttons based on the index of selected subnet
        /// </summary>
        private void UpdateNextAndPreviousButtons()
        {
            PreviousSubnet.IsEnabled = index > 0;
            NextSubnet.IsEnabled = index < mainWindow.resultList.Items.Count - 1;
        }
    }
}
