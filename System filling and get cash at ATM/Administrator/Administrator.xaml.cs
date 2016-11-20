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
using System.Data;
using System.Data.SqlClient;

namespace System_filling_and_get_cash_at_ATM.Administrator
{
    /// <summary>
    /// Interaction logic for Administrator.xaml
    /// </summary>
    public partial class Administrator : Window
    {
        CRUD crud = new CRUD();
        public Administrator()
        {
            InitializeComponent();
            crud.DataGridRefresh(ref dataGrid);
        }

        private void buttonRefresh_Click(object sender, RoutedEventArgs e)
        {
            crud.DataGridRefresh(ref dataGrid);
        }

        private void buttonBlockUnblockUser_Click(object sender, RoutedEventArgs e)
        {
            crud.BlockUnblockUser(textBoxBlockUnblockUser.Text);
            crud.DataGridRefresh(ref dataGrid);
        }
        private void buttonSetALimitCalculations_Click(object sender, RoutedEventArgs e)
        {
            crud.SetALimitCalculations(textBoxIdSetALimitCalculations.Text, textBoxLimitSetALimitCalculations.Text);
            crud.DataGridRefresh(ref dataGrid);
        }

        private void buttonProvideBilling_Click(object sender, RoutedEventArgs e)
        {
            crud.richTextBoxProvideBillingRefresh(ref richTextBoxProvideBilling, textBoxProvideBilling.Text);
        }
    }
}
