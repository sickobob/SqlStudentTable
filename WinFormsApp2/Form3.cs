using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public partial class Form3 : Form
    {
        public Form3(string role)
        {
            InitializeComponent();
            Role = role;
        }
        string connectionString = @"Server=emiit.ru;Database=tereshkin_epb211;User Id=user-96;Password=Baf62932;TrustServerCertificate=True";
        static string Role;

        private void Form3_Load(object sender, EventArgs e)
        {
            Text = "Список группы";
            if (Role == "guest")
            {
                button2.Visible = false;
                button3.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        private void button3_Click(object sender, EventArgs e)
        {
            int delet = dataGridView1.SelectedCells[0].RowIndex;
            int deletedStudent = (int)dataGridView1.Rows[delet].Cells[1].Value;
            string sqlExp = "DELETE FROM Studentsv1 Where Snumber = @Snumber";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                 connection.Open();
                SqlCommand command = new SqlCommand(sqlExp, connection);
                SqlParameter deletParam = new SqlParameter("@Snumber", deletedStudent);
                command.Parameters.Add(deletParam);
                int number = command.ExecuteNonQuery();
                MessageBox.Show($"Удалено объектов: {number}");
            }
            dataGridView1.Rows.RemoveAt(delet);
        }
    }
}
