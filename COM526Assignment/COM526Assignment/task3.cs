using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace COM526Assignment
{
    public partial class task3 : Form
    {
        bool fromSave = false;

        public credentials creds;
        //List<string> oldMessages = new List<string>();
        public globalToolBox global;
        bool resetProcess = false;

        public task3()
        {
            InitializeComponent();
        }

        public void loadOldMessages(string messagesQuerySettings = "")
        {
            string amount = "0";
            string response = global.connectToDatabase("messages", messagesQuerySettings);
            comboBox2.Items.Clear();
            if (response != "none")
            {
                string[] messages = response.Split('|');
                amount = messages.Length.ToString();
                string[] tmp = null;
                int maxWords, id;
                string optionalDots;
                oldMessages.messages.Clear();
                singleMessage msg;
                foreach (string singleMessageDetails in messages)
                {
                    tmp = singleMessageDetails.Split(';');
                    msg = new singleMessage();
                    id = 0;
                    int.TryParse(tmp[0], out id);
                    msg.id = id;
                    msg.agentInitials = global.formatInitials(tmp[5]);
                    msg.date = tmp[1].Replace("-", "/");
                    msg.time = tmp[2];
                    msg.generatedNumberSet = tmp[4];
                    msg.operationType = tmp[6];
                    msg.message = tmp[3];
                    maxWords = msg.message.Length;
                    optionalDots = "";
                    if (msg.message.Length > 35)
                    {
                        maxWords = 35;
                        optionalDots = "..";
                    }
                    oldMessages.messages.Add(msg);
                    comboBox2.Items.Add(msg.id + " - " + msg.agentInitials + " - " + oldMessages.getDateTime(oldMessages.messages.Count-1) + " - " + msg.generatedNumberSet + " - " + msg.operationType+ " - " + msg.message.Substring(0, maxWords) + optionalDots);
                }
                label2.Text = "Select an old message to load, currently listing " + amount + ":";
            }
            else
            {
                string output = (messagesQuerySettings == "") ? "No messages saved yet." : "No messages found for the current search criteria.";
                comboBox2.Items.Add(output);
            }
            label2.Text = "Select an old message to load, currently listing " + amount + ":";
        }

        private void task3_Load(object sender, EventArgs e)
        {
            //code load messages in combobox
            loadOldMessages();
            global.generateAlphabets(creds.password);
            label1.Text = global.formatInitials(creds.initials) + " - Keyword: " + creds.password;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = -1;
            if (fromSave == true)
            {
                if(MessageBox.Show("Do you want to continue using the saved keyword?", "Continue with old keyword?", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    fromSave = false;
                    global.generateAlphabets(creds.password);
                    button1.Text = (richTextBox2.Text.Length > 0) ? "Encrypt!" : "Decrypt!";
                    comboBox2.SelectedIndex = -1;
                }
            }

            string message = "";
            string output = "";
            if (richTextBox1.Text.Length > 0)
            {
                //encrypt
                message = richTextBox1.Text;
                output = encrypt(message);
                MessageBox.Show("Encryption complete! - Clearing Encryption Text Box and Pasting in Decryption Text Box.");
                button2.Enabled = true;
                richTextBox1.Text = "";
                richTextBox2.Text = output;
            }
            else if (richTextBox2.Text.Length > 0)
            {
                //decrypt
                message = richTextBox2.Text;
                output = decrypt(message);
                MessageBox.Show("Decryption complete! - Clearing Dencryption Text Box and Pasting in Encryption Text Box.");               
                richTextBox2.Text = "";
                richTextBox1.Text = output;
            }

            updateDatabase(output);
        }

        public string encrypt(string message)
        {
            int alphabetCount = 0;
            int alphabetCountMax = global.generatedAlphabets.Count;
            string output = "";
            foreach (char c in message)
            {
                if (c != '?' && c != '!' && c != ',' && c != '.' && c != ' ' && (int)c != 39)
                {
                    if (alphabetCount >= alphabetCountMax)
                        alphabetCount = 0;

                    output += ((char)global.generatedAlphabets[alphabetCount][(25 - (global.max - (int)c))]).ToString();
                    alphabetCount++;
                }
                else
                {
                    output += c.ToString();
                }
            }
            return output;
        }

        public string decrypt(string message)
        {
            int alphabetCount = 0;
            int alphabetCountMax = global.generatedAlphabets.Count;
            string output = "";
            foreach (char c in message)
            {
                if (c != '?' && c != '!' && c != ',' && c != '.' && c != ' ' && (int)c != 39)
                {
                    if (alphabetCount >= alphabetCountMax)
                        alphabetCount = 0;

                    output += ((char)(global.min + Array.IndexOf(global.generatedAlphabets[alphabetCount], (int)c))).ToString();
                    alphabetCount++;
                }
                else
                {
                    output += c;
                }
            }
             return output;
        }

        public string getWordFromPoemList(string code)
        {            
            string[] info = code.Split('.');
            int poem = 0; int line = 0; int wordC = 0;
            if (int.TryParse(info[0], out poem) && int.TryParse(info[1], out line) && int.TryParse(info[2], out wordC)) {
            string[] lines = global.poemsList[poem - 1].Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string[] words = global.regexA.Replace(lines[line - 1].ToLower(), "").Split(' ');
                return words[wordC - 1];
            }
            return "";
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = global.regex.Replace(richTextBox1.Text.ToLower(), "");
            if (!fromSave)
                button1.Text = "Encrypt!";
            else
                button1.Text = "Encrypt" + button1.Text.Substring(7);

            if (richTextBox1.Text.Length > 0)
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox2.Text = "";

            enableButton();
        }
        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            richTextBox2.Text = global.regex.Replace(richTextBox2.Text.ToLower(), "");
            if(!fromSave)
               button1.Text = "Decrypt!";
            else
               button1.Text = "Decrypt" + button1.Text.Substring(7);

            if (richTextBox2.Text.Length > 0)
                richTextBox2.SelectionStart = richTextBox2.Text.Length;
            else
                button2.Enabled = false;
                richTextBox1.Text = "";

            enableButton();
        }
        public void enableButton()
        {
            button1.Enabled = (richTextBox1.Text.Length > 0 || richTextBox2.Text.Length > 0) ? true : false;
        }
        public void updateDatabase(string output)
        {
            string generatedNumberSet = creds.generatedNumberSet;
            if(fromSave == true)//if save file, set variable to old nset 
                generatedNumberSet = button1.Text.Split('\'')[1];

            string lastOperation = (richTextBox2.Text.Length > 0) ? "Decrypt" : "Encrypt";                             
            string query = DateTime.Now.ToString("yyyy-MM-dd") + "|" + DateTime.Now.ToString("HH:mm") + "|" + output + "|" + generatedNumberSet + "|" + creds.initials + "|" + lastOperation;
            if (global.connectToDatabase("messages", query, "write") != "added") {
                MessageBox.Show("Message could not be saved to database.");
            };
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string generatedNumber = creds.generatedNumberSet;
            if (fromSave)            
                generatedNumber = button1.Text.Split('\'')[1];
            
            if (global.writeToSeelectedFolder(generatedNumber + "|" + richTextBox2.Text, true))
            {
                richTextBox2.Text = "";
                comboBox2.SelectedIndex = -1;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";

            string[] content = global.loadEncryptedMessageFromFolder();
            if (content != null)
            {
                generateForOldMessages(content);
                richTextBox1.Text = decrypt(content[1]);
                comboBox2.SelectedIndex = -1;
            }
        }

        public void generateForOldMessages(string[] content) {
            fromSave = true;
            string word = getWordFromPoemList(content[0]);
            global.generateAlphabets(word);
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            button1.Text = button1.Text.Substring(0, 7) + " with - '" + content[0] + "' (" + word + ") - keyword";
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int n = comboBox2.SelectedIndex;
            if (n > -1 && comboBox2.Items[n].ToString().Length > 24)
            {
                string[] content = { oldMessages.messages[n].generatedNumberSet, oldMessages.messages[n].message };
                generateForOldMessages(content);
                if (oldMessages.messages[n].operationType.ToLower() == "encrypt")
                {
                    richTextBox1.Text = content[1];
                    button2.Enabled = false;
                }
                else
                {
                    richTextBox2.Text = content[1];
                    button2.Enabled = true;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to set new search filters?", "Oh wait a sec!", MessageBoxButtons.YesNo) != DialogResult.Yes) {
                MessageBox.Show("Please wait while new messages are fetched");
                loadOldMessages();
                MessageBox.Show("New messages successfully updated!");
            }
            else
            {
                panel1.Visible = true;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked && !resetProcess)
                checkboxChanged(radioButton1);

            resetProcess = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
                checkboxChanged(radioButton2);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
                checkboxChanged(radioButton3);
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
                checkboxChanged(radioButton4);
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
                checkboxChanged(radioButton5);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string query = "";
            if (radioButton2.Checked)
                query = "keyword:" + textBox1.Text.Trim().ToLower();
            else if (radioButton3.Checked)
                query = "date:" + dateTimePicker1.Value.ToString("yyyy-MM-dd").Trim();
            else if (radioButton4.Checked)
                query = "between:" +  dateTimePicker2.Value.ToString("yyyy-MM-dd").Trim() + "?" + dateTimePicker3.Value.ToString("yyyy-MM-dd").Trim();
            else if (radioButton5.Checked)
                query = "agent:"+ textBox1.Text.Trim().ToLower().Replace(".", "").Replace(".", "").Replace(" ", "");

            MessageBox.Show("Please wait while new messages are fetched");
            loadOldMessages(query);
            MessageBox.Show("New messages successfully updated!");
            panel1.Visible = false;
            resetRefresh();
        }

        public void checkboxChanged(Control sender)
        {
            foreach(Control rb in panel1.Controls)
            {
                if (rb is RadioButton && rb.Name != sender.Name)
                    rb.Visible = false;
            }            
            const int y = 45;
            int time = 1;
            for(int k = sender.Location.Y; k >= y; k--)
            {
                sender.Location = new System.Drawing.Point(sender.Location.X, k);
                time += Convert.ToInt32((time / k)*(time+500));
                Thread.Sleep(time);
            }
            button5.Enabled = true;
            if (sender.Name == "radioButton1" || sender.Name == "radioButton2" || sender.Name == "radioButton5")
            {
                button5.Enabled = false;
                textBox1.Visible = true;
            }else if (sender.Name == "radioButton3")
            {
                dateTimePicker1.Visible = true;
            }else if (sender.Name == "radioButton4")
            {
                dateTimePicker2.Visible = true;
                dateTimePicker3.Visible = true;
                label6.Visible = true;
                label7.Visible = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            resetRefresh();
        }

        public void resetRefresh()
        {
            panel1.Visible = false;
            resetProcess = true;
            radioButton1.Checked = true;
            int[] radioButtonPositions = { 63, 97, 132, 167, 202 };               
            foreach(Control rb in panel1.Controls)
                if (rb is RadioButton)
                {
                    rb.Visible = true;
                    rb.Location = new System.Drawing.Point(rb.Location.X, radioButtonPositions[int.Parse(rb.Name.Substring(rb.Name.Length-1))-1]);
                }

            textBox1.Visible = false;
            dateTimePicker2.Visible = false;
            dateTimePicker3.Visible = false;
            textBox1.Text = "";
            label6.Visible = false;
            label7.Visible = false;
            dateTimePicker1.Visible = false;
            button5.Enabled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button5.Enabled = (textBox1.Text.Length > 0) ? true : false;
        }
    }
}
