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
using System.Text.RegularExpressions;

namespace DB
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;
        private SqlConnection nrtwindConnection = null;
        private List<string> columList = new List<string>() { null};

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
            

            columList.Add("ProductID");
            //columList.Add("ProductName");
            columList.Add("SupplierID");
            columList.Add("CategoryID");
            //columList.Add("QuantityPerUnit");
            columList.Add("UnitPrice");
            columList.Add("UnitsInStock");
            columList.Add("UnitsOnOrder");
            columList.Add("ReorderLevel");
            columList.Add("Discontinued");

            listBox1.Items.Clear();
            checkedListBox1.Items.Clear();

            //Создание и удаление tabPage
            Login.TabPages.RemoveAt(0);
            Login.TabPages.RemoveAt(0);
            Login.TabPages.RemoveAt(1);
            //Login.TabPages.Add(tabPage1);
            //Login.TabPages.Add(tabPage2);


            for (int i = 0; i < columList.Count; ++i)
            {
                if (columList[i] != null)
                {
                    listBox1.Items.Add(columList[i]);
                    checkedListBox1.Items.Add(columList[i]);
                }
            }
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


        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //Обработка строки
            string box7str = textBox7.Text;
            box7str = Regex.Replace(box7str, @"[^\d]+", "");
            if (box7str == "")
            {
                box7str = "0";
            }

            //Обработка чекбокса
            string chekboxStr = null;
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                chekboxStr = "*";
            }
            else
            {
                for (int i = 0; i < columList.Count; ++i)
                {
                    if (checkedListBox1.CheckedItems.Contains(columList[i]) == true)
                    {
                        chekboxStr += columList[i];
                        chekboxStr += ", ";
                    }
                }
                chekboxStr = chekboxStr.Remove(chekboxStr.Length - 1);
                chekboxStr = chekboxStr.Remove(chekboxStr.Length - 1);
            }

            if (listBox1.SelectedItem != null && listBox2.SelectedItem != null && checkedListBox1.SelectedItems != null)
            {
                //{checkedListBox1.SelectedItems.ToString()}
                SqlDataAdapter dataAdapter = new SqlDataAdapter(
                /*SELECT  Выбранные столбики                          Столбец                          Знак { >, <, = }         Значение*/
                $"SELECT {chekboxStr} FROM Products WHERE {listBox1.SelectedItem.ToString()} {listBox2.SelectedItem.ToString()} {box7str}",
                nrtwindConnection);
                
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dataSet.Tables[0];
            }
            else
            {
                MessageBox.Show("Не выбран Элемент");
            }
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox9.Text != null && textBox10.Text != null)
            {
                SqlCommand command = new SqlCommand(
                $"SELECT COUNT(*) FROM LoginBase WHERE Login = N'{textBox9.Text}' AND Password = N'{textBox10.Text}'",
                nrtwindConnection);

                if (Int32.Parse(command.ExecuteScalar().ToString()) > 0)
                {
                    Login.TabPages.RemoveAt(0);
                    Login.TabPages.Add(tabPage1);
                    Login.TabPages.Add(tabPage2);
                }
                else
                MessageBox.Show("Неверный логин или пароль");
            }
            else
            {
                MessageBox.Show("Не полностью введены данные");
            }

        }
        private void button4_Click(object sender, EventArgs e)
        {
            Login.TabPages.RemoveAt(0);
            Login.TabPages.Add(tabPage4);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox14.Text != null && textBox17.Text != null)
            {
                SqlCommand command1 = new SqlCommand(
                $"SELECT COUNT(*) FROM LoginBase WHERE Login = N'{textBox14.Text}'",
                nrtwindConnection);

                if (Int32.Parse(command1.ExecuteScalar().ToString()) == 0)
                {
                    SqlCommand command = new SqlCommand(
                    $"INSERT INTO [LoginBase] (Login, Password) VALUES(N'{textBox14.Text}', N'{textBox17.Text}')",
                    nrtwindConnection);

                    Login.TabPages.RemoveAt(0);
                    Login.TabPages.Add(tabPage3);
                    command.ExecuteNonQuery();
                    Update_Gird(dataGridView2);
                    MessageBox.Show("Успешная Регистрация");
                }
                else
                {
                    MessageBox.Show("Пользователь уже зарегестрирован");
                }
            }
            else
            {
                MessageBox.Show("Не полностью введены данные");
            }
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void Login_Selected(object sender, TabControlEventArgs e)
        {
            Update_Gird(dataGridView2);
        }

        private void Update_Gird(DataGridView dataGrid, string strSQL = "SELECT Login, Password FROM LoginBase")
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter(
            strSQL,
            nrtwindConnection);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);

            dataGrid.DataSource = null;
            dataGrid.DataSource = dataSet.Tables[0];
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Login.TabPages.RemoveAt(0);
            Login.TabPages.Add(tabPage3);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
