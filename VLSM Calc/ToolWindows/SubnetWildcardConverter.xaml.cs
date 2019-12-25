using System;
using System.Windows;

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
            try
            {
                if (InputParser.TryParseSubnetMask(SubnetMaskInput.Text.Trim(), out IPAddress subnetMask, out string errorMessage))
                {
                    // convert to wildcard mask
                    IPAddress wildcardMask = new IPAddress(~subnetMask.ToUint32());
                    WildcardInput.Text = wildcardMask.ToString();
                }
                else
                {
                    throw new FormatException(errorMessage);
                }
            }
            catch (FormatException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void WildcardButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (InputParser.TryParseWildcardMask(WildcardInput.Text.Trim(), out IPAddress wildcardMask, out string errorMessage))
                {
                    // convert to wildcard mask
                    IPAddress subnetMask = new IPAddress(~wildcardMask.ToUint32());
                    SubnetMaskInput.Text = subnetMask.ToString();
                }
                else
                {
                    throw new FormatException(errorMessage);
                }
            }
            catch (FormatException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
