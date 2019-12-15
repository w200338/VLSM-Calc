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

namespace VLSM_Calc
{
    /// <summary>
    /// Interaction logic for SubnetConverter.xaml
    /// </summary>
    public partial class SubnetConverter : Window
    {
        public SubnetConverter()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Convert CIDR number to subnet mask and vice versa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubnetConvert_Click(object sender, RoutedEventArgs e)
        {
            // from CIDR number to subnet mask
            if (sender.Equals(SubnetCIDRButton))
            {
                if (MainWindow.numberRegex.IsMatch(SubnetCIDR.Text))
                {
                    // get number from input
                    int subnetCidr = Convert.ToInt32(SubnetCIDR.Text);
                    if (subnetCidr >= 0 && subnetCidr <= 32)
                    {
                        try
                        {
                            IPAddress subnetMask = IPAddress.FromCidr(subnetCidr);
                            SubnetMaskInput.Text = subnetMask.ToString();
                        }
                        catch (FormatException exception)
                        {
                            MessageBox.Show(exception.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Number needs to be between 0 and 32");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid number");
                }
            }
            // from subnet mask to CIDR number
            else if (sender.Equals(SubnetMaskButton))
            {
                if (MainWindow.ipRegex.IsMatch(SubnetMaskInput.Text))
                {
                    // split subnet mask, put it into an IPAddress object and try to get CIDR number from it
                    try
                    {
                        string[] subnetMaskBytes = SubnetMaskInput.Text.Split('.');
                        SubnetCIDR.Text = (new IPAddress(Convert.ToByte(subnetMaskBytes[0]), Convert.ToByte(subnetMaskBytes[1]), Convert.ToByte(subnetMaskBytes[2]), Convert.ToByte(subnetMaskBytes[3])).ToCidr()).ToString();
                    }
                    catch (FormatException exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                    catch (OverflowException)
                    {
                        MessageBox.Show("Subnet mask contains a value which is too large");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid subnet mask format");
                }
            }
        }
    }
}
