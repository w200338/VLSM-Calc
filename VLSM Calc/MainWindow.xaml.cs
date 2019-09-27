using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VLSM_Calc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
            catch (FormatException exception)
            {
                MessageBox.Show("Ongeldig aantal hosts");
            }
        }

        private void calculateButton_Click(object sender, RoutedEventArgs e)
        {
            // create and try to populate subnet collection
            subnetCollection = new SubnetCollection();

            string[] ipBytes = IPAddressBox.Text.Split('.');
            subnetCollection.NetworkID = new IPAddress(Convert.ToByte(ipBytes[0]), Convert.ToByte(ipBytes[1]), Convert.ToByte(ipBytes[2]), Convert.ToByte(ipBytes[3]));

            ipBytes = subnetMaskBox.Text.Split('.');
            subnetCollection.Subnet = new IPAddress(Convert.ToByte(ipBytes[0]), Convert.ToByte(ipBytes[1]), Convert.ToByte(ipBytes[2]), Convert.ToByte(ipBytes[3]));

            foreach (UserRequest request in requests)
            {
                if (!subnetCollection.AddSubnet(requests.Count))
                {
                    MessageBox.Show("Ongeldige setup");
                }
            }

            resultList.ItemsSource = subnetCollection.Subnets;
            resultList.Items.Refresh();
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            requests.Remove((UserRequest)hostList.SelectedItem);
            hostList.Items.Refresh();
        }
    }
}
