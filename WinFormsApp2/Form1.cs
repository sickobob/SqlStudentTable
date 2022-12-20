using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string connectionString = @"Server=emiit.ru;Database=tereshkin_epb211;User Id=user-96;Password=Baf62932;TrustServerCertificate=True";
        private void button1_Click(object sender, EventArgs e)
        {


            string Login = textBox1.Text;

            string Password = textBox2.Text;

            string ID = "0";
            string role = " ";
            SqlDataReader DBPassword;

            string pass = "";

            string sqlExpression = "SELECT Password, ID, Role FROM Users WHERE Login = @Login";

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();


            SqlCommand command = new SqlCommand(sqlExpression, connection);

            SqlParameter LoginParam = new SqlParameter("@Login", Login);

            // добавляем параметр к команде​

            command.Parameters.Add(LoginParam);

            DBPassword = command.ExecuteReader();
            if (DBPassword.HasRows)
            {
                while (DBPassword.Read())// построчно считываем данные​
                {
                    pass = Convert.ToString(DBPassword.GetValue(0));

                    ID = Convert.ToString(DBPassword.GetValue(1));
                    role = DBPassword.GetString(2).Trim();
                }

                if (pass != Password) MessageBox.Show("Пароль неверный");
                else
                {
                    Form2 form2 = new Form2(role,Login);
                    form2.ShowDialog();
                }

            }
      
            else MessageBox.Show("Пользователь не найден");
            DBPassword.Close();
            connection.Close();
        }
        int GetMaxId()
        {
            int count = 0;
            string sqlExpr = "Select MAX(ID) from users";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sqlExpr, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    count = reader.GetInt32(0);
               
                }
                reader.Close();
                connection.Close();
          
            }
            return count;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "Вход и регистрация";
        }
        bool FindLogin()
        {
            string Login = textBox1.Text;
            SqlDataReader DBPassword;
            string sqlExpression = "SELECT Password, ID, Role FROM Users WHERE Login = @Login";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            SqlParameter LoginParam = new SqlParameter("@Login", Login);
            command.Parameters.Add(LoginParam);
            DBPassword = command.ExecuteReader();
            if (DBPassword.HasRows)
            {
                while (DBPassword.Read())
                {
                    MessageBox.Show("Данный пользователь уже есть в базе данных, войдите в аккаунт или зарегестрируйтесь под другим логином");
                    connection.Close();
                    return true;
                }
            }
            connection.Close();
            return false;
        }
        int AddUser()
        {

            string sqlExpression = $"INSERT INTO Users (Id,Login,Password,Role) VALUES ('{GetMaxId()+1}','{textBox1.Text}','{textBox2.Text}','guest')";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            return command.ExecuteNonQuery();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (!FindLogin())
            {
                AddUser();
                MessageBox.Show("Вы успешно зарегистрировались");
            }       
        }
    }
}
