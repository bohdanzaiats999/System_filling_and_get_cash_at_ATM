using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace System_filling_and_get_cash_at_ATM
{
    class CRUD
    {
        int balance = 0;
        // Реєстрація
        public bool Registration(string login, string password)
        {
            if (login == "" | password == "")
            {
                MessageBox.Show("Введіть логін і пароль");
                return false;
            }
            else
            {
                SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnectionString);
                SqlDataReader sqlDataReader;

                SqlCommand cmd = new SqlCommand("SELECT Login FROM Users WHERE Login = @Login", sqlConnection);
                cmd.Parameters.Add(new SqlParameter("Login", login));

                sqlConnection.Open();
                sqlDataReader = cmd.ExecuteReader();
                sqlDataReader.Read();

                if (!sqlDataReader.HasRows)
                {
                    // Реєстрація
                    sqlConnection.Close();
                    sqlDataReader.Close();

                    cmd.CommandText = "INSERT INTO Users VALUES (@Login,@Password,@Money,@BlockStatus,@AdminStatus,@Limit)";

                    cmd.Parameters.Add(new SqlParameter("Password", password.GetHashCode()));
                    cmd.Parameters.Add(new SqlParameter("Money", 100));
                    cmd.Parameters.Add(new SqlParameter("BlockStatus", false)); // false - заборонені дії в системі , true - дозволені дії в системі
                    cmd.Parameters.Add(new SqlParameter("AdminStatus", false)); // false - простий користувач, true - адміністратор
                    cmd.Parameters.Add(new SqlParameter("Limit", 1000));

                    sqlConnection.Open();
                    sqlDataReader = cmd.ExecuteReader();
                    sqlConnection.Close();
                    sqlDataReader.Close();

                    cmd.CommandText = "SELECT Id FROM Users WHERE Login = @Login";
                    sqlConnection.Open();
                    sqlDataReader = cmd.ExecuteReader();
                    sqlDataReader.Read();
                    Properties.Settings.Default.IdUser = (int)sqlDataReader[0];
                    Properties.Settings.Default.Save();
                    sqlConnection.Close();
                    sqlDataReader.Close();

                    User.User user = new User.User();
                    user.Show();
                    return true;
                }
                MessageBox.Show("Цей Логін вже занятий");
                return false;
            }
        }

        // Логін
        public bool Login(string login, string password)
        {
            if (login == "" | password == "")
            {
                MessageBox.Show("Введіть логін і пароль");
                return false;
            }
            else
            {
                SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnectionString);
                SqlDataReader sqlDataReader;

                SqlCommand cmd = new SqlCommand("SELECT Login,Password,AdminStatus,Id FROM Users WHERE Login = @Login and Password = @Password ", sqlConnection);
                cmd.Parameters.Add(new SqlParameter("Login", login));
                cmd.Parameters.Add(new SqlParameter("Password", password.GetHashCode()));

                sqlConnection.Open();
                sqlDataReader = cmd.ExecuteReader();
                sqlDataReader.Read();

                if (!sqlDataReader.HasRows)
                {
                    sqlConnection.Close();
                    sqlDataReader.Close();
                    MessageBox.Show("Вибачте,ви не зареєстровані", "УПС");
                    return false;
                }
                else
                {
                    bool adminStatus = (bool)sqlDataReader[2];
                    Properties.Settings.Default.IdUser = (int)sqlDataReader[3];
                    Properties.Settings.Default.Save();
                    if (adminStatus == true)
                    {
                        Administrator.Administrator admin = new Administrator.Administrator();
                        admin.Show();
                    }
                    else
                    {
                        User.User user = new User.User();
                        user.Show();
                    }
                }
                sqlConnection.Close();
                return true;
            }
        }

        // Перевірити Баланс
        public int CheckTheBalance()
        {
            SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnectionString);
            SqlDataReader sqlDataReader;

            SqlCommand sqlCommand = new SqlCommand("Select Money FROM Users WHERE Id = @Id ", sqlConnection);
            sqlCommand.Parameters.Add(new SqlParameter("Id", Properties.Settings.Default.IdUser));

            try
            {
                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                sqlDataReader.Read();
                balance = (int)sqlDataReader[0];
                sqlConnection.Close();
                sqlDataReader.Close();
                return balance;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // Поповнити рахунок
        public void AddFunds(string howMuch)
        {
            int verification = 0;
            if (howMuch == "")
            {
                MessageBox.Show("Введіть суму яку необхідно занести");
            }
            else if (!Int32.TryParse(howMuch, out verification))
            {
                MessageBox.Show("Введіть число");
            }
            else
            {
                SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnectionString);
                SqlCommand sqlCommand = new SqlCommand("UPDATE Users SET Money = @Money WHERE Id = @Id ", sqlConnection);
                sqlCommand.Parameters.Add(new SqlParameter("Money", balance + int.Parse(howMuch)));
                sqlCommand.Parameters.Add(new SqlParameter("Id", Properties.Settings.Default.IdUser));
                try
                {
                    sqlConnection.Open();
                    sqlCommand.ExecuteReader();
                    sqlConnection.Close();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                MessageBox.Show("Рахунок успішно поповнено");

            }
        }

        // Зняти готівку
        public void WithdrawCash(string howMuch)
        {
            int verification = 0;
            if (howMuch == "")
            {
                MessageBox.Show("Введіть суму яку необхідно занести");
            }
            else if (!Int32.TryParse(howMuch, out verification))
            {
                MessageBox.Show("Введіть число");
            }
            else
            {
                int sum = balance - int.Parse(howMuch);
                if (sum >= 0)
                {
                    SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnectionString);
                    SqlCommand sqlCommand = new SqlCommand("UPDATE Users SET Money = @Money WHERE Id = @Id ", sqlConnection);
                    sqlCommand.Parameters.Add(new SqlParameter("Money", sum));
                    sqlCommand.Parameters.Add(new SqlParameter("Id", Properties.Settings.Default.IdUser));
                    try
                    {
                        sqlConnection.Open();
                        sqlCommand.ExecuteNonQuery();
                        sqlConnection.Close();
                        
                    }
                    catch (Exception e)
                    {

                        throw new Exception(e.Message);
                    }
                    MessageBox.Show("Кошти успішно зняті");
                }
                else
                {
                    MessageBox.Show("Недостатньо коштів на рахунку");
                }
            }
        }
        
        // Перевести готівку
        public void TtransferFunds(string howMuch,string loginRecipient)
        {

        }
    }
}
