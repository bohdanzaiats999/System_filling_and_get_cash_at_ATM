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

namespace System_filling_and_get_cash_at_ATM.User
{
    /// <summary>
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class User : Window
    {
        CRUD crud = new CRUD();
        public User()
        {
            InitializeComponent();
            textBlocBalance.Text = crud.CheckTheBalance().ToString();
        }

        private void buttonAddFunds_Click(object sender, RoutedEventArgs e)
        {
            crud.AddFunds(textBoxAddFunds.Text);
            textBlocBalance.Text = crud.CheckTheBalance().ToString();
        }
        private void buttonWithdrawCash_Click(object sender, RoutedEventArgs e)
        {
            crud.WithdrawCash(textBoxWithdrawCash.Text);
            textBlocBalance.Text = crud.CheckTheBalance().ToString();

        }

        private void buttonTtransferFunds_Click(object sender, RoutedEventArgs e)
        {
            crud.TtransferFunds(textBoxQuantityTtransferFunds.Text, textBoxLoginTtransferFunds.Text);
            textBlocBalance.Text = crud.CheckTheBalance().ToString();

        }
    }
}
