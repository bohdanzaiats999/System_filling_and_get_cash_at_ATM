using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Windows.Documents;

namespace System_filling_and_get_cash_at_ATM
{
    class CRUD
    {
        SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnectionString);
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
                SqlDataReader sqlDataReader;

                SqlCommand cmd = new SqlCommand("SELECT Login,Password,AdminStatus,Id,BlockStatus FROM Users WHERE Login = @Login and Password = @Password ", sqlConnection);
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
                    bool blockStatus = (bool)sqlDataReader[4];

                    Properties.Settings.Default.IdUser = (int)sqlDataReader[3];
                    Properties.Settings.Default.Save();
                    Properties.Settings.Default.Login = login;
                    if (adminStatus == true)
                    {
                        Administrator.Administrator admin = new Administrator.Administrator();
                        admin.Show();
                    }
                    else
                    {
                        if (blockStatus)
                        {
                            MessageBox.Show("Ваш акаунт заблокований,очікуйте розблокування зі сторони Адміністратора");
                        }
                        else
                        {
                            User.User user = new User.User();
                            user.Show();
                        }
                    }
                }
                sqlConnection.Close();
                return true;
            }
        }

        // Перевірити Баланс
        public int CheckTheBalance()
        {
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
                SqlCommand sqlCommand = new SqlCommand("UPDATE Users SET Money = @Money WHERE Id = @Id ", sqlConnection);
                sqlCommand.Parameters.Add(new SqlParameter("Money", balance + int.Parse(howMuch)));
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
                DateTime localDate = DateTime.Now;

                sqlCommand.CommandText = "INSERT INTO Histori VALUES(@Id,@HowMuch,@Operation,@Date)";
                sqlCommand.Parameters.Add(new SqlParameter("HowMuch", int.Parse(howMuch)));
                sqlCommand.Parameters.Add(new SqlParameter("Operation", "Поповнення рахунку"));
                sqlCommand.Parameters.Add(new SqlParameter("Date", localDate));
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
                MessageBox.Show("Рахунок успішно поповнено");

            }
        }

        // Зняти готівку
        public void WithdrawCash(string howMuch)
        {
            int verification = 0;
            if (howMuch == "")
            {
                MessageBox.Show("Введіть суму яку необхідно зняти");
            }
            else if (!Int32.TryParse(howMuch, out verification))
            {
                MessageBox.Show("Введіть число");
            }
            else
            {
                SqlDataReader sqlDataReader;
                SqlCommand sqlCommand = new SqlCommand("SELECT Limit FROM Users WHERE Id = @Id ", sqlConnection);
                sqlCommand.Parameters.Add(new SqlParameter("Id", Properties.Settings.Default.IdUser));

                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                sqlDataReader.Read();
                int limit = (int)sqlDataReader[0];
                sqlConnection.Close();

                if (int.Parse(howMuch) > limit)
                {
                    MessageBox.Show("Перевищений ліміт виведення коштів");
                }
                else
                {

                    int sum = balance - int.Parse(howMuch);
                    if (sum >= 0)
                    {
                        sqlCommand.CommandText = "UPDATE Users SET Money = @Money WHERE Id = @Id ";
                        sqlCommand.Parameters.Add(new SqlParameter("Money", sum));
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
                        DateTime localDate = DateTime.Now;

                        sqlCommand.CommandText = "INSERT INTO Histori VALUES(@Id,@HowMuch,@Operation,@Date)";
                        sqlCommand.Parameters.Add(new SqlParameter("HowMuch", int.Parse(howMuch)));
                        sqlCommand.Parameters.Add(new SqlParameter("Operation", "Отримання коштів готівкою"));
                        sqlCommand.Parameters.Add(new SqlParameter("Date", localDate));
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
        }

        // Перевести готівку
        public void TtransferFunds(string howMuch, string loginRecipient)
        {
            int verification = 0;
            if (howMuch == "" || loginRecipient == "")
            {
                MessageBox.Show("Введіть суму яку необхідно перечислити і логін");
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
                    // Провірка наявності Логіна
                    SqlDataReader sqlDataReader;
                    SqlCommand sqlCommand = new SqlCommand("SELECT Login FROM Users Where Login = @LoginRecipient ", sqlConnection);
                    sqlCommand.Parameters.Add(new SqlParameter("LoginRecipient", loginRecipient));

                    try
                    {
                        sqlConnection.Open();
                        sqlDataReader = sqlCommand.ExecuteReader();
                        sqlDataReader.Read();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }

                    if (!sqlDataReader.HasRows)
                    {
                        sqlDataReader.Close();
                        sqlConnection.Close();
                        MessageBox.Show("Введений невірний Логін");
                    }
                    else
                    {


                        sqlDataReader.Close();
                        sqlConnection.Close();

                        //Вилучення коштів відправляючого

                        sqlCommand.CommandText = "UPDATE Users SET Money = @Money WHERE Id = @Id ";
                        sqlCommand.Parameters.Add(new SqlParameter("Money", sum));
                        sqlCommand.Parameters.Add(new SqlParameter("Id", Properties.Settings.Default.IdUser));
                        try
                        {
                            sqlConnection.Open();
                            sqlCommand.ExecuteNonQuery();
                            sqlConnection.Close();
                            balance = sum;

                        }
                        catch (Exception e)
                        {
                            throw new Exception(e.Message);
                        }

                        // Добавлення коштів отримувачу

                        sqlCommand.CommandText = "UPDATE Users SET Money = Money + @howMuch WHERE Login = @log";
                        sqlCommand.Parameters.Add(new SqlParameter("log", loginRecipient));
                        sqlCommand.Parameters.Add(new SqlParameter("howMuch", int.Parse(howMuch)));
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
                        // Занесення інформації в історію операцій
                        DateTime localDate = DateTime.Now;

                        sqlCommand.CommandText = "INSERT INTO Histori VALUES(@Id,@HowMuchMoney,@Operation,@Date)";
                        sqlCommand.Parameters.Add(new SqlParameter("HowMuchMoney", int.Parse(howMuch)));
                        sqlCommand.Parameters.Add(new SqlParameter("Operation", "Перечислення коштів користувачу " + loginRecipient + ""));
                        sqlCommand.Parameters.Add(new SqlParameter("Date", localDate));
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
                        // Зчитування ID отримувача для занесення інформації в історію отримувача
                        sqlCommand.CommandText = "SELECT Id FROM Users WHERE Login = @log ";
                        int idRecipient = 0;
                        try
                        {
                            sqlConnection.Open();
                            sqlDataReader = sqlCommand.ExecuteReader();
                            sqlDataReader.Read();
                            idRecipient = (int)sqlDataReader[0];
                            sqlConnection.Close();
                            sqlDataReader.Close();
                        }
                        catch (Exception e)
                        {
                            throw new Exception(e.Message);
                        }
                        sqlCommand.CommandText = "INSERT INTO Histori VALUES(@IdRecipient,@HowMuchMoney,@OperationForRecipient,@Date)";
                        sqlCommand.Parameters.Add(new SqlParameter("IdRecipient", idRecipient));
                        sqlCommand.Parameters.Add(new SqlParameter("OperationForRecipient", "Отримання коштів від " + Properties.Settings.Default.Login + " "));

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
                        MessageBox.Show("Кошти успішно перечислені");
                    }
                }
                else
                {
                    MessageBox.Show("Недостатньо коштів на рахунку");
                }

            }
        }

        // Заблокувати/Розблокувати користувача
        public void BlockUnblockUser(string id)
        {
            int verification = 0;
            if (id == "")
            {
                MessageBox.Show("Введіть ID");
            }
            else if (!Int32.TryParse(id, out verification))
            {
                MessageBox.Show("Введіть число");
            }
            else
            {
                SqlDataReader sqlDataReader;
                SqlCommand sqlCommand = new SqlCommand("SELECT BlockStatus FROM Users WHERE Id = @Id ", sqlConnection);
                sqlCommand.Parameters.Add(new SqlParameter("Id", id));

                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                sqlDataReader.Read();

                if (!sqlDataReader.HasRows)
                {
                    sqlDataReader.Close();
                    sqlConnection.Close();
                    MessageBox.Show("ID невірний");
                }
                else
                {
                    bool blockStatus = (bool)sqlDataReader[0];
                    sqlConnection.Close();
                    sqlDataReader.Close();

                    sqlCommand.CommandText = "UPDATE Users SET BlockStatus = @Status WHERE Id = @Id";
                    sqlCommand.Parameters.Add(new SqlParameter("Status", !blockStatus));

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();

                    if (blockStatus == true)
                    {
                        MessageBox.Show("Користувач Розблокований");
                    }
                    else
                    {
                        MessageBox.Show("Користувач Заблокований");
                    }
                }
            }
        }

        // Встановити ліміт розрахунків
        public void SetALimitCalculations(string id, string limit)
        {
            int verification = 0;
            if (id == "" | limit == "")
            {
                MessageBox.Show("Введіть ID і ліміт");
            }
            else if (!Int32.TryParse(id, out verification) | !Int32.TryParse(limit, out verification))
            {
                MessageBox.Show("Введіть число в два поля");
            }
            else
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT Id FROM Users WHERE Id = @Id ", sqlConnection);
                SqlDataReader sqlDataReader;
                sqlCommand.Parameters.Add(new SqlParameter("@Id", int.Parse(id)));

                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                sqlDataReader.Read();

                if (!sqlDataReader.HasRows)
                {
                    MessageBox.Show("ID невірний");
                    sqlConnection.Close();
                    sqlDataReader.Close();
                }
                else
                {
                    sqlConnection.Close();
                    sqlDataReader.Close();

                    sqlCommand.CommandText = "UPDATE Users SET Limit = @Limit WHERE Id = @Id ";
                    sqlCommand.Parameters.Add(new SqlParameter("Limit", int.Parse(limit)));

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();

                    MessageBox.Show("Ліміт Встановлений");
                }
            }
        }

        // Обновити RichTextBoxProvideBilling
        public void richTextBoxProvideBillingRefresh(ref RichTextBox richTextBox, string id)
        {
            int verification = 0;
            if (id == "")
            {
                MessageBox.Show("Введіть ID і ліміт");
            }
            else if (!Int32.TryParse(id, out verification))
            {
                MessageBox.Show("Введіть число в поле");
            }
            else
            {
                SqlDataReader sqlDataReader;
                SqlCommand sqlCommand = new SqlCommand("SELECT IdUser FROM Histori WHERE IdUser = @IdUser ", sqlConnection);
                sqlCommand.Parameters.Add(new SqlParameter("IdUser", id));

                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                sqlDataReader.Read();

                if (!sqlDataReader.HasRows)
                {
                    sqlDataReader.Close();
                    sqlConnection.Close();
                    MessageBox.Show("ID невірний або операції за цим рахунком не виконувались");
                }
                else
                {
                    sqlDataReader.Close();
                    sqlConnection.Close();
                    richTextBox.Document.Blocks.Clear();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Histori WHERE IdUser = " + int.Parse(id) + "", sqlConnection);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "Histori");
                    foreach (DataRow dr in ds.Tables["Histori"].Rows)
                    {
                        richTextBox.Document.Blocks.Add(new Paragraph(new Run(dr["Date"].ToString() + " була виконана операція " + dr["Operation"].ToString() + " на суму " + dr["Money"].ToString() + " грн")));
                    }
                }
            }
        }
        // Обновити dataGrid
        public void DataGridRefresh(ref DataGrid dataGrid)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT Id as ID,Login as Логін,Money as Рахунок ,BlockStatus as Заблоковані,Limit as Ліміт FROM Users ", sqlConnection);
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            dataGrid.ItemsSource = dataSet.Tables[0].DefaultView;
        }
    }
}
