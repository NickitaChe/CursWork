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

        private static int sellsCount_;
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

            //listBox1.Items.Clear();
           // checkedListBox1.Items.Clear();

            //Создание и удаление tabPage
            Login.TabPages.Clear();
            Login.TabPages.Add(tabPage3);
            //Login.TabPages.Add(tabPage1);
            //Login.TabPages.Add(tabPage2);

            GetSellCount();            

            for (int i = 0; i < columList.Count; ++i)
            {
                if (columList[i] != null)
                {
                    //listBox1.Items.Add(columList[i]);
                    //checkedListBox1.Items.Add(columList[i]);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Login.TabPages.Clear();
            Login.TabPages.Add(tabPage3);
            clearAlltextBoxes();

        }

        private void button2_Click(object sender, EventArgs e)
        {


        }
        /*
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
                /*SELECT  Выбранные столбики                          Столбец                          Знак { >, <, = }         Значение*//*
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
        }*/

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
                $"SELECT COUNT(*) FROM Sellers WHERE Login = N'{textBox9.Text}' AND Password = N'{textBox10.Text}'",
                sqlConnection);

                if (Int32.Parse(command.ExecuteScalar().ToString()) > 0)
                {
                    Login.TabPages.RemoveAt(0);
                    Login.TabPages.Add(tabPage1);
                    //Login.TabPages.Add(tabPage2);
                    Login.TabPages.Add(tabPage5);
                    Login.TabPages.Add(tabPage8);
                    Login.TabPages.Add(tabPage9);
                    update_Gird(dataGridView6, sqlConnection, "SELECT * FROM Units");
                    
                    update_Gird(dataGridView10, sqlConnection, "SELECT * FROM( SELECT  u.Unit_name AS 'Наименование', SUM(sc.Quantity * (u.SELL_Price - u.BUY_Price)) AS 'Доход' FROM Units u, SellChek sc WHERE sc.Unit_Id = u.ID GROUP BY u.Unit_name) b");
                    update_Gird(dataGridView11, sqlConnection, "SELECT u.Unit_name AS 'Наименование', u.Quantity AS 'Количество'  FROM Units u WHERE u.Quantity < 100");
                    update_Gird(dataGridView8, sqlConnection, "SELECT sc.Date, SUM(sc.Quantity*(u.SELL_Price-u.BUY_Price)) AS 'Доход' FROM SellChek sc LEFT JOIN Units u ON sc.Unit_Id =u.ID	GROUP BY sc.Date");
                    update_Gird(dataGridView9, sqlConnection, "SELECT DATENAME(m, sc.Date)+DATENAME(yy, sc.Date) , SUM(sc.Quantity * (u.SELL_Price - u.BUY_Price)) AS 'Доход' FROM SellChek sc LEFT JOIN Units u ON sc.Unit_Id = u.ID GROUP BY  DATENAME(m, sc.Date) + DATENAME(yy, sc.Date)");
                    update_Gird(dataGridView7, sqlConnection, $"SELECT s.Name AS 'Имя', s.Surname AS 'Фамилия', SUM(s.Commision*sc.Quantity*(u.SELL_Price-u.BUY_Price)) AS 'Зарплата' FROM Sellers s JOIN SellChek sc ON s.ID = sc.Seller_Id JOIN Units u	ON sc.Unit_Id = u.ID WHERE DATEPART(m, sc.date) = DATEPART(m, N'{splitDatetoSql(dateTimePicker2.Value.ToString())}') AND DATEPART(yy, sc.date) = DATEPART(yy, N'{splitDatetoSql(dateTimePicker2.Value.ToString())}') GROUP BY s.Name,s.Surname");

                    command = new SqlCommand(
                    $"SELECT TOP(1) s.Name+s.Surname AS 'имя', SUM(s.Commision * sc.Quantity * (u.SELL_Price - u.BUY_Price)) AS 'Зарплата' FROM Sellers s JOIN SellChek sc ON s.ID = sc.Seller_Id JOIN Units u ON sc.Unit_Id = u.ID GROUP BY s.Name,s.Surname",
                    sqlConnection);

                    string TopMNGR = command.ExecuteScalar().ToString();

                    string[] B = TopMNGR.Split();

                    command = new SqlCommand(
                    $"SELECT TOP(1) s.Surname AS 'имя', SUM(s.Commision * sc.Quantity * (u.SELL_Price - u.BUY_Price)) AS 'Зарплата' FROM Sellers s JOIN SellChek sc ON s.ID = sc.Seller_Id JOIN Units u ON sc.Unit_Id = u.ID GROUP BY s.Name,s.Surname",
                    sqlConnection);

                    B[1] = command.ExecuteScalar().ToString();

                    textBox53.Text = B[0]+" "+B[1];




                    command = new SqlCommand(
                    $"SELECT TOP(1) DATENAME(m, sc.Date)+DATENAME(yy, sc.Date) , SUM(sc.Quantity * (u.SELL_Price - u.BUY_Price)) AS 'Доход' FROM SellChek sc LEFT JOIN Units u ON sc.Unit_Id = u.ID GROUP BY  DATENAME(m, sc.Date) + DATENAME(yy, sc.Date)",
                    sqlConnection);
                    textBox56.Text = command.ExecuteScalar().ToString();

                    command = new SqlCommand(
                    $"SELECT TOP(1) sc.Date, SUM(sc.Quantity*(u.SELL_Price-u.BUY_Price)) AS 'Доход' FROM SellChek sc LEFT JOIN Units u ON sc.Unit_Id =u.ID	GROUP BY sc.Date  ORDER BY ~SUM(sc.Quantity*(u.SELL_Price-u.BUY_Price))",
                    sqlConnection);
                    textBox54.Text = command.ExecuteScalar().ToString();

                    command = new SqlCommand(
                    $"SELECT Name FROM Sellers WHERE Login = N'{textBox9.Text}' AND Password = N'{textBox10.Text}'",
                    sqlConnection);

                    string UserRealName = command.ExecuteScalar().ToString();

                    string[] A = UserRealName.Split();

                    command = new SqlCommand(
                    $"SELECT  Surname FROM Sellers WHERE Login = N'{textBox9.Text}' AND Password = N'{textBox10.Text}'",
                    sqlConnection);
                    A[0] += " ";
                    A[0] += command.ExecuteScalar().ToString();

                    GetSellCount();

                    
                    textBox32.Text = $"Logined as {A[0]} ";

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
            if (textBox14.Text != null && textBox17.Text != null && textBox25.Text != null && textBox4.Text != null)
            {
                SqlCommand command1 = new SqlCommand(
                $"SELECT COUNT(*) FROM Sellers WHERE Login = N'{textBox14.Text}'",
                sqlConnection);

                if (Int32.Parse(command1.ExecuteScalar().ToString()) == 0)
                {
                                     

                    SqlCommand command = new SqlCommand(
                    $"INSERT INTO [Sellers] (Name, Surname, Telnum, Email, Commision, Login, Password) VALUES(N'{textBox25.Text}', N'{textBox4.Text}', N'{textBox29.Text}', N'{textBox26.Text}', 0.15, N'{textBox14.Text}', N'{textBox17.Text}')",
                    sqlConnection);

                    Login.TabPages.RemoveAt(0);
                    Login.TabPages.Add(tabPage3);
                    command.ExecuteNonQuery();
                    update_Gird(dataGridView2, sqlConnection);
                    clearAlltextBoxes();
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
            update_Gird(dataGridView2, sqlConnection);
        }

        private void update_Gird(DataGridView dataGrid, SqlConnection sqlSource, string strSQL = "SELECT Login, Password FROM Sellers")
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter(
            strSQL,
            sqlSource);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);

            dataGrid.DataSource = null;
            dataGrid.DataSource = dataSet.Tables[0];
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Login.TabPages.RemoveAt(0);
            Login.TabPages.Add(tabPage3);
            clearAlltextBoxes();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand(
                    $"SELECT ID FROM Sellers WHERE Login = N'{textBox9.Text}' AND Password = N'{textBox10.Text}'",
                    sqlConnection);
            int SellerID = Int32.Parse(command.ExecuteScalar().ToString());

            command = new SqlCommand(
                    $"SELECT TOP(1) SellChek.Sell_numb FROM SellChek ORDER BY ~Sell_numb",
                    sqlConnection);
            int sellid_ = Int32.Parse(command.ExecuteScalar().ToString());

            if (sellid_ == sellsCount_)
            {
                sellid_ += 1;
            }

            command = new SqlCommand(
                $"SELECT Quantity FROM Units WHERE ID = N'{textBox30.Text}'",
                sqlConnection);
            if(Int32.Parse(command.ExecuteScalar().ToString()) >= numericUpDown1.Value)
            {



                command = new SqlCommand(
                $"INSERT INTO SellChek (Unit_Id, Costumer_Id, Seller_Id, Quantity, Sell_numb, Date) VALUES(N'{textBox30.Text}',N'1',N'{SellerID}',N'{numericUpDown1.Value}',N'{sellid_}', N'{splitDatetoSql(dateTimePicker1.Value.ToString())}')",
                sqlConnection);
                command.ExecuteNonQuery();

                update_Gird(dataGridView4, sqlConnection, $"SELECT Unit_name,sc.Quantity, SELL_Price,sc.Quantity*u.SELL_Price AS 'Цена' FROM Units u, SellChek sc WHERE sc.Unit_Id = u.ID AND sc.Sell_numb = {sellid_}");



                command = new SqlCommand(
                    $"UPDATE Units  SET Quantity = Quantity - {numericUpDown1.Value} WHERE ID = {textBox30.Text}",
                    sqlConnection);
                command.ExecuteNonQuery();
            }
            else
            {
                MessageBox.Show("Неверное число элементов");
            }
            
                command = new SqlCommand(
                    $"SELECT SUM(sc.Quantity*u.SELL_Price)FROM SellChek sc LEFT JOIN Units u ON sc.Unit_Id = u.ID WHERE sc.Sell_numb = {sellid_} ",
                    sqlConnection);
            

            textBox3.Text = command.ExecuteScalar().ToString();
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            update_Gird(dataGridView3, sqlConnection, $"SELECT Unit_name, SELL_Price, Quantity FROM Units WHERE Unit_name LIKE N'%{textBox1.Text}%'");

        }

        private void textBox31_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //выбор элемента из таблицы
            SqlCommand command = new SqlCommand(
                $"SELECT TOP(1) ID FROM(SELECT TOP({e.RowIndex + 1}) ID FROM Units WHERE Unit_name LIKE N'%{textBox1.Text}%' ORDER BY ID) a ORDER BY ~ID",
                sqlConnection);
            textBox30.Text = command.ExecuteScalar().ToString();


            command = new SqlCommand(
                $"SELECT Quantity FROM Units WHERE ID = {textBox30.Text}",
                sqlConnection);
            numericUpDown1.Maximum = Int32.Parse(command.ExecuteScalar().ToString());

        }

        private void clearAlltextBoxes()
        {
            textBox1.Text = "";
            textBox30.Text = "";
            textBox14.Text = "";
            textBox17.Text = "";
            textBox25.Text = "";
            textBox4.Text = "";
            textBox29.Text = "";
            textBox26.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (textBox33.Text != null && textBox34.Text != null && textBox35.Text != null && textBox36.Text != null && textBox37.Text != null)
            {
                SqlCommand command1 = new SqlCommand(
                $"SELECT COUNT(*) FROM Units WHERE Unit_name = N'{textBox33.Text}'",
                sqlConnection);

                if (Int32.Parse(command1.ExecuteScalar().ToString()) == 0)
                {
                    SqlCommand command = new SqlCommand(
                    $"INSERT INTO [Units] (Unit_name, Unit_of_measurement, BUY_Price, SELL_Price, Quantity) VALUES(N'{textBox33.Text}', N'{textBox34.Text}', N'{textBox35.Text}', N'{textBox36.Text}', N'{textBox37.Text}')",
                    sqlConnection);

                    command.ExecuteNonQuery();
                    update_Gird(dataGridView6, sqlConnection,"SELECT * FROM Units");
                    
                    MessageBox.Show("Успешно Добавленно");
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

        private void textBox33_TextChanged(object sender, EventArgs e)
        {
            update_Gird(dataGridView6, sqlConnection, $"SELECT * FROM Units WHERE Unit_name LIKE N'%{textBox33.Text}%'");
        }

        private void textBox43_TextChanged(object sender, EventArgs e)
        {
            update_Gird(dataGridView6, sqlConnection, $"SELECT * FROM Units WHERE Unit_name LIKE N'%{textBox43.Text}%'");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox43.Text != null)
            {
                SqlCommand command = new SqlCommand(
                        $"SELECT COUNT(*) FROM Units WHERE Unit_name = N'{textBox43.Text}'",
                        sqlConnection);
                if (Int32.Parse(command.ExecuteScalar().ToString()) != 0)
                {
                    command = new SqlCommand(
                            $"UPDATE Units SET Quantity = Quantity + {numericUpDown2.Value} WHERE Unit_name = N'{textBox43.Text}'",
                            sqlConnection);

                    command.ExecuteNonQuery();
                    update_Gird(dataGridView6, sqlConnection, $"SELECT * FROM Units WHERE Unit_name LIKE N'%{textBox43.Text}%'");
                }
                else
                {
                    MessageBox.Show("Не верное наименование");
                }
            }
            else
            {
                MessageBox.Show("Не указано наименование");
            }

        }

        private void textBox30_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                GetSellCount();

                SqlCommand command = new SqlCommand(
                    $"SELECT COUNT(*) FROM Costumers WHERE Surname = N'{textBox2.Text}' OR Telnum = N'{textBox2.Text}' OR Email = N'{textBox2.Text}'",
                    sqlConnection);

                int count = Int32.Parse(command.ExecuteScalar().ToString());

                if (count > 0)
                {
                    command = new SqlCommand(
                            $"SELECT ID FROM Costumers WHERE Surname = {textBox2.Text} OR Telnum = {textBox2.Text} OR Email = {textBox2.Text}",
                            sqlConnection);
                    int CostID = Int32.Parse(command.ExecuteScalar().ToString());
                   
                        command = new SqlCommand(
                            $"UPDATE SellChek SET Costumer_Id = {CostID} WHERE Sell_numb = {sellsCount_}",
                            sqlConnection);
                    command.ExecuteNonQuery();
                }
            }
            GetSellCount();
            update_Gird(dataGridView4, sqlConnection, $"SELECT Unit_name,sc.Quantity, SELL_Price FROM Units u, SellChek sc WHERE sc.Unit_Id = -1");
            textBox3.Text = "0";
            update_Gird(dataGridView3, sqlConnection, $"SELECT Unit_name, SELL_Price, Quantity FROM Units WHERE ID = -1"); 
        }

        private void GetSellCount()
        {
            SqlCommand command = new SqlCommand(
                    $"SELECT TOP(1) SellChek.Sell_numb FROM SellChek ORDER BY ~Sell_numb",
                    sqlConnection);
            sellsCount_ = Int32.Parse(command.ExecuteScalar().ToString());
        }

        private void button12_Click(object sender, EventArgs e)
        {

            if (textBox51.Text != null && textBox48.Text != null && textBox47.Text != null)
            {
                SqlCommand command1 = new SqlCommand(
                $"SELECT COUNT(*) FROM Costumers WHERE Telnum = N'{textBox47.Text}'",
                sqlConnection);

                if (Int32.Parse(command1.ExecuteScalar().ToString()) == 0)
                {

                    if(textBox5.Text == "" )
                        textBox5.Text = "none";


                    SqlCommand command = new SqlCommand(
                    $"INSERT INTO [Costumers] (Name, Surname, Telnum, Email) VALUES(N'{textBox51.Text}', N'{textBox48.Text}', N'{textBox29.Text}', N'{textBox5.Text}')",
                    sqlConnection);

                    Login.TabPages.RemoveAt(0);
                    Login.TabPages.Add(tabPage3);
                    command.ExecuteNonQuery();
                    update_Gird(dataGridView2, sqlConnection);
                    clearAlltextBoxes();
                    MessageBox.Show("Успешная Регистрация");
                    textBox48.Text = "";
                    textBox51.Text = "";
                    textBox47.Text = "";
                    textBox5.Text = "";
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            update_Gird(dataGridView5, sqlConnection, $"SELECT Surname, Telnum, Email FROM Costumers WHERE Surname LIKE N'%{textBox2.Text}%' OR Telnum LIKE N'%{textBox2.Text}%' OR Email LIKE N'%{textBox2.Text}%'");
        }

        private string splitDatetoSql(string A)
        {
            string T = A;
            string[] pats = T.Split();
            string[] a = pats[0].Split('.');
            return (a[1] + '/' + a[0] + '/' + a[2]);
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            update_Gird(dataGridView7, sqlConnection, $"SELECT s.Name AS 'Имя', s.Surname AS 'Фамилия', SUM(s.Commision*sc.Quantity*(u.SELL_Price-u.BUY_Price)) AS 'Зарплата' FROM Sellers s JOIN SellChek sc ON s.ID = sc.Seller_Id JOIN Units u	ON sc.Unit_Id = u.ID WHERE DATEPART(m, sc.date) = DATEPART(m, N'{splitDatetoSql(dateTimePicker2.Value.ToString())}') AND DATEPART(yy, sc.date) = DATEPART(yy, N'{splitDatetoSql(dateTimePicker2.Value.ToString())}') GROUP BY s.Name,s.Surname");

        }

        private void button11_Click(object sender, EventArgs e)
        {
            update_Gird(dataGridView10, sqlConnection, "SELECT * FROM( SELECT  u.Unit_name AS 'Наименование', SUM(sc.Quantity * (u.SELL_Price - u.BUY_Price)) AS 'Доход' FROM Units u, SellChek sc WHERE sc.Unit_Id = u.ID GROUP BY u.Unit_name) b");
            update_Gird(dataGridView11, sqlConnection, "SELECT u.Unit_name AS 'Наименование', u.Quantity AS 'Количество'  FROM Units u WHERE u.Quantity < 100");
            update_Gird(dataGridView8, sqlConnection, "SELECT sc.Date, SUM(sc.Quantity*(u.SELL_Price-u.BUY_Price)) AS 'Доход' FROM SellChek sc LEFT JOIN Units u ON sc.Unit_Id =u.ID	GROUP BY sc.Date");
            update_Gird(dataGridView9, sqlConnection, "SELECT DATENAME(m, sc.Date)+DATENAME(yy, sc.Date) , SUM(sc.Quantity * (u.SELL_Price - u.BUY_Price)) AS 'Доход' FROM SellChek sc LEFT JOIN Units u ON sc.Unit_Id = u.ID GROUP BY  DATENAME(m, sc.Date) + DATENAME(yy, sc.Date)");
            update_Gird(dataGridView7, sqlConnection, $"SELECT s.Name AS 'Имя', s.Surname AS 'Фамилия', SUM(s.Commision*sc.Quantity*(u.SELL_Price-u.BUY_Price)) AS 'Зарплата' FROM Sellers s JOIN SellChek sc ON s.ID = sc.Seller_Id JOIN Units u	ON sc.Unit_Id = u.ID WHERE DATEPART(m, sc.date) = DATEPART(m, N'{splitDatetoSql(dateTimePicker2.Value.ToString())}') AND DATEPART(yy, sc.date) = DATEPART(yy, N'{splitDatetoSql(dateTimePicker2.Value.ToString())}') GROUP BY s.Name,s.Surname");
            
            SqlCommand command = new SqlCommand(
            $"SELECT TOP(1) s.Surname AS 'имя', SUM(s.Commision * sc.Quantity * (u.SELL_Price - u.BUY_Price)) AS 'Зарплата' FROM Sellers s JOIN SellChek sc ON s.ID = sc.Seller_Id JOIN Units u ON sc.Unit_Id = u.ID GROUP BY s.Name,s.Surname",
            sqlConnection);
            textBox53.Text = command.ExecuteScalar().ToString();

            command = new SqlCommand(
            $"SELECT TOP(1) DATENAME(m, sc.Date)+DATENAME(yy, sc.Date) , SUM(sc.Quantity * (u.SELL_Price - u.BUY_Price)) AS 'Доход' FROM SellChek sc LEFT JOIN Units u ON sc.Unit_Id = u.ID GROUP BY  DATENAME(m, sc.Date) + DATENAME(yy, sc.Date)",
            sqlConnection);
            textBox56.Text = command.ExecuteScalar().ToString();

            command = new SqlCommand(
            $"SELECT TOP(1) sc.Date, SUM(sc.Quantity*(u.SELL_Price-u.BUY_Price)) AS 'Доход' FROM SellChek sc LEFT JOIN Units u ON sc.Unit_Id =u.ID	GROUP BY sc.Date  ORDER BY ~SUM(sc.Quantity*(u.SELL_Price-u.BUY_Price))",
            sqlConnection);
            textBox54.Text = command.ExecuteScalar().ToString();
        }
    }
}

