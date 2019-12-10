using System;
using System.Windows;

namespace VLSM_Calc
{
    /// <summary>
    /// Interaction logic for UserRequestWindow.xaml
    /// </summary>
    public partial class UserRequestWindow : Window
    {
        private UserRequest request;
        private MainWindow mainWindow;

        public UserRequestWindow(MainWindow mainWindow, UserRequest request)
        {
            InitializeComponent();

            this.request = request;
            this.mainWindow = mainWindow;

            // set datacontext of window
            DataContext = request;

            // only VLSM can have resizeable subnet sizes
            HostInput.IsEnabled = (mainWindow.CalculatorMode == CalculatorMode.VLSM);

            Closed += UserRequestWindow_Closed;
            KeyDown += UserRequestWindow_KeyDown;
        }

        /// <summary>
        /// Close window when enter is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserRequestWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter || e.Key == System.Windows.Input.Key.Return)
            {
                Close();
            }
        }

        /// <summary>
        /// When the window closes update the item collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserRequestWindow_Closed(object sender, EventArgs e)
        {
            mainWindow.hostList.Items.Refresh();
        }
    }
}
