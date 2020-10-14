using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace USB_Testing
{
    public partial class SetConfigLabels : Form
    {
        public SetConfigLabels()
        {
            InitializeComponent();
        }

        private void SetConfigLabels_Load(object sender, EventArgs e)
        {
            USB2_Label.Text = Settings1.Default.USB_2_LABEL;
            USB3_Label.Text = Settings1.Default.USB_3_LABEL;
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            // Set the settings from this Form
            Settings1.Default.USB_2_LABEL = USB2_Label.Text;
            Settings1.Default.USB_3_LABEL = USB3_Label.Text;
            // Save new settings
            Settings1.Default.Save();
            Close();

        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
