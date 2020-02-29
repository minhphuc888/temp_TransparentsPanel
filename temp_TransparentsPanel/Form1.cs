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

        private void form_MouseClick(object sender, MouseEventArgs e)
        {
            Rectangle rc = RectangleDrawer.Draw(this);
            Console.WriteLine(rc.ToString());
        }
    }
}
