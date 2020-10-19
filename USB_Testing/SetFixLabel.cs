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
    public partial class SetFixLabel : Form
    {
        public SetFixLabel()
        {
            InitializeComponent();
        }

        private void SetFixLabel_Load(object sender, EventArgs e)
        {
            LabelSuffixTextBox.Text = Settings1.Default.FIX_LABEL;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            if(LabelSuffixTextBox.Text != "")
            {
                DialogResult UserOpt = MessageBox.Show("Confirm the new Fix Label?", "User Confirm", MessageBoxButtons.OKCancel);
                if (UserOpt == DialogResult.OK)
                {
                    Settings1.Default.FIX_LABEL = LabelSuffixTextBox.Text;
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Enter a Label Identifier");
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
