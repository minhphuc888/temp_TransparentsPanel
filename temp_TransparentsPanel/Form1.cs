using System;
using System.Drawing;
using System.Windows.Forms;

namespace temp_TransparentsPanel
{
    public partial class form : Form
    {
        public form()
        {
            InitializeComponent();
        }

        private void pBox_Click(object sender, EventArgs e)
        {
            Rectangle rc = RectangleDrawer.Draw(pBox);
            Console.WriteLine(rc.ToString());
        }
    }
}
