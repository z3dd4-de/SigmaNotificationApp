using System;
using System.Windows.Forms;

namespace SigmaNotificationApp
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
        }

        private void HelpForm_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate(System.Environment.CurrentDirectory + "\\help.html");
        }
    }
}
