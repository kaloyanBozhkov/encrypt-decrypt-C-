using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace COM526Assignment
{
    public partial class task2 : Form
    {

        globalToolBox global = new globalToolBox();

        public task2()
        {
            InitializeComponent();
        }
       
        public string encryptMessage(string message, bool encryptOrDecrypt = true)
        {
            int shiftBy = 0;
            int.TryParse(textBox1.Text, out shiftBy);
            string encrypted = "";
            int charCode = 0;
            char newChar = new char();
            int totalNew = 0;
            foreach (char c in message)
            {
                charCode = (int)c;
                totalNew = charCode;
                if(charCode != 32 && charCode != 39 && charCode != 33 && charCode != 63 && charCode != 46 && charCode != 44)
                {
                    totalNew = (encryptOrDecrypt) ? (charCode + shiftBy) : (charCode - shiftBy);
                    if (totalNew > global.max)
                        totalNew = totalNew - global.max + global.min - 1;

                    if (totalNew < global.min)
                        totalNew = global.max - global.min + totalNew + 1;
                }
                newChar = (char)totalNew;
                encrypted += newChar.ToString();
            }                
            return encrypted;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {            
            richTextBox1.Text = global.regex.Replace(richTextBox1.Text.ToLower(), "");
            richTextBox1.Text = richTextBox1.Text.ToLower();
            if (richTextBox1.Text.Length > 0)
            {
                richTextBox2.Text = encryptMessage(richTextBox1.Text);
                richTextBox4.Text = richTextBox2.Text;
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                button1.Enabled = true;
            }
            else
            {
                richTextBox4.Text = "";
                richTextBox2.Text = "";
                button1.Enabled = false;
            }
        }

        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {
            richTextBox4.Text = global.regex.Replace(richTextBox4.Text.ToLower(), "");
            if (richTextBox4.Text.Length > 0)
            {
                richTextBox3.Text = encryptMessage(richTextBox4.Text, false);
                richTextBox1.Text = richTextBox3.Text;
                richTextBox4.SelectionStart = richTextBox4.Text.Length;
            }
            else
            {
                richTextBox1.Text = "";
                richTextBox3.Text = "";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = checkInput(textBox1.Text, true);
            if (textBox1.Text.Length > 0)
            {
                textBox1.SelectionStart = textBox1.Text.Length;

                if(richTextBox1.Text.Length > 0)
                {
                    richTextBox2.Text = encryptMessage(richTextBox1.Text);
                    richTextBox4.Text = richTextBox2.Text;
                    richTextBox3.Text = encryptMessage(richTextBox4.Text, false);
                }
            }
            else
            {
                richTextBox2.Text = richTextBox1.Text;
                richTextBox4.Text = richTextBox3.Text;
            }
        }

        public static string checkInput(string x, bool canBeNegative)
        {
            string output = "";
            char y;
            for (var i = 0; i <= x.Length - 1; i++)
            {
                y = x[i];
                if (IsNumeric(y.ToString()))
                    output += y.ToString();
                else if (canBeNegative && y.ToString() == "-" && i == 0)
                    output += "-";
            }
            
            int testOutput = 0;
            if (int.TryParse(output, out testOutput))
            {
                if (testOutput > 25)
                    output = "25";
                else if (testOutput < -25)
                    output = "-25";
            };
            return output;
        }

        public static bool IsNumeric(string str)
        {
            double myNum = 0;
            if (Double.TryParse(str, out myNum))
                return true;

            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            global.writeToSeelectedFolder(textBox1.Text + "|" + richTextBox2.Text);
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            string[] content = global.loadEncryptedMessageFromFolder();
            if(content != null)
            {
                richTextBox1.Text = "";
                textBox1.Text = content[0];
                richTextBox4.Text = content[1];
            }
        }
    }
}
