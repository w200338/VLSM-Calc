using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using VLSM_Calc.ToolWindows;

namespace VLSM_Calc.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Regex to check if an ip is valid
        /// </summary>
        public static Regex ipRegex = new Regex(@"^(\d{1,3}\.){3}\d{1,3}$");

        /// <summary>
        /// Regex to check if a subnet mask is valid (/xx or x.x.x.x)
        /// </summary>
        public static Regex subnetMaskRegex = new Regex(@"^(((\d{1,3}\.){3}\d{1,3})|(\/\d{1,2}))$");

        /// <summary>
        /// Regex to check if string is just a number
        /// </summary>
        public static Regex numberRegex = new Regex("^\\d+$");

        /// <summary>
        /// Calculated collection
        /// </summary>
        private SubnetCollection subnetCollection;

        /// <summary>
        /// User requests
        /// </summary>
        private ObservableCollection<UserRequest> requests = new ObservableCollection<UserRequest>();

        /// <summary>
        /// Current calculator mode
        /// </summary>
        public CalculatorMode CalculatorMode { get; private set; } = CalculatorMode.VLSM;

        public MainWindow()
        {
            InitializeComponent();

            hostList.ItemsSource = requests;
        }

        /// <summary>
        /// Add a new user request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid amount of hosts");
            }
        }

        /// <summary>
        /// Remove selected request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (hostList.SelectedIndex >= 0 && hostList.SelectedIndex < requests.Count)
            {
                requests.RemoveAt(hostList.SelectedIndex);
            }
        }

        /// <summary>
        /// Open UserRequestWindow to change this request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hostList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            UserRequestWindow window = new UserRequestWindow(this, (UserRequest)hostList.SelectedItem);
            window.Show();
        }

        /// <summary>
        /// Calculate subnets
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                // ip address
                if (InputParser.TryParseIPAddress(IPAddressBox.Text.Trim(), out IPAddress iPAddress, out string errorMessage))
                {
                    subnetCollection.NetworkID = iPAddress;
                }
                else
                {
                    throw new Exception(errorMessage);
                }

                // subnet mask
                if (InputParser.TryParseSubnetMask(subnetMaskBox.Text, out IPAddress subnetMask, out errorMessage))
                {
                    subnetCollection.SubnetMask = subnetMask;
                }
                else
                {
                    throw new Exception(errorMessage);
                }

                // check if network ip and subnet combo are valid
                if ((subnetCollection.NetworkID.ToUint32() & subnetCollection.SubnetMask.ToUint32()) != subnetCollection.NetworkID.ToUint32())
                {
                    throw new FormatException("Invalid IP address and subnet mask combination");
                }

                // sort requests based on the amount of hosts, highest first and refresh the input
                List<UserRequest> tempList = new List<UserRequest>(requests);
                tempList.Sort();
                requests = new ObservableCollection<UserRequest>(tempList);
                hostList.DataContext = requests;
                hostList.ItemsSource = requests;

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
                    if (!subnetCollection.AddSubnet(request.RequestedHosts, request.Name))
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
        
        /// <summary>
        /// Shows detail window of selected subnet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void detailButton_Click(object sender, RoutedEventArgs e)
        {
            // don't show details if nothing is selected
            if (resultList.SelectedItem == null)
            {
                return;
            }

            Details details = new Details(this, resultList.SelectedItem as Subnet, resultList.SelectedIndex);
            details.Show();
        }

        /// <summary>
        /// Open Details Window of selected subnet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resultList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Details details = new Details(this, resultList.SelectedItem as Subnet, resultList.SelectedIndex);
            details.Show();
        }

        /// <summary>
        /// Switch on VLSM elements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VLSMSwitch_Click(object sender, RoutedEventArgs e)
        {
            removeButton.IsEnabled = true;
            addButton.IsEnabled = true;
            HostLabel.Text = "Hosts";
            
            DivideButton.IsEnabled = false;
            DivideButton.Visibility = Visibility.Hidden;

            VLSMSwitch.IsChecked = true;
            SubnetSwitch.IsChecked = false;
            Title = "VLSM Calculator - VLSM mode";

            CalculatorMode = CalculatorMode.VLSM;
        }

        /// <summary>
        /// Switch on Subnet elements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubnetSwitch_Click(object sender, RoutedEventArgs e)
        {
            removeButton.IsEnabled = false;
            addButton.IsEnabled = false;
            HostLabel.Text = "Subnets";

            DivideButton.IsEnabled = true;
            DivideButton.Visibility = Visibility.Visible;

            VLSMSwitch.IsChecked = false;
            SubnetSwitch.IsChecked = true;
            Title = "VLSM Calculator - Subnet mode";

            CalculatorMode = CalculatorMode.Subnet;
        }

        /// <summary>
        /// Divide into equal subnets
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DivideButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // get subnet mask
                int totalSubnetMask;
                if (InputParser.TryParseSubnetMask(subnetMaskBox.Text.Trim(), out IPAddress subnetMask, out string errorMessage))
                {
                    totalSubnetMask = subnetMask.ToCidr();
                }
                else
                {
                    throw new FormatException(errorMessage);
                }

                // get number from input
                int subnetAmount = Convert.ToInt32(hostBox.Text.Trim());

                // check if power of 2 and which power
                int power = 0;
                while (((subnetAmount >> power) & 1) == 0)
                {
                    power++;
                }

                if (subnetAmount >> (power + 1) > 0)
                {
                    throw new FormatException("Invalid number of subnets, must be a power of 2");
                }

                // empty list of requests
                requests.Clear();

                // create requests
                int amountOfHosts = (int) Math.Pow(2, 32 - totalSubnetMask - power) - 2;

                for (int i = 0; i < subnetAmount; i++)
                {
                    requests.Add(new UserRequest(amountOfHosts));
                }
            }
            catch (FormatException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        /// Open subnet size calculator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubnetSizeCalculatorButton_Click(object sender, RoutedEventArgs e)
        {
            SubnetSizeCalculator subnetSize = new SubnetSizeCalculator();
            subnetSize.Show();
        }

        /// <summary>
        /// Open subnet size converter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubnetSizeConverterButton_Click(object sender, RoutedEventArgs e)
        {
            SubnetConverter subnetConverter = new SubnetConverter();
            subnetConverter.Show();
        }

        /// <summary>
        /// Open subnet to wildcard converter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubnetWildcardCalculatorButton_Click(object sender, RoutedEventArgs e)
        {
            SubnetWildCardConverter subnetWildCardConverter = new SubnetWildCardConverter();
            subnetWildCardConverter.Show();
        }
    }

    /// <summary>
    /// Modes which the calculator can use
    /// </summary>
    public enum CalculatorMode
    {
        Subnet,
        VLSM
    }
}
