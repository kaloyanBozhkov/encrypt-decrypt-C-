using System;
using System.Windows.Forms;

namespace COM526Assignment
{
    public partial class task3_poems_edit : Form
    {
        public bool successfull = false;
        globalToolBox global = new globalToolBox();
        public task3_poems_edit()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string poem = richTextBox1.Text + "|" + richTextBox2.Text;
            if (global.connectToDatabase("poems", poem, "write") == "done")
            {
                MessageBox.Show("Poem was added successfully.");
                successfull = true;
                this.Hide();
            }
            else {
                MessageBox.Show("There was an issue adding the poem.");
            } 
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            enableBtn();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            enableBtn();
        }

        public void enableBtn()
        {
            if(richTextBox1.Text.Length > 0 && richTextBox2.Text.Length > 0)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        private void task3_poems_edit_Load(object sender, EventArgs e)
        {

        }
    }
}
