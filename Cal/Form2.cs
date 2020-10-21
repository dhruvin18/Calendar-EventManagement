using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cal
{
    public partial class Form2 : Form
    {
        public int User_ID = 0;
        public MySqlConnection conn2;
        public string connectionstring = "data source=127.0.0.1;port=3306;username=root;password=;database=calendar";
        public string datetime="";
        public Form2()
        {
            InitializeComponent();
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
            groupBox2.Visible = false;
            dataGridView1.Visible = false;
            groupBox3.Visible = false;
            
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            datetime = e.Start.ToString();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            conn2 = new MySqlConnection(connectionstring);
            conn2.Open();
            MySqlCommand cmd = new MySqlCommand("Select * from session", conn2);
            MySqlDataReader r = cmd.ExecuteReader();
            if (r.HasRows)
            {
                while (r.Read())
                {
                    User_ID=Convert.ToInt32( r.GetString(0));
                }
            }
            r.Close();
            cmd.Dispose();
            cmd = new MySqlCommand("Select User_username from user where User_Id="+User_ID, conn2);
            r = cmd.ExecuteReader();
            if (r.HasRows)
            {
                while (r.Read())
                {
                    label4.Text = r.GetString(0);
                }
            }
            r.Close();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
            monthCalendar1.Visible = false;
            if (datetime.Equals("") || string.IsNullOrEmpty(datetime))
            {
                MessageBox.Show("SELECT A DATE","",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                monthCalendar1.Visible = true;
                return;
            }
            
            groupBox2.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MySqlConnection conn3 = new MySqlConnection(connectionstring);
            conn3.Open();
            MySqlCommand cmd = new MySqlCommand("Insert into event values(" + User_ID + ",'"+datetime +"','"+textBox1.Text+"','"+richTextBox1.Text+"')",conn3);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            textBox1.Clear();
            richTextBox1.Clear();
            MessageBox.Show("Event Added", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            groupBox2.Visible = false;
            monthCalendar1.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
            monthCalendar1.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].Name = "Date";
            dataGridView1.Columns[1].Name = "EVENT";
            dataGridView1.Columns[2].Name = "DESCRIPTION";
            dataGridView1.Visible = true;
            MySqlConnection conn4 = new MySqlConnection(connectionstring);
            conn4.Open();
            MySqlCommand cmd = new MySqlCommand("Select * from event where User_Id=" + User_ID, conn4);
            MySqlDataReader r = cmd.ExecuteReader();
            if (r.HasRows)
            {
                while (r.Read())
                {
                    string[] t = r.GetString(1).Split(' ');
                    dataGridView1.Rows.Add(t[0], r.GetString(2), r.GetString(3));
                }
            }
            r.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            monthCalendar1.Visible = false;
            int rowselect = e.RowIndex;
            richTextBox2.Text = dataGridView1.Rows[rowselect].Cells[2].Value.ToString();
            groupBox3.Visible = true;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            monthCalendar1.Visible = true;
            groupBox3.Visible = false;
        }

        private void label5_Click(object sender, EventArgs e)
        {
            conn2.Close();
            var form1 = new Form1();
            form1.Show();
            this.Hide();
            form1.Closed += (s, args) => this.Close();
            
        }

        
    }
}
