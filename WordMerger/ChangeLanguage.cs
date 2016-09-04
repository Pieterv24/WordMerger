using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordMerger
{
    public partial class ChangeLanguage : Form
    {
        ResourceManager rm = new ResourceManager("WordMerger.strings", typeof(ChangeLanguage).Assembly);
        private string[] languages = {"English", "Nederlands"};
        private string[] localeCodes = {"en-US", "nl-NL"};

        public ChangeLanguage()
        {
            
            InitializeComponent();
            this.Text = rm.GetString("ChangeLanguage");
            comboBox1.Items.AddRange(languages);
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Language = localeCodes[comboBox1.SelectedIndex];
            Properties.Settings.Default.Save();
            MessageBox.Show(rm.GetString("RestartRequest"));
            this.Close();
        }
    }
}
