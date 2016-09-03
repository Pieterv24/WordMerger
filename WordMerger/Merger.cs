using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace WordMerger
{
    public partial class Merger : Form
    {
        private string[] paths;
        private string destination;
        private int start;
        private bool cancelationInvoked = false;

        public Merger(string[] paths, string destination, int start = 0)
        {
            InitializeComponent();
            this.paths = paths;
            this.destination = destination;
            this.start = start;
            this.Show();
        }

        public void StartMergeDocument()
        {
            bool madeChange = false;
            Word.Application application = new Word.Application();
            Word.Application readApplication = new Word.Application();
            Word.Document resultDocument = application.Documents.Add();
            progressBar1.Minimum = 1;
            progressBar1.Maximum = paths.Length - start;
            progressBar1.Step = 1;
            progressBar1.Value = 1;
            for (int i = start; i < paths.Length; i++)
            {
                if (Path.GetExtension(paths[i]) == ".docx" || Path.GetExtension(paths[i]) == ".doc")
                {
                    madeChange = true;
                    Word.Document document = readApplication.Documents.Open(@paths[i], ReadOnly: true);
                    resultDocument.Content.Text += document.Content.Text;
                    document.Close();
                }
                if (cancelationInvoked)
                {
                    madeChange = false;
                    button2.Enabled = true;
                    break;
                }
                progressBar1.PerformStep();
                progressBar1.Refresh();
            }
            button2.Enabled = true;
            button1.Enabled = false;
            if (madeChange)
            {
                resultDocument.SaveAs(@destination);
            }
            resultDocument.Close();
            application.Quit();
            readApplication.Quit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cancelationInvoked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
