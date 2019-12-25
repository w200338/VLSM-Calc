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
            try
            {
                if (InputParser.TryParseSubnetMask(SubnetInput.Text.Trim(), out IPAddress subnetMask, out string errorMessage))
                {
                    // get size
                    Subnet subnet = new Subnet(0, subnetMask.ToUint32());
                    long size = 1L + subnet.BroadcastIP - subnet.NetworkID;

                    // put into output boxes
                    SizeOutput.Text = size.ToString();
                    HostSizeOutput.Text = (size - 2).ToString();
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
