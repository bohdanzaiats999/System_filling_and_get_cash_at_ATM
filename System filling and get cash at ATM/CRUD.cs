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
                SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionString);
                SqlDataReader reader;

                SqlCommand cmd = new SqlCommand("SELECT Login FROM Users WHERE Login = @Login", connect);
                cmd.Parameters.Add(new SqlParameter("Login", login));

                connect.Open();
                reader = cmd.ExecuteReader();
                reader.Read();

                if (!reader.HasRows)
                {
                    // Реєстрація
                    connect.Close();
                    reader.Close();

                    cmd.CommandText = "INSERT INTO Users VALUES (@Login,@Password,@Money,@BlockStatus,@AdminStatus,@Limit)";

                    cmd.Parameters.Add(new SqlParameter("Password", password.GetHashCode()));
                    cmd.Parameters.Add(new SqlParameter("Money", 100));
                    cmd.Parameters.Add(new SqlParameter("BlockStatus", false)); // false - заборонені дії в системі , true - дозволені дії в системі
                    cmd.Parameters.Add(new SqlParameter("AdminStatus", false)); // false - простий користувач, true - адміністратор
                    cmd.Parameters.Add(new SqlParameter("Limit",1000));

                    connect.Open();
                    reader = cmd.ExecuteReader();
                    connect.Close();
                    reader.Close();

                    cmd.CommandText = "SELECT Id FROM Users WHERE Login = @Login";
                    connect.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    Properties.Settings.Default.IdUser = (int)reader[0];
                    Properties.Settings.Default.Save();
                    connect.Close();
                    reader.Close();

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
                SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionString);
                SqlDataReader reader;

                SqlCommand cmd = new SqlCommand("SELECT Login,Password,AdminStatus,Id FROM Users WHERE Login = @Login and Password = @Password ", connect);
                cmd.Parameters.Add(new SqlParameter("Login", login));
                cmd.Parameters.Add(new SqlParameter("Password", password.GetHashCode()));

                connect.Open();
                reader = cmd.ExecuteReader();
                reader.Read();

                if (!reader.HasRows)
                {
                    connect.Close();
                    reader.Close();
                    MessageBox.Show("Вибачте,ви не зареєстровані", "УПС");
                    return false;
                }
                else
                {
                    bool adminStatus = (bool)reader[2];
                    Properties.Settings.Default.IdUser = (int)reader[3];
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
                connect.Close();
                return true;
            }
        }

    }
}
