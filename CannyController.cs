using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Recognition1
{
    public partial class CannyController : Form
    {
        Form1 home;
        public CannyController()
        {
            InitializeComponent();
        }

        public CannyController(Form1 home)
        {
            InitializeComponent();
            this.home = home;

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            home.MakeCannyImage((double)numericThreshold.Value, (double)numericThreshOldLink.Value);
        }
    }
}
