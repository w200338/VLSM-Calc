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
using System.Windows.Shapes;

namespace VLSM_Calc.ToolWindows
{
    /// <summary>
    /// Interaction logic for SubnetWildCardConverter.xaml
    /// </summary>
    public partial class SubnetWildCardConverter : Window
    {
        public SubnetWildCardConverter()
        {
            InitializeComponent();
        }

        private void SubnetButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.subnetMaskRegex.IsMatch(SubnetMaskInput.Text))
            {
                // get subnet mask
                IPAddress subnetMask;
                if (SubnetMaskInput.Text.Contains("/"))
                {
                    // get value behind / and use bitwise NOT to create subnet mask
                    int subnetMaskCidr = Convert.ToInt32(SubnetMaskInput.Text.Replace("/", ""));

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
                        string[] ipBytes = SubnetMaskInput.Text.Split('.');
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

                // convert to wildcard mask
                IPAddress wildcardMask = new IPAddress(~subnetMask.ToUint32());
                //IPAddress wildcardMask = new IPAddress(uint.MaxValue >> subnetMask.ToCidr());
                WildcardInput.Text = wildcardMask.ToString();
            }
            else
            {
                MessageBox.Show("Invalid subnet format");
            }
        }

        private void WildcardButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.subnetMaskRegex.IsMatch(WildcardInput.Text))
            {
                // get subnet mask
                IPAddress wildcardMask;
                if (WildcardInput.Text.Contains("/"))
                {
                    // get value behind / and use bitwise NOT to create subnet mask
                    int subnetMaskCidr = Convert.ToInt32(WildcardInput.Text.Replace("/", ""));

                    // check if number is valid
                    if (subnetMaskCidr > 31 || subnetMaskCidr < 0)
                    {
                        MessageBox.Show("Subnet cidr number must be between 0 and 31");
                        return;
                    }

                    wildcardMask = IPAddress.FromCidr(subnetMaskCidr);
                }
                else
                {
                    try
                    {
                        // split into 4 parts and make into an ip address
                        string[] ipBytes = WildcardInput.Text.Split('.');
                        wildcardMask = new IPAddress(Convert.ToByte(ipBytes[0]), Convert.ToByte(ipBytes[1]), Convert.ToByte(ipBytes[2]), Convert.ToByte(ipBytes[3]));

                        // check if valid wildcard mask by inverting it and then checking if it would be a valid subnet mask
                        new IPAddress(~wildcardMask.ToUint32()).ToCidr();
                    }
                    catch (FormatException exception)
                    {
                        MessageBox.Show(exception.Message);
                        return;
                    }
                    catch (OverflowException)
                    {
                        MessageBox.Show("wildcard mask contains a value which is too large");
                        return;
                    }
                }

                // convert to wildcard mask
                IPAddress subnetMask = new IPAddress(uint.MaxValue >> wildcardMask.ToCidr());
                WildcardInput.Text = subnetMask.ToString();
            }
            else
            {
                MessageBox.Show("Invalid wildcard format");
            }
        }
    }
}
