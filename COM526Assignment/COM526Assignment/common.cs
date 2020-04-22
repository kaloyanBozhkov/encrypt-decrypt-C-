using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Linq;
namespace COM526Assignment
{
    public class credentials
    {
        public string password;
        public string initials;
        public string generatedNumberSet;
    }

    public class singleMessage
    {
        public string message { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public int id { get; set; }
        public string agentInitials { get; set; }
        public string generatedNumberSet { get; set; }
        public string operationType { get; set; }
    }

    public static class oldMessages
    {
        public static List<singleMessage> messages = new List<singleMessage>();

        public static string getDateTime(int n)
        {
            return messages[n].date + " " + messages[n].time;
        }

    }

    public class globalToolBox
    {
        public List<string> poemsList = new List<string>();
        public Regex regexA = new Regex("[^a-z ]");
        public Regex regex = new Regex("[^a-z '!?.,]");
        public StreamWriter write;
        public StreamReader read;
        public int max = 122;
        public int min = 97;
        public List<int[]> generatedAlphabets = new List<int[]>();

        public string connectToDatabase(string pageName = "", string query = "", string operationType = "read")
        {
            var webClient = new WebClient();
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            webClient.Headers.Add("Accept", "text/xml");
            var parameters = new System.Collections.Specialized.NameValueCollection();
            parameters.Add("query", query);
            parameters.Add("operationType", operationType);

            try{
                byte[] bret = webClient.UploadValues("http://kaloyanbozhkov.com/glyndwr/" + pageName + ".php", "POST", parameters);
                return Encoding.UTF8.GetString(bret);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Oops! There seems to be a problem connecting to the database :(\n\nPlease check your internet connection and try again later.\n\nMESSAGE FOR SMART FOLKS:\n\n" + ex.ToString());
                return null;
            }
        }

        public string formatInitials(string initials)
        {
            return initials.Aggregate(string.Empty, (c, i) => c + i + ". ").ToUpper().Trim();
        }

        public void generateAlphabets(string word)
        {
            generatedAlphabets.Clear();
            //n of alphabets
            char[] alphabetStartLetter = word.ToCharArray();
            int startCharCode = 0;
            for (int k = 0; k < alphabetStartLetter.Length; k++)
            {
                startCharCode = (int)alphabetStartLetter[k];
                int[] alphabet = new int[26];
                int charCounter = startCharCode;
                for (int j = 0; j <= 25; j++)
                {
                    if (charCounter > max)
                        charCounter = min;

                    alphabet[j] = charCounter;
                    charCounter++;
                }
                generatedAlphabets.Add(alphabet);
            }
        }

        public bool writeToSeelectedFolder(string output, bool askForFileName = false)
        {
            string path = "";
            DialogResult result;
            DateTime time = DateTime.Now;
            string name = time.ToString("yyyyMMddhhmmss");
            if (!askForFileName)
            {
                FolderBrowserDialog FolderBrowser = new FolderBrowserDialog();
                FolderBrowser.Description = "Select which folder to save the encrypted message into.";
                result = FolderBrowser.ShowDialog();
                path = FolderBrowser.SelectedPath + "/" + name + ".txt";
            }
            else
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "txt files (*.txt)|*.txt";
                saveFile.FilterIndex = 1;
                saveFile.RestoreDirectory = true;
                saveFile.FileName = name + ".txt";
                result = saveFile.ShowDialog();
                path = Path.GetFullPath(saveFile.FileName);
            }
            
            if (result == DialogResult.OK)
            {
                string filePathFileName = path;
                try
                {
                    write = new StreamWriter(filePathFileName);
                    write.WriteLine(output);
                    write.Close();
                    MessageBox.Show("File saved successfully!");
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an issue when trying to write to " + filePathFileName + "." + ex.ToString());
                }
            }
            return false;
        }

        public string[] loadEncryptedMessageFromFolder()
        {
            OpenFileDialog fileBrowserDialog = new OpenFileDialog();
            fileBrowserDialog.Title = "Select which file to load encrypted message from.";
            fileBrowserDialog.Filter = "TXT files|*.txt";
            fileBrowserDialog.InitialDirectory = @"C:\";
            if (fileBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string filePathFileName = fileBrowserDialog.FileName; // file will always exist when selected from dialog
                try
                {
                    read = new StreamReader(filePathFileName);
                    string line = read.ReadLine(); // I save as shitfBy|text, so always only 1 line 
                    if(line.Length > 0 && line.Contains("|"))
                    {
                        string[] content = line.Split('|');
                        read.Close();
                        MessageBox.Show("Encrypted message from file '" + filePathFileName + "' was successfully loaded!");
                        return content;
                    }
                    else
                    {
                        MessageBox.Show("The file you are tyring to open seems unusual!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an issue when trying to open " + filePathFileName + "." + ex.ToString());
                }
            }
            return null;
        }
    }
}
