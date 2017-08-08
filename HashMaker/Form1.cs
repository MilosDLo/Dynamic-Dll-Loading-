using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HashMaker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void generateHashBtn_Click(object sender, EventArgs e)
        {
            //string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathBox.Text);
            if (pathBox.Text == "not selected")
            {
                label2.Show();
                return;
            }
            string filePath = pathBox.Text;
            string hash = HashGenerator.GetHashFromFile(filePath, HashGenerator.SHA256);

            hashBox.Text = hash;
            File.Delete(pathBox.Text);
            pathBox.Text = "not selected";
        }

        private void selectBtn_Click(object sender, EventArgs e)
        {
            label2.Hide();
            hashBox.Text = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.AddExtension = true;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {

                    string currentPath = AppDomain.CurrentDomain.BaseDirectory;  //esync sqlite nacin 


                    string fileToWriteTo = Path.Combine(currentPath, Path.GetFileName(fileName));

                    File.Copy(fileName, fileToWriteTo);
                    pathBox.Text = fileToWriteTo;
                }
            }
        }
    }
}
