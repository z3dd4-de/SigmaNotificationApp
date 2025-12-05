using System;
using System.Windows.Forms;

namespace SigmaNotificationApp
{
    public partial class HelpForm : Form
    {
        private string file = "help.html";
        public HelpForm()
        {
            InitializeComponent();
        }

        private void HelpForm_Load(object sender, EventArgs e)
        {
            switchLanguage();
            webBrowser1.Navigate(System.Environment.CurrentDirectory + $"\\{file}");
        }

        private void switchLanguage()
        {
            switch (Properties.Settings.Default.Language)
            {
                case "de":
                    file = "help.html";
                    this.Text = "Hilfe";
                    break;
                case "en":
                    file = "help_en.html";
                    this.Text = "Help";
                    break;
                default:
                    file = "help_en.html";
                    this.Text = "Help";
                    break;
            }
        }
    }
}
