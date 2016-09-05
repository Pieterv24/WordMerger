using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace WordMerger
{
    public partial class MainProgram : Form
    {
        private string[] args = Environment.GetCommandLineArgs();
        private bool recievedArgs = false;

        public MainProgram()
        {
            InitializeComponent();
            if (args.Length > 1)
            {
                recievedArgs = true;
                textBox1.Text = arrayToString(args, 1);
                textBox1.Enabled = false;
                button1.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox1.Text = arrayToString(openFileDialog1.FileNames);
            args = openFileDialog1.FileNames;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (args.Length > 1)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string savePath = saveFileDialog1.FileName;
                    Merger myMerger = recievedArgs ? new Merger(args, savePath, 1) : new Merger(args, savePath);
                    myMerger.StartMergeDocument();
                }
            }
            else
            {
                MessageBox.Show("Please select a source");
            }
        }

        string arrayToString(string[] array, int start = 0)
        {
            string result = "";
            for (int i = start; i < array.Length; i++)
            {
                if (i != array.Length - 1)
                {
                    result += array[i] + ",";
                }
                else
                {
                    result += array[i];
                }
            }
            return result;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
