using System;
using System.Windows;

namespace VLSM_Calc.ToolWindows
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
            try
            {
                // from CIDR number to subnet mask
                if (sender.Equals(SubnetCIDRButton))
                {
                    if (InputParser.TryParseCidrNumber(SubnetCIDR.Text.Trim(), out int subnetCidr, out string errorMessage))
                    {
                        IPAddress subnetMask = IPAddress.FromCidr(subnetCidr);
                        SubnetMaskInput.Text = subnetMask.ToString();
                    }
                    else
                    {
                        throw new FormatException(errorMessage);
                    }
                }
                // from subnet mask to CIDR number
                else if (sender.Equals(SubnetMaskButton))
                {
                    if (InputParser.TryParseSubnetMask(SubnetMaskInput.Text.Trim(), out IPAddress subnetMask, out string errorMessage))
                    {
                        SubnetCIDR.Text = $"/{subnetMask.ToCidr()}";
                    }
                    else
                    {
                        throw new FormatException(errorMessage);
                    }
                }
            }
            catch (FormatException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
