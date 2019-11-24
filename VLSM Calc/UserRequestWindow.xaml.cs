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

            Closed += UserRequestWindow_Closed;
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
