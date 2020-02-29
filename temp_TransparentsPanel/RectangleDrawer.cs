using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace temp_TransparentsPanel
{
    public static class RectangleDrawer
    {
        private static Form mMask;
        private static Point mPos;
        private static Form f;

        public static Rectangle Draw(Form parent)
        {
            // Record the start point
            mPos = parent.PointToClient(Control.MousePosition);
            f = parent;
            // Create a transparent form on top of <frm>
            mMask = new Form();
            mMask.FormBorderStyle = FormBorderStyle.None;
            mMask.BackColor = Color.Magenta;
            mMask.TransparencyKey = mMask.BackColor;
            mMask.ShowInTaskbar = false;
            mMask.StartPosition = FormStartPosition.Manual;
            mMask.Size = parent.Size;
            mMask.Location = parent.PointToScreen(Point.Empty);
            mMask.MouseMove += MouseMove;
            mMask.MouseUp += MouseUp;
            mMask.Paint += PaintRectangle;
            mMask.Load += DoCapture;
            // Display the overlay
            mMask.ShowDialog(parent);
            // Clean-up and calculate return value
            mMask.Dispose();
            mMask = null;
            Point pos = parent.PointToClient(Control.MousePosition);
            int x = Math.Min(mPos.X, pos.X);
            int y = Math.Min(mPos.Y, pos.Y);
            int w = Math.Abs(mPos.X - pos.X);
            int h = Math.Abs(mPos.Y - pos.Y);
            return new Rectangle(x, y, w, h);
        }
        private static void DoCapture(object sender, EventArgs e)
        {
            // Grab the mouse
            mMask.Capture = true;
        }
        private static void MouseMove(object sender, MouseEventArgs e)
        {
            // Repaint the rectangle
            mMask.Invalidate();
        }
        private static void MouseUp(object sender, MouseEventArgs e)
        {
            // Done, close mask
            mMask.Close();
            Paint(f);
        }



        private static void PaintRectangle(object sender, PaintEventArgs e)
        {
            // Draw the current rectangle
            pos = mMask.PointToClient(Control.MousePosition);
            using (Pen pen = new Pen(Brushes.Black))
            {
                pen.DashStyle = DashStyle.Dot;
                e.Graphics.DrawLine(pen, mPos.X, mPos.Y, pos.X, mPos.Y);
                e.Graphics.DrawLine(pen, pos.X, mPos.Y, pos.X, pos.Y);
                e.Graphics.DrawLine(pen, pos.X, pos.Y, mPos.X, pos.Y);
                e.Graphics.DrawLine(pen, mPos.X, pos.Y, mPos.X, mPos.Y);
            }
        }

        static Point pos;
        private static void Paint(Form parent)
        {
            using (Pen pen = new Pen(Brushes.Black))
            {
                pen.DashStyle = DashStyle.Dot;

                Bitmap myBitmap = new Bitmap(parent.Width, parent.Height);
                var g = Graphics.FromImage(myBitmap);

                g.DrawLine(pen, mPos.X, mPos.Y, pos.X, mPos.Y);
                g.DrawLine(pen, pos.X, mPos.Y, pos.X, pos.Y);
                g.DrawLine(pen, pos.X, pos.Y, mPos.X, pos.Y);
                g.DrawLine(pen, mPos.X, pos.Y, mPos.X, mPos.Y);

                Bitmap bmp = new Bitmap(100, 100, g);
                parent.BackgroundImage = myBitmap;
            }

        }
    }
}
