using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net.Mail;
using System.Xml;

namespace COM526Assignment
{
    public partial class task3_login : Form
    {

        globalToolBox global = new globalToolBox();
        bool registrationProcess = false;
        string[] encodedDate = { " ", " ", " " };

        public task3_login()
        {
            InitializeComponent();
        }

        private void task3_login_Load(object sender, EventArgs e)
        {
            if (!updatePoemList())
            {
                this.Close(); //no internet connection/database inaccessible
            }
            else
            {
                string query = DateTime.Now.ToString("yyyy-MM-dd");
                if (global.connectToDatabase("checkKeyword", query) == "exists")
                {
                    //keyword exists so proceed to login screen
                    panel1.Visible = true;
                    resetWindow(180);
                }
                else
                {
                    //keyword needs to be created since it does not exist
                    MessageBox.Show("Fetching poem list from database, please wait a while!");
                    date.Text = "No keyword has been set for today's date (" + DateTime.Now.ToString("yyyy-MM-dd") + ")";
                    panel2.Visible = true;
                    resetWindow(545);
                }
            }
        }
        
        public bool updatePoemList()
        {
            string poemListStirng = global.connectToDatabase("poems");
            if(poemListStirng != null)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(poemListStirng);
                XmlNodeList elem = doc.GetElementsByTagName("poem");
                comboBox1.Items.Clear();
                for (int k = 0; k < elem.Count; k++)
                {
                    comboBox1.Items.Add(elem.Item(k).SelectSingleNode("id").InnerText + " - " + elem.Item(k).SelectSingleNode("title").InnerText);
                    global.poemsList.Add(elem.Item(k).SelectSingleNode("body").InnerText);
                }
                return true;
            }
            return false;
        }

        public void sendEmail(string emailBody, string emailSubject, string emailTo, string easyToFindOutSecretButIAmTrackingYourIpIfYouTry)
        {//using net.mail
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("kaloyan@bozhkov.com");
                mail.To.Add(emailTo);
                mail.Subject = emailSubject;
                mail.IsBodyHtml = true;
                mail.Body = emailBody;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("kaloyan@bozhkov.com", easyToFindOutSecretButIAmTrackingYourIpIfYouTry);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            encodedDate[0] = " ";
            encodedDate[1] = " ";
            encodedDate[2] = " ";
            listBox2.Items.Clear();
            listBox2.Items.Add("No line of a poem selected yet.");
            listBox1.Items.Clear();
            if (comboBox1.SelectedIndex > -1)
            {

                listBox1.Enabled = true;
                string[] loadPoemLines = global.poemsList[comboBox1.SelectedIndex].Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                for (int k = 0; k < loadPoemLines.Length; k++)
                    listBox1.Items.Add(loadPoemLines[k]);

                encodedDate[0] = ((int)(comboBox1.SelectedIndex) + 1).ToString();
                setZeroInEncodedDate(0);
            }
            else
            {
                listBox2.Enabled = false;
                listBox2.Enabled = false;
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.Enabled == true && listBox2.SelectedIndex > -1)
            {
                button1.Enabled = true;
                encodedDate[2] = ((int)(listBox2.SelectedIndex) + 1).ToString();
                setZeroInEncodedDate(2);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (listBox1.SelectedIndex > -1)
            {
                if (listBox1.SelectedIndex == 0 && listBox1.Items[0].ToString() == "No poem selected yet.")
                {
                    listBox1.Enabled = false;
                }
                else
                {
                    listBox2.Items.Clear();
                    listBox1.Enabled = true;
                    listBox2.Enabled = true;
                    string[] words = global.regexA.Replace(listBox1.Items[listBox1.SelectedIndex].ToString().ToLower(), "").Split(' ');
                    for (int k = 0; k < words.Length; k++)
                    {
                        listBox2.Items.Add(words[k]);
                    }
                    encodedDate[1] = ((int)(listBox1.SelectedIndex) + 1).ToString();
                    setZeroInEncodedDate(1);
                }
            }
            else
            {
                listBox1.Enabled = false;
                listBox2.Enabled = false;
                listBox2.Items.Clear();
                listBox2.Items.Add("No line of a poem selected yet.");
            }
        }

        public void setZeroInEncodedDate(int n)
        {
            if (encodedDate[n].Length == 1)
                encodedDate[n] = "0" + encodedDate[n];

            generatedNumberSet.Text = "Generated number set: " + encodedDate[0] + "." + encodedDate[1] + "." + encodedDate[2];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string generatedNumberSetValue = encodedDate[0] + "." + encodedDate[1] + "." + encodedDate[2];
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string query = listBox2.Items[listBox2.SelectedIndex] +"|"+ date + "|"+ generatedNumberSetValue;
            string result = global.connectToDatabase("checkKeyword", query, "write");
            if(result == "success")
            {
                MessageBox.Show("Keyword of the day has successfully been set, please wait while sending emails to agents.");
                string response = global.connectToDatabase("mailTo");
                if (response != "no")
                {
                    string subject = "Get your freshly generated number set!";
                    string body = "";
                    string[] agentInfo;
                    string[] agents = response.Split('+');
                    string htmlMessage = agents[agents.Length - 1];
                    for (int k = 0; k < agents.Length - 2; k++)
                    {
                        agentInfo = agents[k].Split('?');
                        body = htmlMessage.Replace("XYZJ", global.formatInitials(agentInfo[0])).Replace("GNSJZW", generatedNumberSetValue).Replace("fkdate", date);
                        sendEmail(body, subject, agentInfo[1], agents[agents.Length - 2]);
                    }
                    MessageBox.Show("Emails have been sent containing the Generated Number Set and expiration date.");
                }           
                panel2.Visible = false;
                panel1.Visible = true;
                resetWindow(180);
            }
            else
            {
                MessageBox.Show("There has been an issue setting the keyword of the day.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            task3_poems_edit poemEdit = new task3_poems_edit();
            this.Hide();
            poemEdit.ShowDialog();
            if (poemEdit.successfull)
            {
                MessageBox.Show("A new poem has been added, please wait for the refresh to finish.");
                updatePoemList();
            }

            this.Show();
            poemEdit.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string query = DateTime.Now.ToString("yyyy-MM-dd");
            if (global.connectToDatabase("checkKeyword", query, "delete") == "success")
            {
                MessageBox.Show("The keyword of the day was successfully deleted.");
                panel2.Visible = true;
                panel1.Visible = false;
                resetWindow(545);
            }
            else
            {
                MessageBox.Show("There was an issue deleting the keyword of the day.");
            }
        }

        public void resetWindow(int height)
        {
            this.Size = new Size(683, height);
            this.CenterToScreen();
            if(height > 200)
                comboBox1.SelectedIndex = -1;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            enableBtn();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            enableBtn();
        }

        public void enableBtn()
        {
            if (textBox1.Text.Length > 0 && textBox2.Text.Length > 0 && !registrationProcess)
                button4.Enabled = true;
            else if(!registrationProcess)
                button4.Enabled = false;
            else if (registrationProcess && textBox1.Text.Length > 0 && textBox2.Text.Length > 0 && validateEmail(textBox2.Text))
                button4.Enabled = true;
            else
                button4.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string initials = textBox1.Text.Replace(".", "").Replace(" ", "").Replace(",", "").ToLower();
            string keyword = textBox2.Text.ToLower();
            if (!registrationProcess)
            {
                string query = initials + ";" + keyword + ";" + DateTime.Now.ToString("yyyy-MM-dd");
                string output = global.connectToDatabase("agentLoginSet", query);
                if (output != "none")
                {
                    MessageBox.Show("Logged in!");
                    task3 x = new task3();
                    x.creds = new credentials();
                    x.creds.password = keyword;
                    x.creds.initials = initials;
                    x.creds.generatedNumberSet = output;
                    x.global = new globalToolBox();
                    x.global.poemsList = global.poemsList;
                    this.Hide();
                    x.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Wrong Initials Or Keyword!");
                }
            }
            else
            {
                if(global.connectToDatabase("agentLoginSet", initials+";"+keyword, "write") == "done"){
                    MessageBox.Show("Agent was successfully registered!");
                    resetSettings();
                }
                else
                {
                    MessageBox.Show("Agent could not be registered at the moment, make sure the email is not already in use.");
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!registrationProcess)
            {
                registrationProcess = true;
                button4.Text = "Register Credentials!";
                button5.Text = "Cancel";
                label5.Text = "Email:";
            }
            else
            {
                resetSettings();
            }
            enableBtn();
        }

        public void resetSettings()
        {
            registrationProcess = false;
            button4.Text = "Sign In";
            button5.Text = "Add New Agent Initials";
            label5.Text = "Keyword:";
            textBox2.Text = "";
        }

        public bool validateEmail(string email)
        {
            if (email.Length > 0 && email.Contains("@") && email.Split('@')[0].Length > 0 && email.Split('@')[1].Length > 0 && email.Split('@')[1].Contains(".") && email.Split('@')[1].Split('.')[0].Length > 0 && email.Split('@')[1].Split('.')[1].Length > 0)
                return true;

            return false;
        }
    }
}
