using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Resources;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace WordMerger
{
    public partial class Merger : Form
    {
        private string[] paths;
        private string destination;
        private int start;
        BackgroundWorker bgw = new BackgroundWorker();
        private string[] allowedExtentions = {".doc", ".docx", ".odt"};

        public Merger(string[] paths, string destination, int start = 0)
        {
            ResourceManager rm = new ResourceManager("WordMerger.strings", typeof(AdvancedFrom).Assembly);

            InitializeComponent();

            //Add localization code
            this.Text = rm.GetString("MergeWindowTitle");
            button2.Text = rm.GetString("Done");
            button1.Text = rm.GetString("Cancel");

            this.paths = paths;
            this.destination = destination;
            this.start = start;
            this.Show();
        }

        public void StartMergeDocument()
        {
            progressBar1.Minimum = 1;
            progressBar1.Maximum = paths.Length - start;
            progressBar1.Step = 1;
            progressBar1.Value = 1;

            bgw.WorkerSupportsCancellation = true;
            bgw.ProgressChanged += bgw_ProgressChanged;
            bgw.DoWork += bgw_mergeDocument;
            bgw.WorkerReportsProgress = true;
            bgw.RunWorkerCompleted += bgw_RunCompleted;
            bgw.RunWorkerAsync();
        }

        private void bgw_mergeDocument(object sender, DoWorkEventArgs e)
        {
            bool madeChange = false;
            Word.Application application = new Word.Application();
            Word.Application readApplication = new Word.Application();
            Word.Document resultDocument = application.Documents.Add();
            for (int i = start; i < paths.Length; i++)
            {
                if (bgw.CancellationPending)
                {
                    madeChange = false;
                    Console.WriteLine("canceled");
                    break;
                }
                if (allowedExtentions.Contains(Path.GetExtension(paths[i])))
                {
                    madeChange = true;
                    Word.Document document = readApplication.Documents.Open(@paths[i], ReadOnly: true);
                    foreach (Word.Paragraph paragraph in document.Content.Paragraphs)
                    {
                        resultDocument.Content.Paragraphs.Last.Range.Font.Name = paragraph.Range.Font.Name;
                        resultDocument.Content.Paragraphs.Last.Range.Font.Size = paragraph.Range.Font.Size;
                        resultDocument.Content.Paragraphs.Last.Range.Text = paragraph.Range.Text;
                    }
                    document.Close();
                }
                ((BackgroundWorker)sender).ReportProgress(2);
            }
            if (madeChange && !bgw.CancellationPending)
            {
                resultDocument.SaveAs(@destination);
            }
            resultDocument.Close(false);
            application.Quit();
            readApplication.Quit();
        }

        private void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.PerformStep();
            Console.WriteLine("step taken");
        }

        private void bgw_RunCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Value = progressBar1.Maximum;
            button1.Enabled = false;
            button2.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bgw.CancelAsync();
            button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
