using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Resources;
using System.Threading;

namespace WordMerger
{
    public partial class AdvancedFrom : Form
    {
        private List<string> pathList = new List<string>();

        public AdvancedFrom()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
            ResourceManager rm = new ResourceManager("WordMerger.strings", typeof(AdvancedFrom).Assembly);
            InitializeComponent();

            //Set localization strings
            this.Text = rm.GetString("AppName");
            fileToolStripMenuItem.Text = rm.GetString("fileToolStripItem");
            closeToolStripMenuItem.Text = rm.GetString("closeToolstripItem");
            AddFile.Text = rm.GetString("AddFileMenuButton");
            upButton.Text = rm.GetString("upButton");
            downButton.Text = rm.GetString("downButton");
            startButton.Text = rm.GetString("startButton");
            refreshButton.Text = rm.GetString("refreshButton");
            RemoveButton.Text = rm.GetString("RemoveButton");
            addFilesToolStripMenuItem.Text = rm.GetString("AddFileMenuButton");
            mergeDocumentsToolStripMenuItem.Text = rm.GetString("startButton");
            helpToolStripMenuItem.Text = rm.GetString("Help");
            changeLanguageToolStripMenuItem.Text = rm.GetString("ChangeLanguage");

            List<string> args = Environment.GetCommandLineArgs().ToList();
            args.RemoveAt(0);
            pathList.AddRange(args);
            FillList(); 
        }

        private void FillList()
        {
            checkedListBox1.Items.Clear();
            foreach (string path in pathList)
            {
                checkedListBox1.Items.Add(Path.GetFileName(path));
            }
        }

        public void addEntries(string[] entries, int start = 0)
        {
            for (int i = start; i < entries.Length; i++)
            {
                if (!pathList.Contains(entries[i]))
                {
                    pathList.Add(entries[i]);
                }
            }
            FillList();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                addEntries(openFileDialog1.FileNames);
            }
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            FillList();
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedIndices.Count > 0)
            {
                pathList.RemoveAt(checkedListBox1.CheckedIndices[0]);
                FillList();
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (e.Index != i)
                    {
                        checkedListBox1.SetItemChecked(i, false);
                    }
                }
            }
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            int checkedListItem = checkedListBox1.CheckedIndices[0];
            if (checkedListItem - 1 >= 0 && checkedListItem - 1 < pathList.Count)
            {
                string path = pathList[checkedListItem];
                pathList.RemoveAt(checkedListItem);
                pathList.Insert(checkedListItem - 1, path);
                FillList();
                checkedListBox1.SetItemChecked(checkedListItem - 1, true);
            }
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            int checkedListItem = checkedListBox1.CheckedIndices[0];
            if (checkedListItem + 1 >= 0 && checkedListItem + 1 < pathList.Count)
            {
                string path = pathList[checkedListItem];
                pathList.RemoveAt(checkedListItem);
                pathList.Insert(checkedListItem + 1, path);
                FillList();
                checkedListBox1.SetItemChecked(checkedListItem + 1, true);
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (pathList.Count > 1)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string savePath = saveFileDialog1.FileName;
                    Merger myMerger = new Merger(pathList.ToArray(), savePath);
                    myMerger.StartMergeDocument();
                }
            }
            else
            {
                MessageBox.Show("Please 2 or more select files to merge");
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void changeLanguageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeLanguage ch = new ChangeLanguage();
            ch.ShowDialog();
        }
    }
}
