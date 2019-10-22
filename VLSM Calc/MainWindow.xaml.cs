using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;

namespace VLSM_Calc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Regex to check if an ip is valid
        /// </summary>
        private static Regex ipRegex = new Regex(@"^(\d{1,3}\.){3}\d{1,3}$");

        /// <summary>
        /// Regex to check if a subnet mask is valid (/xx or x.x.x.x)
        /// </summary>
        private static Regex subnetMaskRegex = new Regex(@"^(((\d{1,3}\.){3}\d{1,3})|(\/?\d{1,2}))$");

        /// <summary>
        /// Calculated collection
        /// </summary>
        private SubnetCollection subnetCollection;

        /// <summary>
        /// User requests
        /// </summary>
        private List<UserRequest> requests = new List<UserRequest>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // parse requested number of hosts, make sure it isn't negative
                int requestedNumber = Convert.ToInt32(hostBox.Text.Trim());
                if (requestedNumber <= 0)
                {
                    throw new FormatException();
                }

                UserRequest request = new UserRequest(requestedNumber);
                requests.Add(request);

                hostList.ItemsSource = requests;
                hostList.Items.Refresh();
            }
            catch (FormatException)
            {
                MessageBox.Show("Ongeldig aantal hosts");
            }
        }

        private void calculateButton_Click(object sender, RoutedEventArgs e)
        {
            // don't do a thing if there's no requests
            if (requests.Count == 0)
            {
                return;
            }

            // create and try to populate subnet collection
            subnetCollection = new SubnetCollection();
            
            try
            {
                // check inputs with regex
                if (IPAddressBox.Text.Trim().Length == 0)
                {
                    IPAddressBox.Text = "0.0.0.0";
                }
                else if (!ipRegex.IsMatch(IPAddressBox.Text))
                {
                    throw new FormatException("Invalid IP address");
                }

                if (!subnetMaskRegex.IsMatch(subnetMaskBox.Text))
                {
                    throw new FormatException("Invalid subnet mask");
                }

                // get ip from input
                string[] ipBytes = IPAddressBox.Text.Split('.');
                subnetCollection.NetworkID = new IPAddress(Convert.ToByte(ipBytes[0]), Convert.ToByte(ipBytes[1]), Convert.ToByte(ipBytes[2]), Convert.ToByte(ipBytes[3]));

                // get subnet mask
                if (subnetMaskBox.Text.Contains("/"))
                {
                    // get value behind / and use bitwise NOT to create subnet mask
                    int subnetMask = Convert.ToInt32(subnetMaskBox.Text.Replace("/", ""));
                    subnetCollection.SubnetMask = IPAddress.FromCidr(subnetMask);
                }
                else
                {
                    // split into 4 parts
                    ipBytes = subnetMaskBox.Text.Split('.');
                    subnetCollection.SubnetMask = new IPAddress(Convert.ToByte(ipBytes[0]), Convert.ToByte(ipBytes[1]), Convert.ToByte(ipBytes[2]), Convert.ToByte(ipBytes[3]));
                }
                
                // check if network ip and subnet combo are valid
                if ((subnetCollection.NetworkID.ToUint32() & subnetCollection.SubnetMask.ToUint32()) != subnetCollection.NetworkID.ToUint32())
                {
                    throw new FormatException("Invalid IP address and subnet mask combination");
                }

                // sort requests based on the amount of hosts, highest first and refresh the input
                requests.Sort();
                hostList.Items.Refresh();

                // check if their total is already over the amount of hosts this setup can support
                uint totalHosts = 0;
                foreach (UserRequest request in requests)
                {
                    totalHosts += (uint)request.RequestedHosts;
                }

                if (totalHosts > ~subnetCollection.SubnetMask.ToUint32())
                {
                    MessageBox.Show("Too many hosts requested");
                    return;
                }

                // try to fulfill all requests
                foreach (UserRequest request in requests)
                {
                    // if a subnet can't be added
                    if (!subnetCollection.AddSubnet(request.RequestedHosts))
                    {
                        MessageBox.Show("Invalid combination of hosts");
                        return;
                    }
                }
            }
            catch (FormatException exception)
            {
                MessageBox.Show(exception.Message);
            }

            resultList.ItemsSource = subnetCollection.Subnets;
            resultList.Items.Refresh();
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            requests.Remove((UserRequest)hostList.SelectedItem);
            hostList.Items.Refresh();
        }

        private void detailButton_Click(object sender, RoutedEventArgs e)
        {
            // don't show details if nothing is selected
            if (resultList.SelectedItem == null)
            {
                return;
            }

            Details details = new Details(resultList.SelectedItem as Subnet);
            details.Show();
        }

        private void resultList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Details details = new Details(resultList.SelectedItem as Subnet);
            details.Show();
        }
    }
}
