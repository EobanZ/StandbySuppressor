using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace StandbySuppressor
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (StandbySuppressor.isStandbySuppressed == true)
                StandbySuppressor.EnableStandby();
        }

        private void btnToggle_Click(object sender, EventArgs e)
        {
            StandbySuppressor.SuppresionMode mode = (StandbySuppressor.SuppresionMode)this.cmbMethod.SelectedIndex;

            if(StandbySuppressor.isStandbySuppressed != true)
            {
                StandbySuppressor.isStandbySuppressed = StandbySuppressor.DisableStandby(mode, this.chbDisplay.Checked);
            }
            else
            {
                StandbySuppressor.isStandbySuppressed = !StandbySuppressor.EnableStandby();
            }


            if(StandbySuppressor.isStandbySuppressed)
            {
                this.lblStatus.Text = "On";
                this.lblStatus.ForeColor = Color.Green;
            }
            else 
            {
                this.lblStatus.Text = "Off";
                this.lblStatus.ForeColor = Color.Red;
            }
        }


    }

    
    
}
