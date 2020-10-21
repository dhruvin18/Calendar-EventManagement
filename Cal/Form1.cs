using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cal
{
    public partial class Form1 : Form
    {
        public int id;
        MySqlConnection conn;
        public string connectionString;
        public Form1()
        {
            InitializeComponent();
            textBox2.PasswordChar = '*';
            textBox4.PasswordChar = '*';
            textBox5.PasswordChar = '*';
            groupBox2.Visible = false;
            groupBox2.Controls.Remove(groupBox1);
            connectionString = "data source=127.0.0.1;port=3306;username=root;password=;database=calendar";
            conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT COUNT(User_Id) FROM user", conn);
                cmd.Dispose();
                id = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                id = id + 1;
                //string t=r.GetString(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {
            label5.ForeColor =Color.BlueViolet;
            groupBox2.Visible = true;
            groupBox1.Visible = false;
        }

        

        private void button2_Click_1(object sender, EventArgs e)
        {
            string ErrorMessage;

            if (string.IsNullOrWhiteSpace(textBox3.Text)){
                MessageBox.Show("USERNAME CANNOT BE EMPTY", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (textBox4.Text.Equals(textBox5.Text))
                {
                    if (ValidatePassword(textBox4.Text, out ErrorMessage))
                    {
                        MySqlCommand cmd = new MySqlCommand("Insert into user VALUES(" + id + ",'" + textBox3.Text + "','" + textBox4.Text + "')", conn);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        id++;
                        groupBox2.Visible = false;
                        groupBox1.Visible = true;
                        textBox3.Clear();
                        textBox4.Clear();
                        textBox5.Clear();
                    }
                    else
                    {
                        MessageBox.Show(ErrorMessage, "Validate Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox4.Clear();
                        textBox5.Clear();
                    }

                }
                else
                {
                    textBox4.Clear();
                    textBox5.Clear();
                    MessageBox.Show("ENTER THE SAME PASSWORD", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private bool ValidatePassword(string password, out string ErrorMessage)
        {
            var input = password;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                //ErrorMessage = "Password cannot be empty";
                //return false;
                throw new Exception("Password should not be empty");                
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,15}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasLowerChar.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one lower case letter";
                return false;
            }
            else if (!hasUpperChar.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one upper case letter";
                return false;
            }
            else if (!hasMiniMaxChars.IsMatch(input))
            {
                ErrorMessage = "Password Should have Minimum 8 characters and Maximum 15 characters";
                return false;
            }
            else if (!hasNumber.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one numeric value";
                return false;
            }

            else if (!hasSymbols.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one special case characters";
                return false;
            }
            else
            {
                return true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection conn1 = new MySqlConnection(connectionString);
            string username = textBox1.Text;
            MySqlCommand cmd = new MySqlCommand("Select * from user where User_username='" + username + "'", conn);
            MySqlDataReader r = cmd.ExecuteReader();
            if (r.HasRows)
            {
                if (r.Read())
                {
                    if (r.GetString(2).Equals(textBox2.Text))
                    {
                        conn1.Open();
                        int temp = Convert.ToInt32(r.GetString(0));
                        MySqlCommand cmd1 = new MySqlCommand("Insert into session values(" + temp + ")",conn1);
                        cmd1.ExecuteNonQuery();
                        cmd1.Dispose();
                        conn1.Close();
                        r.Close();
                        r.Dispose();
                        cmd.Dispose();
                        MessageBox.Show("LOGIN SUCCESSFUL");
                        
                        this.Hide();
                        var form2 = new Form2();
                        form2.Closed += (s, args) => this.Close();
                        form2.Show();

                    }
                    else
                    {
                        r.Close();
                        r.Dispose();
                        cmd.Dispose();
                        MessageBox.Show("Enter The correct Username/Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                textBox1.Clear();
                textBox2.Clear();
                r.Close();
                r.Dispose();
                cmd.Dispose();
                MessageBox.Show("Enter The correct Username/Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
            groupBox1.Visible = true;
        }
    }
}
