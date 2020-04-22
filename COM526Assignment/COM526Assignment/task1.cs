using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace COM526Assignment
{
    public partial class task1 : Form
    {
        private string fileName;
        public task1(string name)
        {
            InitializeComponent();
            fileName = name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Content-Type", "application/octet-stream");
                webClient.Headers.Add("Accept", "text/html");
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "PDF files (*.pdf)|*.pdf";
                saveFile.FilterIndex = 1;
                saveFile.RestoreDirectory = true;
                saveFile.FileName = fileName + ".pdf";
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        webClient.DownloadFile("http://kaloyanbozhkov.com/glyndwr/files/" + fileName + ".pdf", Path.GetFullPath(saveFile.FileName));
                        MessageBox.Show("File was downloaded successfully!");
                        
                    }catch(Exception ex)
                    {
                        MessageBox.Show("There seems to have been an issue when connecting to the remote database! :(\n\nMESSAGE FOR SMART FOLKS:\n\n"+ex.ToString());
                    }
                    this.Close();
                }
            }
        }

        private void task1_Load(object sender, EventArgs e)
        {
            if (fileName == "task4")
                label1.Text = "Click the button below to download the PDF file \r\ncontaining my personal evaluation of my solution.";
        }
    }
}
