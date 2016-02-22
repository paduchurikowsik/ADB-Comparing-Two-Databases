using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DatabaseProjectFinal
{
    public partial class Form1 : Form
    {
        String connString = "Server=localhost;Uid=root;password=kowsik;";
        public MySqlConnection conn;

        String DB1Name;
        String DB2Name;

        List<String> DB1Tables;
        List<String> DB2Tables;

        List<Tuple<String, String, String>> DB1Total;
        List<Tuple<String, String, String>> DB2Total;

        bool checkSchema = false;

        int DB1TotalNum = 0;
        int DB2TotalNum= 0;
        public Form1()
        {
            InitializeComponent();
           

        }

        private void button1_Click(object sender, EventArgs e)
        {

            
            conn = new MySqlConnection(connString);
            try
            {
                conn.Open();
            }
            catch(Exception ex)
            {
                label1.Text = ex.Message;
                label1.ForeColor = Color.Red;
            }
            if(conn.State == ConnectionState.Open)
            {
                label1.Text = "Connection Established";
                label1.ForeColor = Color.Green;
                MySqlCommand cmd = new MySqlCommand("show databases", conn);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine(reader[0].ToString());
                    comboBox1.Items.Add(reader[0].ToString());
                    comboBox2.Items.Add(reader[0].ToString());
                }
            }
            conn.Close();
            
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine(comboBox1.SelectedItem.ToString());

            DB1Name = comboBox1.Text;

            DB1Tables = new List<String>();


            conn = new MySqlConnection(connString);
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                label1.Text = ex.Message;
                label1.ForeColor = Color.Red;
            }
            if (conn.State == ConnectionState.Open)
            {
                label1.Text = "Connection Established";
                label1.ForeColor = Color.Green;
                MySqlCommand cmd = new MySqlCommand("show tables from "+ comboBox1.Text, conn);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine(reader[0].ToString());
                   DB1Tables.Add(reader[0].ToString());

                }
            }
           Console.WriteLine("DB1 " + DB1Tables.Count());
            conn.Close();


        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine(comboBox2.SelectedItem);

            DB2Name = comboBox2.Text;

            DB2Tables = new List<String>();

            conn = new MySqlConnection(connString);
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                label1.Text = ex.Message;
                label1.ForeColor = Color.Red;
            }
            if (conn.State == ConnectionState.Open)
            {
                label1.Text = "Connection Established";
                label1.ForeColor = Color.Green;
                MySqlCommand cmd = new MySqlCommand("SHOW TABLES from "+ comboBox2.Text, conn);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine(reader[0].ToString());
                    DB2Tables.Add(reader[0].ToString());

                }

            }
           Console.WriteLine("DB2 " + DB2Tables.Count());

            //Console.WriteLine("DB2 " + DB2Tables[0]);
            conn.Close();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadColumns();
            //int i = DB1Total.Count();
            //int j = DB2Total.Count();

            int totalCommon = 0;

            Console.WriteLine("DB1TOtal :" + DB1TotalNum);
            Console.WriteLine("DB2TOtal :" + DB2TotalNum);

            textBox1.Clear();

            textBox1.AppendText("\r\n *****************************************************************************************");

            textBox1.AppendText("\r\n Total columns in Database 1 :" + DB1TotalNum);
            textBox1.AppendText("\r\n Total columns in Database 2 :" + DB2TotalNum);

            textBox1.AppendText("\r\n *****************************************************************************************");

            for (int k = 0; k < DB1TotalNum; k++)
            {
                for (int l = 0; l < DB2TotalNum; l++)
                {
                    //Console.WriteLine("DB1TOtal Name :" + DB1Total[k].Item2);
                    //Console.WriteLine("DB2TOtal Name :" + DB2Total[l].Item2);
                    // && DB1Total[k].Item3 == DB2Total[l].Item3
                    if (DB1Total[k].Item2 == DB2Total[l].Item2)
                    {
                        //textBox1.Text += "\n" + DB1Total[k].Item1 + " => " + DB1Total[k].Item2 + " == " + DB2Total[l].Item1 + " => " + DB2Total[l].Item2;
                        if (checkSchema && DB1Total[k].Item3 == DB2Total[l].Item3)
                        {
                            textBox1.AppendText("\r\n { "+DB1Name+" }  " + DB1Total[k].Item1 + " => " + DB1Total[k].Item2 + " => " + DB1Total[k].Item3 + " ==    { " + DB2Name + " }  " + DB2Total[l].Item1 + " => " + DB2Total[l].Item2 + " => " + DB2Total[l].Item3);

                            Console.WriteLine(DB1Total[k].Item1 + " => " + DB1Total[k].Item2 + DB1Total[k].Item3 + " == " + DB2Total[l].Item1 + " => " + DB2Total[l].Item2 + " => " + DB2Total[l].Item3);

                            totalCommon++;

                            //connect to get the rows that are count of rows commone here


                            conn = new MySqlConnection(connString);
                            try
                            {
                                conn.Open();
                            }
                            catch (Exception ex)
                            {
                                label1.Text = ex.Message;
                                label1.ForeColor = Color.Red;
                            }
                            if (conn.State == ConnectionState.Open)
                            {
                                label1.Text = "Connection Established";
                                label1.ForeColor = Color.Green;
                                MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM " + DB1Name + "." + DB1Total[k].Item1 + ", " + DB2Name + "." + DB2Total[l].Item1 + " WHERE " + DB1Name + "." + DB1Total[k].Item1 + "." + DB1Total[k].Item2 + " = " + DB2Name + "." + DB2Total[l].Item1 + "." + DB2Total[l].Item2 + ";", conn);

                                MySqlDataReader reader = cmd.ExecuteReader();

                                while (reader.Read())
                                {
                                    Console.WriteLine("Common rows: " + reader[0].ToString());
                                    textBox1.AppendText("\r\n Total rows common are : " + reader[0].ToString());

                                }
                                conn.Close();
                            }

                        }

                        else if (checkSchema == false)
                        {
                            textBox1.AppendText("\r\n { " + DB1Name + " }  " + DB1Total[k].Item1 + " => " + DB1Total[k].Item2 + " => " + DB1Total[k].Item3 + " ==    { " + DB2Name + " }  " + DB2Total[l].Item1 + " => " + DB2Total[l].Item2 + " => " + DB2Total[l].Item3);

                            Console.WriteLine(DB1Total[k].Item1 + " => " + DB1Total[k].Item2 + DB1Total[k].Item3 + " == " + DB2Total[l].Item1 + " => " + DB2Total[l].Item2 + " => " + DB2Total[l].Item3);

                            totalCommon++;

                            //connect to get the rows that are count of rows commone here


                            conn = new MySqlConnection(connString);
                            try
                            {
                                conn.Open();
                            }
                            catch (Exception ex)
                            {
                                label1.Text = ex.Message;
                                label1.ForeColor = Color.Red;
                            }
                            if (conn.State == ConnectionState.Open)
                            {
                                label1.Text = "Connection Established";
                                label1.ForeColor = Color.Green;
                                MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM " + DB1Name + "." + DB1Total[k].Item1 + ", " + DB2Name + "." + DB2Total[l].Item1 + " WHERE " + DB1Name + "." + DB1Total[k].Item1 + "." + DB1Total[k].Item2 + " = " + DB2Name + "." + DB2Total[l].Item1 + "." + DB2Total[l].Item2 + ";", conn);

                                MySqlDataReader reader = cmd.ExecuteReader();

                                while (reader.Read())
                                {
                                    Console.WriteLine("Common rows: " + reader[0].ToString());
                                    textBox1.AppendText("\r\n Total rows common are : " + reader[0].ToString());

                                }
                                conn.Close();
                            }
                        }

                        conn.Close();

                    }
                }

                
            }
            textBox1.AppendText("\r\n *****************************************************************************************");
            textBox1.AppendText("\r\n Total commom columns are " + totalCommon);
            textBox1.AppendText("\r\n *****************************************************************************************");


        }

        private void LoadColumns()
        {
            int i = DB1Tables.Count();
            int j = DB2Tables.Count();

            DB1TotalNum = 0;
            DB2TotalNum = 0;

            DB1Total = new List<Tuple<String, String, String>>();
            DB2Total = new List<Tuple<String, String, String>>();

            conn = new MySqlConnection(connString);

            for (int k = 0; k < i; k++)
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    //label1.Text = ex.Message;
                    //label1.ForeColor = Color.Red;
                }
                if (conn.State == ConnectionState.Open)
                {

                    //label1.Text = "Connection Established";
                    //label1.ForeColor = Color.Green;

                    Console.WriteLine(DB1Tables[k]);

                    MySqlCommand cmd = new MySqlCommand("show columns from " + DB1Tables[k] + " in " + DB1Name, conn);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine(reader[0].ToString());
                        

                        String[] substrings = reader[1].ToString().Split('(');
                        Console.WriteLine(substrings[0]); 

                        DB1Total.Add(new Tuple<String, String, String>(DB1Tables[k], reader[0].ToString(), substrings[0]));

                        DB1TotalNum++;

                    }

                    //Console.WriteLine("DB1 Name "+DB1Total[k].Item1);
                    //Console.WriteLine("DB1 column " + DB1Total[k].Item2);
                }
                conn.Close();
            }

                for (int l = 0; l < j; l++)
                {
                    try
                    {
                        conn.Open();
                    }
                    catch (Exception ex)
                    {
                        //label1.Text = ex.Message;
                        //label1.ForeColor = Color.Red;
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        
                        //label1.Text = "Connection Established";
                        //label1.ForeColor = Color.Green;

                        Console.WriteLine(DB2Tables[l]);

                        MySqlCommand cmd = new MySqlCommand("show columns from " + DB2Tables[l] + " in " + DB2Name, conn);

                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Console.WriteLine(reader[0].ToString());
                        String[] substrings = reader[1].ToString().Split('(');
                        Console.WriteLine(substrings[0]);
                        DB2Total.Add(new Tuple<String, String, String>(DB2Tables[l], reader[0].ToString(), substrings[0]));

                            DB2TotalNum++;

                        }

                        //Console.WriteLine("DB1 Name " + DB2Total[l].Item1);
                        //Console.WriteLine("DB1 column " + DB2Total[l].Item2);
                    }
                    conn.Close();
                }

                
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                checkSchema = true;

                Console.WriteLine("Bool Changed: " + checkSchema.ToString());
            }
            else
            {
                checkSchema = false;
                Console.WriteLine("Bool Changed: " + checkSchema.ToString());
            }
        }
    }
}
