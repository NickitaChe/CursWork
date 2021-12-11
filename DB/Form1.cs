using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace DB
{
    
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;
        private SqlConnection nrtwindConnection = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBKey"].ConnectionString);

            sqlConnection.Open();

            nrtwindConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString);

            nrtwindConnection.Open();



        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand(
                $"INSERT INTO [Students] (Name, Surname, BirthDay, BurnState, Phone, EMail) VALUES(@Name, @Surname, @BirthDay, @BurnState, @Phone, @EMail)",
                sqlConnection);
            DateTime date = DateTime.Parse(textBox3.Text);


            command.Parameters.AddWithValue("Name", textBox1.Text);
            command.Parameters.AddWithValue("Surname", textBox2.Text);
            command.Parameters.AddWithValue("BirthDay", $"{date.Month}/{date.Day}/{date.Year}");
            command.Parameters.AddWithValue("BurnState", textBox4.Text);
            command.Parameters.AddWithValue("Phone", textBox5.Text);
            command.Parameters.AddWithValue("EMail", textBox6.Text);

            MessageBox.Show(command.ExecuteNonQuery().ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter(
                "SELECT * FROM Products WHERE UnitPrice > 100",
                nrtwindConnection);

            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet);

            dataGridView1.DataSource = dataSet.Tables[0];

        }
    }
}
