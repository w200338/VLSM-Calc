using System;
using System.Windows;

namespace VLSM_Calc.ToolWindows
{
    /// <summary>
    /// Interaction logic for SubnetSizeCalculator.xaml
    /// </summary>
    public partial class SubnetSizeCalculator : Window
    {
        public SubnetSizeCalculator()
        {
            InitializeComponent();
        }

        private void SubnetButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.subnetMaskRegex.IsMatch(SubnetInput.Text))
            {
                // get subnet mask
                IPAddress subnetMask;
                if (SubnetInput.Text.Contains("/"))
                {
                    // get value behind / and use bitwise NOT to create subnet mask
                    int subnetMaskCidr = Convert.ToInt32(SubnetInput.Text.Replace("/", ""));

                    // check if number is valid
                    if (subnetMaskCidr > 31 || subnetMaskCidr < 0)
                    {
                        MessageBox.Show("Subnet cidr number must be between 0 and 31");
                        return;
                    }

                    subnetMask = IPAddress.FromCidr(subnetMaskCidr);
                }
                else
                {
                    try
                    {
                        // split into 4 parts and make into an ip address
                        string[] ipBytes = SubnetInput.Text.Split('.');
                        subnetMask = new IPAddress(Convert.ToByte(ipBytes[0]), Convert.ToByte(ipBytes[1]), Convert.ToByte(ipBytes[2]), Convert.ToByte(ipBytes[3]));

                        // check if valid subnet mask
                        subnetMask.ToCidr();
                    }
                    catch (FormatException exception)
                    {
                        MessageBox.Show(exception.Message);
                        return;
                    }
                    catch (OverflowException)
                    {
                        MessageBox.Show("Subnet mask contains a value which is too large");
                        return;
                    }
                }

                // get size
                Subnet subnet = new Subnet(0, subnetMask.ToUint32());
                long size = subnet.BroadcastIP - subnet.NetworkID + 1L;

                // put into output boxes
                SizeOutput.Text = size.ToString();
                HostSizeOutput.Text = (size - 2).ToString();
            }
            else
            {
                MessageBox.Show("Invalid subnet format");
            }
        }
    }
}
