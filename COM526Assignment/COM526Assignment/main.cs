using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COM526Assignment
{
    public partial class main : Form
    {
        string[] descriptions = { "Task 1 - Leads to a form where you will download a PDF file containing the documentation for Task 2 and Task 3.", "Task 2 - Encrypts or decrypts messages by shifting the alphabet by -25 to 25 characters, depending on what is set in the shift by field. Allows to choose where to save an encrypted message or what file to open and decrypt a message from. The encryption/decryption is easily achieved by converting mesages' characters to their ASCII codes and performing checks on the arrays containing them.", "Task 3 - Checks if a keyword for today is set in the remote database, if not then a new one must be set, otherwise a login screen appears. Setting a new keyword is achieved by loading poems from the poems table located on the remote database, then splitting them accordingly in the combobox and listbox fields, from which user can choose which word to use. Additional poems can also be added, from the Add Poems form. After setting the new keyword, every agent on the agents table will be emailed the generated number set and the date it is valid for. When logging in, keyword and agent initials must match data stored in the database. There are options to delete the keyword of the day or to add a new agent (initials + email required). Once logged in, messages can be encrypted/decrypted with the daily keyword and each operation is recorded on the remote database. The combobox allows to load old encryptions/decryptions and to perform new encryptions/decryptions using the old loaded keyword. There are also options for saving an encrypted message or opening and decrypting one.", "Task 4 - Leads to a form where a PDF file containing the evaluation of my solution/design will be downloaded."};

        string fileName = "";

        globalToolBox n = new globalToolBox();

        public main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {   
            description.Text = descriptions[0];
        }

        private void button0_Click(object sender, EventArgs e)
        {
            fileName = "task1";
            loadTask();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            loadTask(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadTask(2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            fileName = "task4";
            loadTask();
        }

        public void loadTask(int n = 0)
        {           
            Form[] task = { new task1(fileName), new task2(), new task3_login() };
            this.Hide();
            task[n].ShowDialog();
            this.Show();
        }

        //hover handlers
        private void button0_MouseEnter(object sender, EventArgs e)
        {
            description.Text = descriptions[0];
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            description.Text = descriptions[1];
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            description.Text = descriptions[2];
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            description.Text = descriptions[3];
        }
    }
}
